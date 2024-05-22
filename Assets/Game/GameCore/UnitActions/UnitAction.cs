using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameCore
{
    public partial class UnitAction : RTSRuntimeData, IActionSource
    {
        public string sourceName => $"Action {this.GetType()}";
        
        public float duration = -1;

        public ActionState state = ActionState.NotStarted;

        public RTSTimerIntervals stateTimer;
        public RTSInput initialInput;
        
        public virtual bool canExistParallel => false;
        public virtual ActionStackingPolicy stackingPolicy => ActionStackingPolicy.Interruptable;

        public void Init(GameModel model, Unit owner)
        {
            InitInternal(model, owner);
            stateTimer = new RTSTimerIntervals()
            {
                intervals = new List<float> { duration },
                loop = false
            };
        }

        protected virtual void InitInternal(GameModel model, Unit owner)
        {
            
        }

        public void Activate(GameModel model, Unit owner, RTSInput input)
        {
            initialInput = input;
            OnActivation(model, owner, input);
        }
        
        protected virtual void OnActivation(GameModel gameModel, Unit owner, RTSInput input)
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

        protected virtual void ProcessTick(GameModel model, float dt, Unit owner)
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
            if (ZeroLagSettings.messageLogs)
                Debug.Log($"Action {this.GetType()} terminated by {source.sourceName}");
            state = ActionState.Finished;
            OnTerminate(model, owner, source); 
        }

        public virtual void OnTerminate(GameModel model, Unit owner, IActionSource source)
        {
            
        }
    }

    public enum ActionStackingPolicy
    {
        Parallel,
        Interruptable,
        Stackable
    }

    public enum ActionState
    {
        NotStarted,
        Processing,
        Finished
    }
    
    public partial class TargetData : RTSRuntimeData
    {
        public List<int> sourceIds;
        public int targetId;
        public Vector3 worldPosition;
    }

    public partial class RTSInput : RTSRuntimeData
    {
        public RTSInputType inputType;
        public int inputTypeVariation;
        public TargetData targetData;
        public RTSInputFlags flags;
    }

    public enum RTSInputType
    {
        Move,
        AutoAttack,
        CastAbility
    }

    [Flags]
    public enum RTSInputFlags
    {
        ControlModifier = 1 << 1,
        IsDirectionalModifier = 1 << 2,
    } 
}