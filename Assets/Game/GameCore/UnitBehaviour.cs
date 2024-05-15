namespace Game.GameCore
{
    public partial class UnitBehaviour : RTSRuntimeData
    {
        public virtual void Init(GameModel model, Unit targetUnit) { }
        public virtual void Tick(GameModel model, Unit targetUnit, float dt) { }
        public virtual void ProcessInput(GameModel model, Unit targetUnit, RTSInput input) { }
    }
}