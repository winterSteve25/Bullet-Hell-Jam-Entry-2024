using Enemies;
using Utils;

namespace Effects.Status
{
    public class Bounded : StatusEffect
    {
        protected override float TickSpeed => 0.1f;

        private float _duration;
        private float _elapsed;
        private Enemy _enemy;
        private float _prevSpeed;

        public void Init(float duration)
        {
            _duration = duration;
            _elapsed = 0;
            _enemy = EffectObject.GetComponent<Enemy>();
            _prevSpeed = _enemy.Speed;
            _enemy.Speed = 0;
        }

        protected override bool ShouldEnd(EffectObject effectObject)
        {
            _elapsed += TickSpeed;
            return _elapsed >= _duration;
        }

        protected override void Act(EffectObject effectObject)
        {
        }

        protected override void CleanUp(EffectObject effectObject)
        {
            _enemy.Speed = _prevSpeed;
        }
    }
}
