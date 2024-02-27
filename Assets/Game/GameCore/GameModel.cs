using System;
using System.Collections.Generic;
using System.Linq;
using Game.NodeArchitecture;
using UnityEngine;
using UnityEngine.Serialization;
using ZergRush;
using ZergRush.CodeGen;
using ZergRush.ReactiveCore;

namespace Game.GameCore
{
    public partial class RTSContextRoot : ContextRoot { }

    public partial class RTSContextNode : ContextNode
    {
        public GameModel gameModel => (GameModel)root;
        public ILogger logger => gameModel.logger;
    }

    public enum GameState
    {
        NotStarted,
        InProgress,
        Paused,
        Finished
    }

    public interface ISelectable
    {
        public RTSTransform Transform { get; }
        public bool IsSelected { get; set; }
        public void OnSelect();
        public void OnDeselect();
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
    public partial class GameModel : RTSContextRoot, IActionSource
    {
        public string sourceName => $"GameModel";
        public const int TargetFps = 60;
        public const float FrameTime = 1.0f / TargetFps;
        
        public ReactiveCollection<Faction> factions;
        public ZergRandom random;

        [GenIgnore]
        public ILogger logger;
        
        public Cell<GameState> gameState;

        public IEnumerable<Unit> allUnits => factions.Map(f => f.units).Join();
        public IEnumerable<ISelectable> selectables => allUnits.Cast<ISelectable>();
 
        public void Init(IEnumerable<Faction> factionPrototypes)
        {
            foreach (var factionPrototype in factionPrototypes)
            {
                factions.Add(CreateChild(factionPrototype));
            }
        }

        public void GameStart()
        {
            gameState.value = GameState.InProgress;
        }

        public void Touch()
        {
            Tick(0);
        }
        
        public List<Unit> SelectUnitsInsideRect(SelectionRectClipSpace selectionRectClipSpace)
        {
            var units = GetUnitsInsideOpaqueQuadrangle(selectionRectClipSpace);
            foreach (var unit in allUnits)
            {
                if (units.Any(u => u == unit))
                {
                    if (unit.IsSelected == false)
                    {
                        unit.IsSelected = true;
                        unit.OnSelect();
                    }
                }
                else
                {
                    if (unit.IsSelected)
                    {
                        unit.IsSelected = false;
                        unit.OnDeselect();
                    }
                }
            }

            return units;
        }
        
        public List<Unit> GetUnitsInsideOpaqueQuadrangle(SelectionRectClipSpace selectionRectClipSpace)
        {
            // (Vector2 leftBottom2, Vector2 rightBottom2, Vector2 leftUpper2, Vector2 rightUpper2) =
            //     NormalizeCorners(selectionRectClipSpace.leftBottom, selectionRectClipSpace.rightBottom, selectionRectClipSpace.leftTop, selectionRectClipSpace.rightTop);

            List<Unit> result = new List<Unit>();
            
            foreach (var unit in allUnits)
            {
                Vector4 clipCoord = selectionRectClipSpace.unitToViewportMatrix * new Vector4(unit.Transform.position.x , unit.Transform.position.y, unit.Transform.position.z, 1);
                Vector4 perspectiveCoord = clipCoord / -clipCoord.w;
                perspectiveCoord.x = (perspectiveCoord.x + 1) / 2;
                perspectiveCoord.y = (perspectiveCoord.y + 1) / 2;
                
                if (selectionRectClipSpace.IsPointInsideRect(perspectiveCoord))
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
            
            for (var i = 0; i < factions.Count; i++)
            {
                factions[i].Tick(dt);
            }
        }

    }

    public partial class SelectionRectClipSpace : RTSRuntimeData
    {
        public Vector2 leftBottom;
        public Vector2 rightTop;

        public Matrix4x4 unitToViewportMatrix;
        public Vector2 cameraSize;
        
        public bool IsPointInsideRect(Vector2 point)
        {
            return leftBottom.x <= point.x && point.x <= rightTop.x &&
                   leftBottom.y <= point.y && point.y <= rightTop.y;
        }
    }
}