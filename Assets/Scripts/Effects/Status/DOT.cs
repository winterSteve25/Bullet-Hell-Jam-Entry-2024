namespace Effects.Status
{
    public class DOT : StatusEffect
    {
        protected override float TickSpeed => 2f;

        protected override bool ShouldEnd(EffectObject effectObject)
        {
            return effectObject.GetAmount(Effect.Fire) > 0.2f && effectObject.GetAmount(Effect.Plant) > 0.2f;
        }

        protected override void Act(EffectObject effectObject)
        {
            effectObject.RemoveEffect(Effect.Fire, 0.2f);
            effectObject.RemoveEffect(Effect.Plant, 0.2f);
            effectObject.Hp--;
        }

        protected override void CleanUp(EffectObject effectObject)
        {
        }
    }
}
