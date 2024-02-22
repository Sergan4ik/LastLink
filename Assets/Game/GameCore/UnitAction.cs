namespace Game.GameCore
{
    public partial class UnitAction : RTSRuntimeData
    {
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

    public partial class UnitMove : UnitAction
    {
        protected override void ProcessTick(float dt)
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
                    // yield return CoroutineEngine.SkipFrame;
                }
                
                // yield break;
            }
            
            var path = new NavMeshPath();
            var calculatePath = NavMesh.CalculatePath(transform.position.value, destination, NavMesh.AllAreas, path);

            // if (!calculatePath) yield break;
            
            foreach (var newDestination in path.corners)
            {
                logger.Log($"New destination {newDestination}");
                // yield return MoveTo(newDestination, true);
            }
        }
    }
}