using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using ZergRush;
using ZergRush.ReactiveCore;

namespace Game.GameCore
{
    public enum FactionType
    {
        Humans,
        Scavengers
    }

    public enum FactionSlot
    {
        Player1,
        Player2,
        Player3,
        Player4,
        Neutral
    }

    public partial class ControlData : RTSRuntimeData
    {
        public int serverPlayerId = -1;
        public FactionSlot factionSlot;
    }

    public partial class Faction : RTSRuntimeData, IActionSource 
    {
        public ControlData FactionControlData(GameModel gameModel) => gameModel.controlData.FirstOrDefault(cd => cd.factionSlot == slot);
        public FactionSlot slot;
        public string sourceName => $"Faction {factionType}";
        public FactionType factionType;

        public void Init(GameModel gameModel, IEnumerable<UnitConfig> unitConfigs)
        {
            foreach (var unitConfig in unitConfigs)
            {
                var unitPrototype = new Unit();
                unitPrototype.Init(gameModel, slot, unitConfig, 0);

                gameModel.units.Add(unitPrototype);
            }
        }
        
        public void MoveStackTo(GameModel gameModel, List<Unit> stack, Vector3 destination)
        {
            if (stack.Any(u => u.factionSlot != slot))
                return;
            
            Unit mainUnit = stack[0];
            for (var i = 0; i < stack.Count; i++)
            {
                Vector3 offset = stack[i].transform.position - mainUnit.transform.position;
                stack[i].MoveTo(gameModel, destination + offset);
                // Debug.Log($"Moving {stack[i]} to {destination + offset}");
            }
        }

        public void Tick(GameModel gameModel, float dt)
        {
        }
    }
}