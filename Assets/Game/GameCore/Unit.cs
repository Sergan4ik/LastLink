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
    [GenMultipleRefs]
    public partial class Unit : RTSRuntimeData, IActionSource
    {
        public string sourceName => $"Unit ${cfg.name}";
        public RTSTransform transform;

        [GenIgnore, JetBrains.Annotations.CanBeNull] 
        public UnitView view;

        public List<UnitAction> unitActions;
        
        public UnitStatsContainer stats;
        public ref float hp => ref stats.Health;
        public float maxHp => stats.MaxHealth;
        public float moveSpeed => stats.MoveSpeed;
        public UnitConfig cfg;
        public FactionSlot factionSlot;
        
        public AnimationData currentAnimation;
        public Faction UnitFaction(GameModel gameModel) => gameModel.GetFactionBySlot(factionSlot);
        public bool isMoving => unitActions.Any(a => a is UnitMove);
        
        public void Init(GameModel model, FactionSlot factionSlot, UnitConfig cfg, int level)
        {
            this.factionSlot = factionSlot;
            this.cfg = cfg;
            var levelCfg = cfg.GetLevelConfig(level);
            stats.UpdateFrom(levelCfg.stats);
            
            PlayIdle();
        }
        
        public void PlayAnimation(AnimationData animationData)
        {
            currentAnimation = new AnimationData();
            currentAnimation.UpdateFrom(animationData);
            currentAnimation.Init();
        }

        public void PlayIdle()
        {
            PlayAnimation(cfg.idleAnimation);
        }
        
        public void Tick(GameModel gameModel, float dt)
        {
            if (currentAnimation.timer.Tick(dt) && currentAnimation.loop == false)
                PlayIdle();
            
            foreach (var unitAction in unitActions)
            {
                unitAction.Tick(gameModel, dt, this);
            }

            for (int i = unitActions.Count - 1; i >= 0; i--)
            {
                if (unitActions[i].state == ActionState.Finished)
                {
                    Debug.Log($"Unit action {unitActions[i].sourceName} expired");
                    unitActions.RemoveAt(i);
                }
            }
        }

        public void SetupAction(GameModel gameModel,UnitAction action)
        {
            action.Init(gameModel, this);
            unitActions.Add(action);
        }
        
        public void MoveTo(GameModel gameModel, Vector3 destination)
        {
            if (unitActions.Any(a => a is UnitMove))
            {
                unitActions.Find(a => a is UnitMove).Terminate(gameModel, this, this);
            }
            SetupAction(gameModel, new UnitMove {globalDestination = destination, moveSpeed = moveSpeed});
        }

        public void DealRawDamage(float dps)
        {
            if (hp > dps)
                hp -= dps;
            else
                hp = 0;
        }
    }
    
    public partial class AnimationData : RTSRuntimeData
    {
        public string animationName;
        public float duration;
        public bool loop;
        public RTSTimerIntervals timer;
        public float normilizedTime => timer.totalTimeElapsed / duration;

        public void Init()
        {
            timer = new RTSTimerIntervals()
            {
                intervals = new List<float> { duration },
                loop = loop
            };
        }
    }
}