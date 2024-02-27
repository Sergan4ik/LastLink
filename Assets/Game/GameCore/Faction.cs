using System.Collections.Generic;
using UnityEngine;
using ZergRush;
using ZergRush.ReactiveCore;

namespace Game.GameCore
{
    public enum FactionType
    {
        Humans,
        Scavengers
    }

    public partial class Faction : RTSContextNode, IActionSource
    {
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

        public void MoveSelectedTo(Vector3 destination)
        {
            Unit mainUnit = null;
            for (var i = 0; i < units.Count; i++)
            {
                if (units[i].IsSelected)
                {
                    if (mainUnit == null)
                        mainUnit = units[i];
                    Vector3 offset = units[i].transform.position - mainUnit.transform.position;
                    units[i].MoveTo(destination + offset);
                    Debug.Log($"Moving {units[i]} to {destination + offset}");
                }
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