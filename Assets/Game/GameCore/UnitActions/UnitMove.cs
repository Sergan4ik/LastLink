using UnityEngine;
using UnityEngine.AI;

namespace Game.GameCore
{
    public partial class UnitMove : UnitAction
    {
        public float moveSpeed;
        public float rotationSpeed;
        public Vector3 globalDestination;
        public Vector3 localDestination => cachedWaypoints[currentWaypoint];

        // public NavMeshPath path;
        public int currentWaypoint;

        public float maxDistanceToTarget;
        
        public Vector3[] cachedWaypoints;
        
        const float DISTANCE_THRESHOLD = 0.1f;
        const float ROTATION_MOVEMENT_BLOCKER = 30f;

        private RTSTimerCountDown dirrectionalMoveDelay;

        protected override void OnActivation(GameModel gameModel, Unit owner, RTSInput input)
        {
            var nearestPossiblePoint = ((owner.transform.position - input.targetData.worldPosition) * maxDistanceToTarget) + input.targetData.worldPosition;
            var canGoOnMaxDist = NavMesh.SamplePosition(nearestPossiblePoint, out var hit, 2f, NavMesh.AllAreas);

            globalDestination = canGoOnMaxDist ? hit.position : input.targetData.worldPosition;
            var path = new NavMeshPath();
            bool calculatePath = NavMesh.CalculatePath(owner.transform.position, globalDestination, NavMesh.AllAreas, path);
            currentWaypoint = 0;
            cachedWaypoints = path.corners;
            
            dirrectionalMoveDelay = new RTSTimerCountDown()
            {
                staticCycleTime = 0.1f
            };
            
            if (calculatePath == false)
                Terminate(gameModel, owner,this);
            
            if (owner.currentAnimation.animationName != owner.cfg.walkAnimation.animationName)
                owner.PlayAnimation(owner.cfg.walkAnimation);
        }

        protected override void ProcessTick(GameModel model, float dt, Unit owner)
        {
            if (initialInput.targetData.worldPosition != globalDestination &&
                (owner.transform.position - globalDestination).sqrMagnitude < maxDistanceToTarget * maxDistanceToTarget)
            {
                Terminate(model, owner,this);
                return;
            }

            if (currentWaypoint >= cachedWaypoints.Length)
            {
                Terminate(model, owner,this);
                return;
            }


            Vector3 direction = (cachedWaypoints[currentWaypoint] - owner.transform.position).normalized;
            while (currentWaypoint < cachedWaypoints.Length - 1 && direction.sqrMagnitude < 0.9f)
            {
                direction = (cachedWaypoints[++currentWaypoint] - owner.transform.position).normalized;
            }

            
            Quaternion lookRotation = direction.WithY(0).sqrMagnitude > 0.01f ? Quaternion.LookRotation(new Vector3(direction.x, 0 , direction.z)) : owner.transform.rotation;

            if (owner.transform.rotation != lookRotation)
            {
                owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, lookRotation, rotationSpeed * dt);
            }
            
            float angle = Quaternion.Angle(owner.transform.rotation, lookRotation);
            if (angle <= ((initialInput.flags & RTSInputFlags.IsDirectionalModifier) != 0
                    ? ROTATION_MOVEMENT_BLOCKER
                    : 1e-5))
            {
                if (currentWaypoint == 1 && (initialInput.flags & RTSInputFlags.IsDirectionalModifier) != 0)
                {
                    dirrectionalMoveDelay.Tick(dt);
                    if (dirrectionalMoveDelay.isEnded)
                    {
                        MoveUnitToLocalDestination(dt, owner, direction);
                    }
                }
                else
                {
                    MoveUnitToLocalDestination(dt, owner, direction);
                }
            }

            if ((owner.transform.position - localDestination).sqrMagnitude < DISTANCE_THRESHOLD * DISTANCE_THRESHOLD)
            {
                currentWaypoint++;
            }
        }

        private void MoveUnitToLocalDestination(float dt, Unit owner, Vector3 direction)
        {
            float dist = moveSpeed * dt;
            if ((localDestination - owner.transform.position).sqrMagnitude > dist * dist)
            {
                owner.transform.position += direction * dist;
            }
            else
            {
                owner.transform.position = localDestination;
            }
        }

        public override void OnTerminate(GameModel model, Unit owner, IActionSource source)
        {
            if (source is not UnitMove move || move == this)
            {
                owner.PlayIdle();
            }
            else
            {
            }
        }
    }
}