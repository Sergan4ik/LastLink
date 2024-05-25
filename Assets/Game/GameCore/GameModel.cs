using System;
using System.Collections.Generic;
using System.Linq;
using Game.NodeArchitecture;
using UnityEngine;
using UnityEngine.Serialization;
using ZergRush;
using ZergRush.CodeGen;
using ZergRush.ReactiveCore;
using ZeroLag;

namespace Game.GameCore
{
    public partial class RTSContextRoot : ContextRoot { }

    public partial class RTSContextNode : ContextNode
    {
        // public GameModel gameModel => (GameModel)root;
        // public ILogger logger => gameModel.logger;
    }

    public interface IBattlePart
    {
        public void Init(GameModel model);
    }
    
    public enum GameState
    {
        NotStarted,
        InProgress,
        Paused,
        Finished
    }

    public interface IActionSource
    {
        public string sourceName { get; }
    }
    public class OneTimeActionSource : IActionSource
    {
        public string sourceName { get; }
        public OneTimeActionSource(string sourceName)
        {
            this.sourceName = sourceName;
        }
    }

    [GenTask(GenTaskFlags.PolymorphicDataPack)]
    public partial class RTSRuntimeData { }
    
    [GenTask(GenTaskFlags.SimpleDataPack)]
    public partial class GameModel : IActionSource, IDeterministicRealtimeModel<GameModel>
    {
        public ReactiveCollection<ControlData> controlData;
        public List<short> readyPlayers;
        public string sourceName => $"GameModel";
        public const int TargetFps = 60;
        public const float FrameTime = 1.0f / TargetFps;
        public const int FrameTimeMS = (int)(FrameTime * 1000);
        
        public ReactiveCollection<Faction> factions;
        public ReactiveCollection<Unit> units;
        public ZergRandom random;
        
        public RTSStopWatch stopWatch;

        public Cell<GameState> gameState;

        public IEnumerable<Unit> allUnits => units;

        private int idFactory;
        
        public Unit GetUnit(int id) => units.FirstOrDefault(u => u.id == id);
 
        public void Init(IEnumerable<Faction> factions)
        {
            foreach (var faction in factions)
            {
                this.factions.Add(faction);
            }
        }
        
        public Unit CreateUnit(FactionSlot slot, UnitConfig cfg, int level)
        {
            var unit = new Unit
            {
                id = idFactory++
            };
            unit.Init(this, slot, cfg, level);
            units.Add(unit);
            return unit;
        }

        public void GameStart()
        {
            stopWatch.MarkCircle();
            gameState.value = GameState.InProgress;
        }

        public void Touch()
        {
            Tick(0);
        }

