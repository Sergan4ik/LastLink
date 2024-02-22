using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Game.GameCore;
using Sirenix.OdinInspector;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;
using ZergRush;
using ZergRush.ReactiveCore;
using ZergRush.ReactiveUI;
using ILogger = UnityEngine.ILogger;
using PlayerInput = Game.PlayerInput;

public class GameView : ConnectableMonoBehaviour
{
    public MapView mapView;
    public RTSCameraController cameraController => mapView.cameraController;
    
    public GameModel game;
    public IntervalRepeater gameTicker = new IntervalRepeater();

    public PlayerInput input;
    public SelectionHandler selectionHandler;
    
    public Connections gameConnections = new Connections();
    public Transform unitsRoot;
    
    private ListPresenter<Unit,UnitView> unitsPresenter;

    public void Awake()
    {
        input = new PlayerInput();
        input.Enable();
        input.RTS.Enable();
    }

    private void OnEnable()
    {
        input.RTS.SecondaryAction.performed += OnSecondaryAction;
        input.RTS.MainAction.started += ctx => game.allUnits[0].view.OnSelectionToggle(true);
        input.RTS.MainAction.canceled += ctx => game.allUnits[0].view.OnSelectionToggle(false);
        input.RTS.UnitSelection.started += selectionHandler.SelectionProcess;
        input.RTS.UnitSelection.canceled += selectionHandler.SelectionProcess;
    }

    private void OnDisable()
    {
        input.RTS.SecondaryAction.performed -= OnSecondaryAction;
        input.RTS.UnitSelection.started -= selectionHandler.SelectionProcess;
        input.RTS.UnitSelection.canceled -= selectionHandler.SelectionProcess;
    }
    
    private void OnSecondaryAction(InputAction.CallbackContext ctx)
    {
        Debug.Log($"{ctx.duration} seconds passed since the button was pressed");
        if (game != null && game.gameState.value == GameState.InProgress)
        {
            if (cameraController.TryGetWorldMousePosition(out var worldMousePosition) == false) return;
            OnTerrainClick();
            // QueueAction(gm =>
            // {
            //     var unit = gm.allUnits[0];
            //     return unit.MoveTo(worldMousePosition, false);
            // });
        }
    }
    
    private void OnTerrainClick()
    {
        var go = Instantiate(Resources.Load<GameObject>("PointMark"), cameraController.worldMousePosition, Quaternion.identity);
        Destroy(go, 1);
    }

    private void Start()
    {
        StartTestGame();
    }

    [Button]
    public void StartTestGame()
    {
        game = new GameModel()
        {
            logger = new UnityLogger()
        };
        Faction myFaction = new Faction();
        game.Init(new[] { myFaction });
        game.factions[0].Init(new List<Unit>()
        {
            new Unit()
            {
                moveSpeed = 15,
                transform = new RTSTransform()
                {
                    position = new Vector3(0, 0, 0),
                    rotation = Quaternion.identity
                }
            },
            new Unit()
            {
                moveSpeed = 5,
                transform = new RTSTransform()
                {
                    position = new Vector3(3, 0, 0),
                    rotation = Quaternion.identity
                }
            },
            new Unit()
            {
                moveSpeed = 5,
                transform = new RTSTransform()
                {
                    position = new Vector3(0, 0, 3),
                    rotation = Quaternion.identity
                }
            },
            new Unit()
            {
                moveSpeed = 5,
                transform = new RTSTransform()
                {
                    position = new Vector3(3, 0, 3),
                    rotation = Quaternion.identity
                }
            },
            new Unit()
            {
                moveSpeed = 5,
                transform = new RTSTransform()
                {
                    position = new Vector3(6, 0, 0),
                    rotation = Quaternion.identity
                }
            },
        });
        game.GameStart();

        gameTicker = new IntervalRepeater()
        {
            repeatInterval = GameModel.FrameTime,
        };
        gameConnections += gameTicker.onNotify.Subscribe(OnGameTick);
       
        unitsPresenter = new ListPresenter<Unit, UnitView>(unitsRoot, PrefabRef<UnitView>.Auto(),
            (unit, view) => view.UpdateFrom(unit), view => view.OnUnload());
        
        gameConnections += game.allUnits.Filter(u => u != null).Present(transform, PrefabRef<UnitView>.Auto(),
            (unit, view) =>
            {
            }, options: PresentOptions.None, delegates: TableDelegates<UnitView>.WithRemoveAnimation(view =>
            {
                Destroy(view, 2f);
                return 2;
            })
        );
    }

    private void OnGameTick(float dt)
    {
        game.Tick(dt);
        unitsPresenter.UpdateFrom(game.allUnits);
    }

    public void Update()
    {
        if (game != null && game.gameState.value == GameState.InProgress)
        {
            gameTicker.Update(Time.deltaTime);
        }
        
        selectionHandler.SelectionTick(input.RTS.UnitSelection);
    }
}


public class IntervalRepeater
{
    public Cell<float> elapsedTime = new Cell<float>();
    public float repeatInterval;
    public EventStream<float> onNotify = new EventStream<float>();
    
    public void Update(float dt)
    {
        elapsedTime.value += dt;
        while (elapsedTime.value > repeatInterval)
        {
            //log
            // Debug.Log($"IntervalRepeater: onNotify.Send(), elapsedTime: {elapsedTime.value}, repeatInterval: {repeatInterval}");
            onNotify.Send(repeatInterval);
            elapsedTime.value -= repeatInterval;
        }
    }
}