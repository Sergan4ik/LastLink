using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.GameCore;
using Game.GameCore.GameControllers;
using NUnit.Framework;
using Sirenix.OdinInspector;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.AI;
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
    public GameSession gameSession => GameSession.instance;
    public short serverPlayerId => gameSession.clientController.serverPlayerId;
}

public partial class GameView : RTSView
{
    public static GameView instance;
    public static GameConfig config => GameConfig.Instance;
    
    public MapView mapView;
    public RTSCameraController cameraController => mapView.cameraController;
    
    public GameModel game => modelGetter?.Invoke();
    
    public IntervalRepeater gameTicker = new IntervalRepeater();

    public PlayerInput input;
    public SelectionHandler selectionHandler;
    
    public Connections gameConnections = new Connections();
    public Transform unitsRoot;

    public GameUI gameUI;
    
    private ListPresenter<Unit,UnitView> unitsPresenter;

    private Func<GameModel> modelGetter;
    
    public Faction localPlayerFaction => game.GetFactionByPlayerId(serverPlayerId);
    
    [HideInInspector]
    public List<UnitView> currentSelection;

    public IEnumerable<Unit> currentSelectionModels => currentSelection.Select(v => v.currentUnit);
    
    public IEnumerable<UnitView> GetViewsByModel(IEnumerable<Unit> units) => unitsPresenter.Views().Where(uv => units.Contains(uv.currentUnit));
    public UnitView GetViewByModel(Unit unit) => unitsPresenter.Views().FirstOrDefault(uv => uv.currentUnit == unit);
    public bool IsUnitSelected(Unit unit) => currentSelectionModels.Contains(unit);

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

        input.RTS.MainAction.performed += OnMainAction;
    }

    private void OnDisable()
    {
        input.RTS.SecondaryAction.performed -= OnSecondaryAction;
        input.RTS.UnitSelection.started -= selectionHandler.SelectionProcess;
        input.RTS.UnitSelection.canceled -= selectionHandler.SelectionProcess;
        
        input.RTS.MainAction.performed -= OnMainAction;
    }

    private void OnMainAction(InputAction.CallbackContext obj)
    {
        if (game != null && game.gameState.value == GameState.InProgress)
        {
            if (cameraController.TryGetWorldMousePosition(out var worldMousePosition) == false) return;
            if (cameraController.TryGetPointedUnit(out var unit))
            {
                if (input.RTS.ControlModifier.IsPressed())
                {
                    if (IsUnitSelected(unit))
                        RemoveFromSelection(unit);
                    else
                        AddToSelection(unit);
                }
                else
                {
                    ProcessSelection(new List<Unit>(){unit}, currentSelectionModels);
                }
            }
            else
            {
                ResetSelection();
            }
        }
    }

    private void OnTerrainClick(Vector3 position)
    {
        var mark = Resources.Load<GameObject>("PointMark");
        var go = Instantiate(mark, position, mark.transform.rotation);
        Destroy(go, 1);
    }

    private void Start()
    {
    }
    
    public void SetupGameModel(Func<GameModel> gameModelGetter)
    {
        gameConnections.DisconnectAll();
        unitsPresenter?.UnloadAll(true);
        
        modelGetter = gameModelGetter;

        gameTicker = new IntervalRepeater()
        {
            repeatInterval = GameModel.FrameTime,
        };
        gameConnections += selectionHandler.onSelection.Subscribe(ProcessSelectionInput);
       
        unitsPresenter = new ListPresenter<Unit, UnitView>(u => u.cfg.name.GetUnitView() ,unitsRoot, 
            (unit, view) => view.ShowUnit(unit), view => view.OnUnload());
        
        gameSession.SendRTSCommand(new StartGameCommand());
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

        if (Keyboard.current.oKey.wasPressedThisFrame)
        {
            NavMesh.SamplePosition(cameraController.worldMousePosition, out var hit, 10, NavMesh.AllAreas);
            OnTerrainClick(hit.position);
        }
        
        selectionHandler.SelectionTick(input.RTS.UnitSelection);
    }
    
    private void OnSecondaryAction(InputAction.CallbackContext ctx)
    {
        if (game != null && game.gameState.value == GameState.InProgress)
        {
            RTSInput gameInput = null;
            if (cameraController.TryGetWorldMousePosition(out var worldMousePosition) == false) return;
            if (cameraController.TryGetPointedUnit(out var unit))
            {
                if (currentSelection.Count > 0)
                {
                    gameInput = new RTSInput()
                    {
                        targetData = GetTargetData(),
                        inputType = RTSInputType.AutoAttack
                    };
                }
            }
            else
            {
                OnTerrainClick(worldMousePosition);
                if (currentSelection.Count > 0)
                    gameInput = new RTSInput()
                    {
                        targetData = GetTargetData(),
                        inputType = RTSInputType.Move
                    };
            }
            
            if (gameInput != null)
            {
                var inputCommand = new InputCommand()
                {
                    input = gameInput,
                };
                gameSession.clientController?.WriteLocalAndSendCommand(inputCommand);
                
                if (gameSession.serverController != null)
                {
                    gameSession.serverController.EmitServerCommand(inputCommand, true);
                }
            }
        }
    }

    public TargetData GetTargetData()
    {
        return new TargetData()
        {
            sourceIds = currentSelectionModels.Select(m => m.id).ToList(),
            targetId = cameraController.pointedUnit?.id ?? -1,
            worldPosition = cameraController.worldMousePosition
        };
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