using System.Collections.Generic;

namespace Game.GameCore
{
    public partial class DefaultAttack : UnitAction
    {
        public RTSTimerIntervals attackTimer;

        public Unit target;
        public float attackDamage;

        protected override void InitInternal(GameModel model, Unit owner)
        {
            attackTimer = new RTSTimerIntervals()
            {
                intervals = new List<float> { 0.5f, },
                loop = false
            };
        }

        protected override void ProcessTick(GameModel model, float dt, Unit owner1)
        {
            attackTimer.Tick(dt);
            switch (attackTimer.PassedIntervals)
            {
                case 0:
                    break;
                case 1:
                    break;
            }
        }
    }
}