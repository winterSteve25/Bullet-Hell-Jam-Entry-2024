using System;
using UnityEngine;
using Utils;

namespace Effects.Status
{
    public class DOT : StatusEffect
    {
        private static Sprite _texture;

        protected override float TickSpeed => 2f;

        private Action<float> _progress;
        private Action _delete;

        private void Start()
        {
            _texture ??= Resources.Load<Sprite>("Sprites/Effect/DOT");
            (_progress, _delete) = StatusEffectsUI.Add(_texture, Color.white);
        }

        protected override bool ShouldEnd(EffectObject effectObject)
        {
            return effectObject.GetAmount(Effect.Fire) > 0.2f && effectObject.GetAmount(Effect.Plant) > 0.2f;
        }

        protected override void Act(EffectObject effectObject)
        {
            effectObject.RemoveEffect(Effect.Fire, 0.2f);
            effectObject.RemoveEffect(Effect.Plant, 0.2f);
            effectObject.Hp--;
            _progress(1 - Mathf.Min(effectObject.GetAmount(Effect.Fire), effectObject.GetAmount(Effect.Plant)) / 0.2f);
        }

        protected override void CleanUp(EffectObject effectObject)
        {
            _delete();
        }
    }
}
