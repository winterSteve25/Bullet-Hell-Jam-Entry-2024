namespace Effects.Status
{
    public class Burning : StatusEffect
    {
        protected override float TickSpeed => 0.5f;
        
        protected override bool ShouldEnd(EffectObject effectObject)
        {
            return effectObject.GetAmount(Effect.Fire) < 0.5f || effectObject.GetAmount(Effect.Oil) < 0.5f;
        }
        
        protected override void Act(EffectObject effectObject)
        {
            effectObject.RemoveEffect(Effect.Oil, 0.5f);
            effectObject.RemoveEffect(Effect.Fire, 0.5f);
            effectObject.Hp -= 1;
        }
    }
}