using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public int id;
        public string sourceName => $"Unit ${cfg.name}_{id}";
        public RTSTransform transform;

        [GenIgnore, JetBrains.Annotations.CanBeNull] 
        public UnitView view;

        public List<UnitAction> unitActions;
        
        public UnitStatsContainer stats;
        public ref float hp => ref stats.Health;
        public float maxHp => stats.MaxHealth;
        public bool isDead => hp <= 0;
        public float moveSpeed => stats.MoveSpeed;
        public UnitConfig cfg;
        public FactionSlot factionSlot;
        
        public AnimationData currentAnimation;
        
        public UnitBehaviour behaviour;
        
        public Faction UnitFaction(GameModel gameModel) => gameModel.GetFactionBySlot(factionSlot);
        public bool isMoving => unitActions.Any(a => a is UnitMove);
        
        public void Init(GameModel model, FactionSlot factionSlot, UnitConfig cfg, int level)
        {
            this.factionSlot = factionSlot;
            this.cfg = cfg;
            var levelCfg = cfg.GetLevelConfig(level);
            stats.UpdateFrom(levelCfg.stats);
            
            PlayIdle();
            behaviour.Init(model, this);
        }
        
        public void PlayAnimation(AnimationData animationDataPrototype)
        {
            currentAnimation = new AnimationData();
            currentAnimation.UpdateFrom(animationDataPrototype);
            currentAnimation.Init();
        }

        public void PlayIdle()
        {
            PlayAnimation(cfg.idleAnimation);
        }
        
        public void Tick(GameModel gameModel, float dt)
        {
            if (currentAnimation.timer.Tick(dt) && currentAnimation.loop == false && isDead == false)
                PlayIdle();
            
            behaviour.Tick(gameModel, this, dt);
            
            foreach (var unitAction in unitActions)
            {
                unitAction.Tick(gameModel, dt, this);
            }

            for (int i = unitActions.Count - 1; i >= 0; i--)
            {
                if (unitActions[i].state == ActionState.Finished)
                {
                    if (ZeroLagSettings.messageLogs)
                        Debug.Log($"Unit_#{id} action {unitActions[i].sourceName} expired");
                    unitActions.RemoveAt(i);
                }
            }
            
        }

        public void SetupAction(GameModel gameModel, UnitAction action, RTSInput input)
        {
            if (isDead)
                return;
            
            action.Init(gameModel, this);

            foreach (var unitAction in unitActions)
            {
                if (action.canExistParallel == false && unitAction.canExistParallel == false)
                {
                    unitAction.Terminate(gameModel, this, action);
                }
                else if (action.stackingPolicy == ActionStackingPolicy.Interruptable &&
                         unitAction.GetType() == action.GetType())
                {
                    unitAction.Terminate(gameModel, this, action);
                }
            }

            action.Activate(gameModel, this, input);

            unitActions.Add(action);
        }

        public void MoveTo(GameModel gameModel, RTSInput input)
        {
            SetupAction(gameModel, new UnitMove {moveSpeed = moveSpeed}, input);
        }

        public void DealRawDamage(GameModel model, AttackInfo attack)
        {
            if (hp > attack.damage)
            {
                hp -= attack.damage;
            }
            else if (hp > 0)
            {
                hp = 0;
                OnDied(model, attack.source);
            }
        }

        public void OnDied(GameModel model, IActionSource source)
        {
            foreach (var unitAction in unitActions)
            {
                unitAction.Terminate(model, this, source);
            }
            PlayAnimation(cfg.deathAnimation);
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
            if (loop == false)
                timer.intervals.Add(-1);
        }
    }
}