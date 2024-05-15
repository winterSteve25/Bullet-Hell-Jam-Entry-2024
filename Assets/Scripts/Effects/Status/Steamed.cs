namespace Effects.Status
{
    public class Steamed : StatusEffect
    {
        protected override float TickSpeed => 1;

        protected override bool ShouldEnd(EffectObject effectObject)
        {
            return true;
        }

        protected override void Act(EffectObject effectObject)
        {
        }
    }
}