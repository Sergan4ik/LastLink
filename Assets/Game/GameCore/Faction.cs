using System.Collections.Generic;
using ZergRush;
using ZergRush.ReactiveCore;

namespace Game.GameCore
{
    public enum FactionType
    {
        Humans,
        Scavengers
    }

    public partial class Faction : RTSContextNode
    {
        public FactionType factionType;
        public ReactiveCollection<Unit> units;

        public void Init(IEnumerable<Unit> unitPrototypes)
        {
            foreach (var unitPrototype in unitPrototypes)
            {
                units.Add(CreateChild(unitPrototype));
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