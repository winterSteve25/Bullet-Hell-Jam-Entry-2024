namespace Effects.Status
{
    public class Shielded : StatusEffect
    {
        protected override float TickSpeed => 0.1f;

        private float _duration;
        private float _elapsed;

        public void Init(float duration, int shieldStrength)
        {
            _duration = duration;
            _elapsed = 0;
            EffectObject.Shield = shieldStrength;
        }

        public void RefreshDuration()
        {
            _elapsed = 0;
        }

        protected override bool ShouldEnd(EffectObject effectObject)
        {
            _elapsed += TickSpeed;
            return _elapsed > _duration;
        }

        protected override void Act(EffectObject effectObject)
        {
        }

        protected override void CleanUp(EffectObject effectObject)
        {
            effectObject.Shield = 0;
        }
    }
}
