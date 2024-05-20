using System;
using UnityEngine;
using Utils;

namespace Effects.Status
{
    public class Shielded : StatusEffect
    {
        private static Sprite _texture;
        protected override float TickSpeed => 0.1f;

        private float _duration;
        private float _elapsed;

        private Action<float> _progress;
        private Action _delete;

        public void Init(float duration, int shieldStrength)
        {
            _texture ??= Resources.Load<Sprite>("Sprites/Effect/Shielded");
            _duration = duration;
            _elapsed = 0;
            EffectObject.Shield = shieldStrength;
            (_progress, _delete) = StatusEffectsUI.Add(_texture, Color.white);
        }

        public void RefreshDuration()
        {
            _elapsed = 0;
        }

        protected override bool ShouldEnd(EffectObject effectObject)
        {
            _elapsed += TickSpeed;
            _progress(1 - _elapsed / _duration);
            return _elapsed > _duration;
        }

        protected override void Act(EffectObject effectObject)
        {
        }

        protected override void CleanUp(EffectObject effectObject)
        {
            _delete();
            effectObject.Shield = 0;
        }
    }
}
