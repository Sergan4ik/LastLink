namespace Game.GameCore
{
    public partial class DOT : UnitEffect
    {
        public float dps = 10;

        protected override void ProcessTick(GameModel model, float dt, Unit owner)
        {
            owner.DealRawDamage(dps * dt);
        }
    }
}