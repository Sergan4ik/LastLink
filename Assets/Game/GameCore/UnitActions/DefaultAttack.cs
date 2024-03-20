using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GameCore
{
    public partial class DefaultAttack : UnitAction
    {
        public RTSTimerIntervals attackTimer;

        protected override void InitInternal(GameModel model, Unit owner)
        {
            duration = owner.cfg.autoAttackAnimation.duration;
        }

        protected override void OnActivation(GameModel gameModel, Unit owner, RTSInput input)
        {
            owner.PlayAnimation(owner.cfg.autoAttackAnimation);
            attackTimer = new RTSTimerIntervals()
            {
                intervals = new List<float> { 0.3f, 0.7f},
                loop = false
            };
        }

        protected override void ProcessTick(GameModel model, float dt, Unit owner)
        {
            bool isCastMoment = attackTimer.Tick(dt);
            switch (attackTimer.PassedIntervals, isCastMoment)
            {
                case (1, true) :
                    model.Attack(new AttackInfo(owner, model.GetUnit(initialInput.targetData.targetId), this, 20));
                    break;
            }
        }
    }
    
}