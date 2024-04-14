using UnityEngine;
using UnityEngine.AI;

namespace Game.GameCore
{
    public partial class UnitMove : UnitAction
    {
        public float moveSpeed;
        public Vector3 globalDestination;
        public Vector3 localDestination => cachedWaypoints[currentWaypoint];

        // public NavMeshPath path;
        public int currentWaypoint;

        public float maxDistanceToTarget;
        
        public Vector3[] cachedWaypoints;
        
        const float DISTANCE_THRESHOLD = 0.1f;

        protected override void OnActivation(GameModel gameModel, Unit owner, RTSInput input)
        {
            var nearestPossiblePoint = ((owner.transform.position - input.targetData.worldPosition) * maxDistanceToTarget) + input.targetData.worldPosition;
            var canGoOnMaxDist = NavMesh.SamplePosition(nearestPossiblePoint, out var hit, 2f, NavMesh.AllAreas);

            globalDestination = canGoOnMaxDist ? hit.position : input.targetData.worldPosition;
            var path = new NavMeshPath();
            bool calculatePath = NavMesh.CalculatePath(owner.transform.position, globalDestination, NavMesh.AllAreas, path);
            currentWaypoint = 0;
            cachedWaypoints = path.corners;
            
            if (calculatePath == false)
                Terminate(gameModel, owner,this);
            
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

        public override void OnTerminate(GameModel model, Unit owner, IActionSource source)
        {
            owner.PlayIdle();
        }
    }
}