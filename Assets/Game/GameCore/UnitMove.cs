using UnityEngine;
using UnityEngine.AI;

namespace Game.GameCore
{
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