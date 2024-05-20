using System;
using UnityEngine;
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

        private Action<float> _progress;
        private Action _delete;

        public void Init(float duration)
        {
            _duration = duration;
            _elapsed = 0;
            _mover = EffectObject.GetComponent<MovementBase>();
            _prevSpeed = _mover.speed;
            _mover.speed = 0;
            StatusEffectsUI.Add(null, new Color());
        }

        protected override bool ShouldEnd(EffectObject effectObject)
        {
            _elapsed += TickSpeed;
            _progress(_elapsed / _duration);
            return _elapsed >= _duration;
        }

        protected override void Act(EffectObject effectObject)
        {
        }

        protected override void CleanUp(EffectObject effectObject)
        {
            _mover.speed = _prevSpeed;
            _delete();
        }
    }
}
