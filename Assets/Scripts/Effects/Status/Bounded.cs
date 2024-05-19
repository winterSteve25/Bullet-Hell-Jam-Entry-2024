﻿using Enemies;
using Utils;

namespace Effects.Status
{
    public class Bounded : StatusEffect
    {
        protected override float TickSpeed => 0.1f;

        private float _duration;
        private float _elapsed;
        private MovementBase _mover;
        private float _prevSpeed;

        public void Init(float duration)
        {
            _duration = duration;
            _elapsed = 0;
            _mover = EffectObject.GetComponent<MovementBase>();
            _prevSpeed = _mover.speed;
            _mover.speed = 0;
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
            _mover.speed = _prevSpeed;
        }
    }
}
