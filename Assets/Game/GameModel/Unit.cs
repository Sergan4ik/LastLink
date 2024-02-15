using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using ZergRush.ReactiveCore;

namespace Game.GameModel
{
    public partial class RTSTransform : RTSRuntimeData
    {
        public Cell<Vector3> position;
        public Cell<Quaternion> rotation;

        public Vector3 Position => position.value;
        public Quaternion Rotation => rotation.value;
    }
    public partial class Unit : RTSContextNode
    {
        public CoroutineEngine engine;
        public RTSTransform transform;

        public float moveSpeed = 5;
        public void Tick(float dt)
        {
            engine.Update(dt);
        }
        
        public IEnumerator MoveTo(Vector3 destination, bool isDirectionalMove)
        {
            const float DISTANCE_THRESHOLD = 0.1f;
            if (isDirectionalMove)
            {
                Vector3 direction = (destination - transform.position.value).normalized;
                while ((transform.position.value - destination).sqrMagnitude > DISTANCE_THRESHOLD * DISTANCE_THRESHOLD)
                {
                    float dist = moveSpeed * GameModel.FrameTime;
                    transform.position.value += direction * dist;
                    logger.Log($"Pos {transform.position.value} , Dest {destination}, moved dist {dist}");
                    yield return CoroutineEngine.SkipFrame;
                }
                
                yield break;
            }
            
            var path = new NavMeshPath();
            var calculatePath = NavMesh.CalculatePath(transform.position.value, destination, NavMesh.AllAreas, path);

            if (!calculatePath) yield break;
            
            foreach (var newDestination in path.corners)
            {
                logger.Log($"New destination {newDestination}");
                yield return MoveTo(newDestination, true);
            }
        }
    }
}