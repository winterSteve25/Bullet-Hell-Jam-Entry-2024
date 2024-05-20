namespace Effects.Status
{
    public class Steamed : StatusEffect
    {
        protected override float TickSpeed => 1;

        protected override bool ShouldEnd(EffectObject effectObject)
        {
            return false;
        }

        protected override void Act(EffectObject effectObject)
        {
        }

        protected override void CleanUp(EffectObject effectObject)
        {

        }
    }
}
