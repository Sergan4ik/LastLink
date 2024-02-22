using System;
using System.Collections.Generic;
using Game.NodeArchitecture;
using UnityEngine;
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
    
    [GenTask(GenTaskFlags.SimpleDataPack)]
    public partial class RTSRuntimeData { }
    
    [GenTask(GenTaskFlags.SimpleDataPack)]
    public partial class GameModel : RTSContextRoot
    {
        public const int TargetFps = 60;
        public const float FrameTime = 1.0f / TargetFps;
        
        public ReactiveCollection<Faction> factions;
        public ZergRandom random;

        [GenIgnore]
        public ILogger logger;
        
        public Cell<GameState> gameState;

        public IReactiveCollection<Unit> allUnits => factions.Map(f => f.units).Join();

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
            onGameStarted.Send();
        }

        public void Touch()
        {
            Tick(0);
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

    public interface ILogger
    {
        public void Log(string message);
        
        public void LogError(string message);
        
        public void LogWarning(string message);
    }
}