using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

    public partial class UnitMove : UnitAction
    {
        public float moveSpeed;
        public Vector3 globalDestination;
        
        public Vector3 localDestination => path.corners[currentWaypoint];

        public NavMeshPath path;
        public int currentWaypoint;
        
        const float DISTANCE_THRESHOLD = 0.1f;
        protected override void InitInternal()
        {
            path = new NavMeshPath();
            bool calculatePath = NavMesh.CalculatePath(owner.transform.position, globalDestination, NavMesh.AllAreas, path);
            currentWaypoint = 0;
            
            if (calculatePath == false)
                Terminate(this);
        }

        private Vector3[] cachedWaypoints;
        protected override void ProcessTick(float dt)
        {
            cachedWaypoints = path.corners;
            if (currentWaypoint >= cachedWaypoints.Length)
            {
                Terminate(this);
                return;
            }
            
            
            Vector3 direction = (cachedWaypoints[currentWaypoint] - owner.transform.position).normalized;
            while (currentWaypoint < cachedWaypoints.Length && direction.sqrMagnitude < 0.9f)
            {
                direction = (cachedWaypoints[++currentWaypoint] - owner.transform.position).normalized;
            }

            Quaternion lookRotation = UnityEngine.Quaternion.LookRotation(new Vector3(direction.x, 0 , direction.z));
            float dist = moveSpeed * dt;
            if ((localDestination - owner.transform.position).sqrMagnitude > dist * dist)
            {
                owner.transform.position += direction * dist;
                owner.transform.rotation = lookRotation;
            }
            else
            {
                owner.transform.position = localDestination;
            }
            
            if ((owner.transform.position - localDestination).sqrMagnitude < DISTANCE_THRESHOLD * DISTANCE_THRESHOLD)
            {
                currentWaypoint++;
            }
            
        }
    }
}