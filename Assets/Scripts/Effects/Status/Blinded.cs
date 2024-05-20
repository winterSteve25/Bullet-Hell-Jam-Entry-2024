using System;
using Enemies;
using UnityEngine;
using Utils;

namespace Effects.Status
{
    public class Blinded : StatusEffect
    {
        private static Sprite _texture;

        protected override float TickSpeed => 0.1f;

        private float _duration;
        private float _elapsed;
        private float _prevInac;
        private Enemy _enemy;

        private Action<float> _progress;
        private Action _delete;

        public void Init(float duration)
        {
            _texture ??= Resources.Load<Sprite>("Sprites/Effect/blinded");

            _duration = duration;
            _elapsed = 0;
            (_progress, _delete) = StatusEffectsUI.Add(_texture, Color.white);

            if (EffectObject.TryGetComponent(out Enemy enemy))
            {
                _prevInac = enemy.inaccuracy;
                enemy.inaccuracy *= 2;
                _enemy = enemy;
            }
            else
            {
                PostProcessingInstance.EnableVignette();
            }
        }

        protected override bool ShouldEnd(EffectObject effectObject)
        {
            _elapsed += Time.deltaTime;
            _progress(1 - _elapsed / _duration);
            return _elapsed >= _duration;
        }

        protected override void Act(EffectObject effectObject)
        {
        }

        protected override void CleanUp(EffectObject effectObject)
        {
            if (_enemy is not null)
            {
                _enemy.inaccuracy = _prevInac;
            }
            else
            {
                PostProcessingInstance.DisableVignette();
            }

            _delete();
        }
    }
}
