using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

public class RTSView : ReusableView
{
    public GameView gameView => GameView.instance;
    public GameModel game => gameView.game;
    public GameConfig config => GameConfig.Instance;
}

public class GameView : RTSView
{
    public static GameView instance;
    public static GameConfig config => GameConfig.Instance;
    
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
        if (instance != null)
        {
            Debug.LogError("GameView already exists");
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        
        input = new PlayerInput();
        input.Enable();
        input.RTS.Enable();
    }

    private void OnEnable()
    {
        input.RTS.SecondaryAction.performed += OnSecondaryAction;
        input.RTS.UnitSelection.started += selectionHandler.SelectionProcess;
        input.RTS.UnitSelection.canceled += selectionHandler.SelectionProcess;
        
    }

    private void OnDisable()
    {
        input.RTS.SecondaryAction.performed -= OnSecondaryAction;
        input.RTS.UnitSelection.started -= selectionHandler.SelectionProcess;
        input.RTS.UnitSelection.canceled -= selectionHandler.SelectionProcess;
        
    }

    private void ProcessSelection(SelectionRectClipSpace rectClipSpace)
    {
        game.SelectUnitsInsideRect(rectClipSpace);
    }

    private void OnSecondaryAction(InputAction.CallbackContext ctx)
    {
        Debug.Log($"{ctx.duration} seconds passed since the button was pressed");
        if (game != null && game.gameState.value == GameState.InProgress)
        {
            if (cameraController.TryGetWorldMousePosition(out var worldMousePosition) == false) return;
            if (cameraController.TryGetPointedUnit(out var unit))
            {
                unit.SetupAction(new DOT()
                {
                    duration = 3,
                    preparingTime = 0,
                });
                
            }
            else
            {
                OnTerrainClick();
                game.factions[0].MoveSelectedTo(worldMousePosition);
            }
        }
    }
    
    private void OnTerrainClick()
    {
        var go = Instantiate(Resources.Load<GameObject>("PointMark"), cameraController.worldMousePosition, Quaternion.identity);
        Destroy(go, 1);
    }

    private void Start()
    {
    }

    public void SetupGameModel(GameModel gameModel)
    {
        gameConnections.DisconnectAll();
        unitsPresenter?.UnloadAll(true);
        
        game = gameModel;
        

        gameTicker = new IntervalRepeater()
        {
            repeatInterval = GameModel.FrameTime,
        };
        gameConnections += selectionHandler.onSelection.Subscribe(ProcessSelection);
       
        unitsPresenter = new ListPresenter<Unit, UnitView>(unitsRoot, PrefabRef<UnitView>.Auto(),
            (unit, view) => view.ShowUnit(unit), view => view.OnUnload());
        
        game.GameStart();
    }

    private void OnGameTick(float dt)
    {
        game.Tick(dt);
    }

    public void Update()
    {
        if (game != null && game.gameState.value == GameState.InProgress)
        {
            gameTicker.Update(Time.deltaTime);
            while (gameTicker.IntervalCheck())
            {
                OnGameTick(gameTicker.repeatInterval);
            }
            unitsPresenter.UpdateFrom(game.allUnits);
        }
        
        selectionHandler.SelectionTick(input.RTS.UnitSelection);
    }
}


public class IntervalRepeater
{
    public Cell<float> elapsedTime = new Cell<float>();
    public float repeatInterval;
    
    public void Update(float dt)
    {
        elapsedTime.value += dt;
    }

    public bool IntervalCheck()
    {
        if (elapsedTime.value > repeatInterval)
        {
            elapsedTime.value -= repeatInterval;
            return true;
        }

        return false;
    }
}