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
        private float accumulatedTime => intervals.Take(currentInterval + 1).Sum();
        
        public int PassedIntervals => currentInterval;

        public override bool Tick(float dt)
        {
            while (intervals[currentInterval] != -1f && accumulatedTime < elapsedTime)
                currentInterval++;
            
            if (currentInterval >= intervals.Count)
            {
                if (loop)
                {
                    currentInterval = 0;
                }
                else
                {
                    return false;
                }
            }
            
            return base.Tick(dt);
        }
    }
}