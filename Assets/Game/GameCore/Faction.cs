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

    public partial class Faction : RTSContextNode, IActionSource
    {
        public ControlData factionControlData => gameModel.controlData.FirstOrDefault(cd => cd.factionSlot == slot);
        public FactionSlot slot;
        public string sourceName => $"Faction {factionType}";
        public FactionType factionType;
        public ReactiveCollection<Unit> units;

        public void Init(IEnumerable<UnitConfig> unitConfigs)
        {
            foreach (var unitConfig in unitConfigs)
            {
                var unitPrototype = new Unit();
                unitPrototype.Init(unitConfig, 0);
                
                units.Add(CreateChild(unitPrototype));
            }
        }
        
        public void MoveStackTo(List<Unit> stack, Vector3 destination)
        {
            if (stack.Any(u => u.faction != this))
                return;
            
            Unit mainUnit = stack[0];
            for (var i = 0; i < stack.Count; i++)
            {
                Vector3 offset = stack[i].transform.position - mainUnit.transform.position;
                stack[i].MoveTo(destination + offset);
                Debug.Log($"Moving {stack[i]} to {destination + offset}");
            }
        }

        public void Tick(float dt)
        {
            for (var i = 0; i < units.Count; i++)
            {
                units[i].Tick(dt);
            }
        }

    }
}