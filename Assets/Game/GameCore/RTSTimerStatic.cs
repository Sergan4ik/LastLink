using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

namespace Game.GameCore
{
    public partial class RTSStopWatch : RTSRuntimeData
    {
        public float elapsedTime = 0;
        public List<float> circles = new List<float>();
        public bool isPaused = false;
        public float totalElapsedTime => circles.Sum() + elapsedTime;

        public void MarkCircle()
        {
            circles.Add(elapsedTime);
            elapsedTime = 0;
        }

        public void Pause() => isPaused = true;
        public void Resume() => isPaused = false;

        public void Tick(float dt)
        {
            if (isPaused == false)
                elapsedTime += dt;
        }
    }
    public abstract partial class RTSTimer : RTSRuntimeData
    {
        public float elapsedTime = 0;
        public TimerState state = TimerState.NotStarted;
        public abstract float cycleTime { get; }
        public virtual bool Tick(float dt)
        {
            state = TimerState.Processing;
            
            elapsedTime += dt;
            if (elapsedTime >= cycleTime)
            {
                elapsedTime -= cycleTime;
                return true;
            }
            
            return false;
        }

        public abstract void Reset();
    }
    public enum TimerState
    {
        NotStarted,
        Processing,
        Reset
    }
    
    public partial class RTSTimerStatic : RTSTimer
    {
        public float staticCycleTime = 0;
        public override float cycleTime => staticCycleTime;
        public override void Reset()
        {
            elapsedTime = 0;
            state = TimerState.Reset;
        }
    }
    
    public partial class RTSTimerCountDown : RTSTimerStatic
    {
        public bool isEnded => elapsedTime >= staticCycleTime;
        public override bool Tick(float dt)
        {
            state = TimerState.Processing;
            elapsedTime += dt;
            if (isEnded)
                return true;
            return false;
        }
    }

    public partial class RTSTimerIntervals : RTSTimer
    {
        // [1, 3, 2] mean 1s 3s 2s intervals, -1 means infinity
        public List<float> intervals = new List<float>();
        public int currentInterval = 0;
        public bool loop = false;
        public float totalTimeElapsed;
        public override float cycleTime => intervals[currentInterval];
        public int PassedIntervals => currentInterval;
        public bool IsTimerFinished => currentInterval >= intervals.Count && loop == false;
        public float cycleTimeElapsed => intervals.Take(currentInterval).Sum() + elapsedTime;

        public override bool Tick(float dt)
        {
            state = TimerState.Processing;
            totalTimeElapsed += dt;
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

        public override void Reset()
        {
            elapsedTime = 0;
            currentInterval = 0;
            state = TimerState.Reset;
            totalTimeElapsed = 0;
        }
    }
}