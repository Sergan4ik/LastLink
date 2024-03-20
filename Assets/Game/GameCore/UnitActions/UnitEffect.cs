namespace Game.GameCore
{
    public partial class UnitEffect : UnitAction
    {
        public override bool canExistParallel => true;
        public override ActionStackingPolicy stackingPolicy => ActionStackingPolicy.Parallel;
    }
}