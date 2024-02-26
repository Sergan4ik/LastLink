namespace Game.GameCore
{
    public partial class DOT : UnitEffect
    {
        public float dps = 10;

        protected override void ProcessTick(float dt)
        {
            owner.DealRawDamage(dps * dt);
        }
    }
}