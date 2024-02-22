using UnityEngine;
using UnityEngine.AI;

namespace Game.GameCore
{
    public partial class UnitAction : RTSContextNode
    {
        public Unit owner => (Unit)parent;
        
        public float elapsedTime = 0;
        public float preparingTime = 0;
        public float duration = -1;

        public ActionState state = ActionState.NotStarted;
        public void Tick(float dt)
        {
            elapsedTime += dt;
            if (elapsedTime < preparingTime)
            {
                state = ActionState.Preparing;
                PreparingTick(dt);
            }
            else if (elapsedTime < preparingTime + duration)
            {
                state = ActionState.Processing;
                ProcessTick(dt);
            }
            else
            {
                state = ActionState.Finished;
            }
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

        protected override void ProcessTick(float dt)
        {
        }
        public void MoveTo(Vector3 destination, bool isDirectionalMove)
        {
            const float DISTANCE_THRESHOLD = 0.1f;
            if (isDirectionalMove)
            {
                Vector3 direction = (destination - owner.transform.position).normalized;
                // if (direction.sqrMagnitude < 0.9f) yield break;
                
                Quaternion lookRotation = UnityEngine.Quaternion.LookRotation(new Vector3(direction.x, 0 , direction.z));
                while ((owner.transform.position - destination).sqrMagnitude > DISTANCE_THRESHOLD * DISTANCE_THRESHOLD)
                {
                    float dist = moveSpeed * GameCore.GameModel.FrameTime;
                    if ((destination - owner.transform.position).sqrMagnitude > dist * dist)
                    {
                        owner.transform.position += direction * dist;
                        owner.transform.rotation = lookRotation;
                    }
                    else
                    {
                        owner.transform.position = destination;
                    }
                    // yield return CoroutineEngine.SkipFrame;
                }
                
                // yield break;
            }
            
            var path = new NavMeshPath();
            var calculatePath = NavMesh.CalculatePath(owner.transform.position, destination, NavMesh.AllAreas, path);

            // if (!calculatePath) yield break;
            
            foreach (var newDestination in path.corners)
            {
                logger.Log($"New destination {newDestination}");
                // yield return MoveTo(newDestination, true);
            }
        }
    }
}