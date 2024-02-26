using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

namespace Game.GameCore
{
    public abstract partial class RTSTimer : RTSRuntimeData
    {
        public float elapsedTime = 0;
        public abstract float cycleTime { get; }
        public virtual bool Tick(float dt)
        {
            elapsedTime += dt;
            if (elapsedTime >= cycleTime)
            {
                elapsedTime -= cycleTime;
                return true;
            }
            
            return false;
        }
    }
    
    public partial class RTSTimerStatic : RTSTimer
    {
        public float staticCycleTime = 0;
        public override float cycleTime => staticCycleTime;
    }

    public partial class RTSTimerIntervals : RTSTimer
    {
        // [1, 3, 2] mean 1s 3s 2s intervals, -1 means infinity
        public List<float> intervals = new List<float>();
        public int currentInterval = 0;
        public bool loop = false;
        public override float cycleTime => intervals[currentInterval];
        
        public int PassedIntervals => currentInterval;

        public override bool Tick(float dt)
        {
            if (cycleTime == -1)
            {
                elapsedTime += dt;
                return false;
            }
            
            bool passedInterval = base.Tick(dt);
            if (passedInterval)
                currentInterval++;

            if (loop && currentInterval >= intervals.Count)
                currentInterval = 0;
                
            return passedInterval;
        }
    }
}