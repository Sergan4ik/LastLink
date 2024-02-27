using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameCore;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using ZergRush.CodeGen;
using ZergRush.ReactiveCore;

namespace Game.GameCore
{
    public partial class Unit : RTSContextNode, IActionSource
    {
        public string sourceName => $"Unit ${cfg.name}_{nodeId}";
        public Faction faction => (Faction)parent;
        public RTSTransform transform;

        [GenIgnore, JetBrains.Annotations.CanBeNull] 
        public UnitView view;

        public List<UnitAction> unitActions;

        public UnitStatsContainer stats;
        public ref float hp => ref stats.Health;
        public float maxHp => stats.MaxHealth;
        public float moveSpeed => stats.MoveSpeed;
        public UnitConfig cfg;
        
        public bool isMoving => unitActions.Any(a => a is UnitMove);
        
        public void Init(UnitConfig cfg, int level)
        {
            this.cfg = cfg;
            var levelCfg = cfg.GetLevelConfig(level);
            stats.UpdateFrom(levelCfg.stats);
        }
        
        public void Tick(float dt)
        {
            foreach (var unitAction in unitActions)
            {
                unitAction.Tick(dt);
            }

            for (int i = unitActions.Count - 1; i >= 0; i--)
            {
                if (unitActions[i].state == ActionState.Finished)
                {
                    logger.Log($"Unit action {unitActions[i].GetType()}_{unitActions[i].nodeId} expired");
                    unitActions.RemoveAt(i);
                }
            }
        }

        public void SetupAction(UnitAction actionPrototype)
        {
            UnitAction action = CreateChild(actionPrototype);
            action.Init();
            unitActions.Add(action);
        }
        
        public void MoveTo(Vector3 destination)
        {
            if (unitActions.Any(a => a is UnitMove))
            {
                unitActions.Find(a => a is UnitMove).Terminate(this);
            }
            SetupAction(new UnitMove {globalDestination = destination, moveSpeed = moveSpeed});
        }

        public void DealRawDamage(float dps)
        {
            if (hp > dps)
                hp -= dps;
            else
                hp = 0;
        }
    }
}