        public void ApplyInput(InputCommand input)
        {
            Faction playerFaction = GetFactionByServerPlayerId(input.serverPlayerId);
            List<Unit> stack = input.input.targetData.sourceIds.Select(GetUnit).ToList();

            if (stack.Any(u => u.factionSlot != playerFaction.slot))
            {
                Debug.LogError($"Player {input.serverPlayerId} tried to control units from different factions");
            }
            
            Unit target = GetUnit(input.input.targetData.targetId);
            switch (input.input.inputType)
            {
                case RTSInputType.Move:
                    playerFaction.MoveStackTo(this, stack, input.input.targetData.worldPosition, (input.input.flags & RTSInputFlags.IsDirectionalModifier) != 0);
                    break;
                case RTSInputType.AutoAttack:
                    playerFaction.AutoAttackStack(this, stack, input.input);
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            foreach (var unit in stack)
            {
                unit.behaviour.ProcessInput(this, unit, input.input);
            }
        }

        public Faction GetFactionBySlot(FactionSlot slot) => factions.FirstOrDefault(f => f.slot == slot);

        public ControlData GetControlDataByServerPlayerId(short serverPlayerId)
        {
            return controlData.FirstOrDefault(cd => cd.serverPlayerId == serverPlayerId);
        }
        public ControlData GetControlDataByGlobalPlayerId(long globalPlayerId)
        {
            return controlData.FirstOrDefault(cd => cd.globalPlayerId == globalPlayerId);
        }
        public Faction GetFactionByServerPlayerId(short serverPlayerId)
        {
            var cd = GetControlDataByServerPlayerId(serverPlayerId);
            if (cd == default)
            {
                Debug.LogError($"Player with id {serverPlayerId} is not connected");
                return null;
            }
            
            return GetFactionBySlot(cd.factionSlot);
        }
        public Faction GetFactionByGlobalId(long globalPlayerId)
        {
            var cd = GetControlDataByGlobalPlayerId(globalPlayerId);
            if (cd == default)
            {
                Debug.LogError($"Player with id {globalPlayerId} is not connected");
                return null;
            }
            
            return GetFactionBySlot(cd.factionSlot);
        }


        public List<Unit> GetUnitsInsideOpaqueQuadrangle(SelectionRectClipSpace selectionRectClipSpace, Func<Unit, bool> skipIf = null)
        {
            // (Vector2 leftBottom2, Vector2 rightBottom2, Vector2 leftUpper2, Vector2 rightUpper2) =
            //     NormalizeCorners(selectionRectClipSpace.leftBottom, selectionRectClipSpace.rightBottom, selectionRectClipSpace.leftTop, selectionRectClipSpace.rightTop);

            if (skipIf == null)
                skipIf = u => false;
            
            List<Unit> result = new List<Unit>();
            
            foreach (var unit in allUnits)
            {
                Vector4 clipCoord = selectionRectClipSpace.unitToViewportMatrix * new Vector4(unit.transform.position.x , unit.transform.position.y, unit.transform.position.z, 1);
                Vector4 perspectiveCoord = clipCoord / -clipCoord.w;
                perspectiveCoord.x = (perspectiveCoord.x + 1) / 2;
                perspectiveCoord.y = (perspectiveCoord.y + 1) / 2;
                
                if (selectionRectClipSpace.IsPointInsideRect(perspectiveCoord) && skipIf(unit) == false)
                {
                    result.Add(unit);
                }
            }

            return result;
            
            // (Vector2 leftBottom2, Vector2 rightBottom2, Vector2 leftUpper2, Vector2 rightUpper2) NormalizeCorners(Vector3 leftBottomLocal, Vector3 rightBottomLocal, Vector3 leftUpperLocal, Vector3 rightUpperLocal)
            // {
            //     return (leftBottomLocal, rightBottomLocal, leftUpperLocal, rightUpperLocal);
            // }
        }
        
        public void Tick(float dt)
        {
            if (Math.Abs(dt - FrameTime) > 1e-5 && dt > 0)
                Debug.LogError("Frame time is not equal to target frame time");
            
            stopWatch.Tick(dt);
            
            for (var i = 0; i < factions.Count; i++)
            {
                factions[i].Tick(this, dt);
            }

            for (var i = 0; i < units.Count; i++)
            {
                var unit = units[i];
                unit.Tick(this, dt);
            }
        }

        public void Attack(GameModel model, AttackInfo info)
        {
            info.target.DealRawDamage(model, info);
        }

        public void Init()
        {
        }

        public long timeStepMs => FrameTimeMS;
        [GenInclude]
        public int step { get; set; }
        public void Update(List<ZeroLagCommand> consideredCommands)
        {
            foreach (var command in consideredCommands)
            {
                switch (command)
                {
                    case LogCommand logCommand:
                        Debug.Log(logCommand.message);
                        break;
                    case ConnectCommand connectCommand:
                        ConnectPlayer(connectCommand);
                        break;
                    case InputCommand inputCommand:
                        ApplyInput(inputCommand);
                        break;
                    case SetReadyCommand setReadyCommand:
                        SetReady(setReadyCommand);
                        break;
                    case CancelReadyCommand cancelReadyCommand:
                        CancelReady(cancelReadyCommand);
                        break;
                    case UpdateLobbyPlayerCommand updatePlayerCommand:
                        UpdateLobbyPlayer(updatePlayerCommand);
                        break;
                    case StartGameCommand startGameCommand:
                        GameStart();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            
            Tick(FrameTime);
            step++;
        }

        private void ConnectPlayer(ConnectCommand connectCommand)
        {
            if (factions.Count < controlData.Count + 1)
            {
                Debug.LogError($"This map supports only {factions.Count} players");
                return;
            }
            controlData.Add(new ControlData()
            {
                serverPlayerId = connectCommand.serverPlayerId,
                globalPlayerId = connectCommand.globalPlayerId,
                factionSlot = (FactionSlot)controlData.Count
            });
        }

        public bool IsPlayerCreated(long playerId)
        {
            return controlData.Any(c => c.globalPlayerId == playerId);
        }
    }

    public struct AttackInfo 
    {
        public Unit attacker;
        public Unit target;
        public IActionSource source;
        
        public float damage;
        
        public AttackInfo(Unit attacker, Unit target, IActionSource source, float damage)
        {
            this.attacker = attacker;
            this.target = target;
            this.source = source;
            this.damage = damage;
        }
    }

    public partial class SelectionRectClipSpace : RTSRuntimeData
    {
        public Vector2 leftBottom;
        public Vector2 rightTop;

        public Matrix4x4 unitToViewportMatrix;
        public Vector2 cameraSize;

        public float selectionDuration;
        
        public bool IsPointInsideRect(Vector2 point)
        {
            return leftBottom.x <= point.x && point.x <= rightTop.x &&
                   leftBottom.y <= point.y && point.y <= rightTop.y;
        }
        
        public float area => (rightTop.x - leftBottom.x) * (rightTop.y - leftBottom.y);
    }
}