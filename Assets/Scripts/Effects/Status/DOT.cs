using System;
using UnityEngine;
using Utils;

namespace Effects.Status
{
    public class DOT : StatusEffect
    {
        protected override float TickSpeed => 2f;

        private Action<float> _progress;
        private Action _delete;

        private void Start()
        {
            StatusEffectsUI.Add(null, new Color());
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
        }

        protected override void CleanUp(EffectObject effectObject)
        {
        }
    }
}
