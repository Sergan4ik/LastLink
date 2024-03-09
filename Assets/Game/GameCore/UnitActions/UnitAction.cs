using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameCore
{
    public partial class UnitAction : IActionSource
    {
        public string sourceName => $"Action {this.GetType()}";
        
        public float duration = -1;

        public ActionState state = ActionState.NotStarted;

        public RTSTimerIntervals stateTimer;

        public void Init(GameModel model, Unit owner)
        {
            stateTimer = new RTSTimerIntervals()
            {
                intervals = new List<float> { duration },
                loop = false
            };
            InitInternal(model, owner);
        }

        protected virtual void InitInternal(GameModel model, Unit owner)
        {
            
        }
        
        public virtual void Tick(GameModel model, float dt, Unit owner)
        {
            if (state == ActionState.Finished) return;
            
            stateTimer.Tick(dt);
            GetCurrentActionState();

            switch (state)
            {
                case ActionState.Processing:
                    ProcessTick(model, dt, owner);
                    break;
            }
        }

        protected virtual void ProcessTick(GameModel model, float dt, Unit owner1)
        {
            
        }

        private void GetCurrentActionState()
        {
            state = stateTimer.PassedIntervals switch
            {
                0 =>  ActionState.Processing,
                1 => ActionState.Finished,
                _ => state
            };
        }

        public void Terminate(GameModel model, Unit owner, IActionSource source)
        {
            Debug.Log($"Action {this.GetType()} terminated by {source.sourceName}");
            state = ActionState.Finished;
            OnTerminate(model, owner, source); 
        }
        
        public virtual void OnTerminate(GameModel model, Unit owner, IActionSource source)
        {
            
        }
    }

    public enum ActionState
    {
        NotStarted,
        Processing,
        Finished
    }
}