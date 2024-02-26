using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Game.GameCore
{
    public partial class UnitAction : RTSContextNode, IActionSource
    {
        public string sourceName => $"Action {this.GetType()} #{nodeId}";
        public Unit owner => (Unit)parent;
        
        public float preparingTime = 0;
        public float duration = -1;

        public ActionState state = ActionState.NotStarted;

        public RTSTimerIntervals stateTimer;

        public void Init()
        {
            stateTimer = new RTSTimerIntervals()
            {
                intervals = new List<float> { preparingTime, duration },
                loop = false
            };
            InitInternal();
        }

        protected virtual void InitInternal()
        {
            
        }
        
        public void Tick(float dt)
        {
            if (state == ActionState.Finished) return;
            
            stateTimer.Tick(dt);
            GetCurrentActionState();

            switch (state)
            {
                case ActionState.Preparing:
                    PreparingTick(dt);
                    break;
                case ActionState.Processing:
                    ProcessTick(dt);
                    break;
            }
        }

        private void GetCurrentActionState()
        {
            state = stateTimer.PassedIntervals switch
            {
                0 => ActionState.Preparing,
                1 => ActionState.Processing,
                2 => ActionState.Finished,
                _ => state
            };
        }

        public void Terminate(IActionSource source)
        {
            logger.Log($"Action {this.GetType()} terminated by {source.sourceName}");
            state = ActionState.Finished;
        }

        protected virtual void PreparingTick(float dt)
        {
            
        }
        protected virtual void ProcessTick(float dt)
        {
            
        }

    }

    public enum ActionState
    {
        NotStarted,
        Preparing,
        Processing,
        Finished
    }
}