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

        public void Init(IEnumerable<Unit> unitPrototypes)
        {
            foreach (var unitPrototype in unitPrototypes)
            {
                units.Add(CreateChild(unitPrototype));
            }
        }

        public void MoveSelectedTo(Vector3 destination)
        {
            for (var i = 0; i < units.Count; i++)
            {
                if (units[i].IsSelected)
                {
                    Vector3 offset = units[i].transform.position - units[0].transform.position;
                    units[i].MoveTo(destination + offset);
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