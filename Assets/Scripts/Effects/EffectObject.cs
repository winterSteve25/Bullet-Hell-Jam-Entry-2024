using System.Collections.Generic;
using System.Linq;
using Effects.Status;
using UnityEngine;
using Utils;

namespace Effects
{
    [RequireComponent(typeof(HitPoints))]
    public class EffectObject : MonoBehaviour
    {
        [SerializeField] private EffectStackDic effects;
        [SerializeField] private Effect inheritElement;

        private HitPoints _hp;
        public int Hp
        {
            get => _hp.Hp;
            set => _hp.Hp = value;
        }
        public int Shield
        {
            set => _hp.Shield = value;
        }

        public bool Invincible
        {
            get => _hp.invincible;
            set => _hp.invincible = value;
        }

        public Effect InheritElement => inheritElement;

        private void Start()
        {
            _hp = GetComponent<HitPoints>();
            if (inheritElement != null) return;
            if (effects.Count <= 0) return;
            inheritElement = effects.First().Key;
        }

        public bool RemoveEffect(Effect effect, float amount)
        {
            if (!effects.ContainsKey(effect)) return false;
            effects[effect] -= amount;
            if (effects[effect] > 0) return true;
            effects.Remove(effect);
            return false;
        }

        public float GetAmount(Effect effect)
        {
            if (effects.TryGetValue(effect, out var amount))
            {
                return amount;
            }

            return 0;
        }

        public float Apply(Effect eff, float amount, bool forceApplyAll = false)
        {
            if (eff is null || amount == 0)
            {
                return 0;
            }

            var half = amount * 0.5f;

            if (effects.ContainsKey(eff))
            {
                if (forceApplyAll)
                {
                    effects[eff] += amount;
                    return 0;
                }

                effects[eff] += half;
                return half;
            }

            if (!forceApplyAll)
            {
                effects.Add(eff, half);
                EffectsChanged(this, eff, half);
                return half;
            }

            effects.Add(eff, amount);
            EffectsChanged(this, eff, amount);
            return 0;
        }

        private void Apply(EffectStackDic effectStack)
        {
            List<(Effect, float)> changes = new List<(Effect, float)>();

            foreach (var (eff, amount) in effectStack)
            {
                changes.Add((eff, Apply(eff, amount)));
            }

            foreach (var (eff, amount) in changes)
            {
                effectStack[eff] = amount;
            }
        }

        protected virtual void EffectsChanged(EffectObject obj, Effect effectAdded, float amount)
        {
            if (BothEffectsPresent(Effect.Fire, Effect.Water, effectAdded))
            {
                StatusEffect.Add<Steamed>(obj);
            }
        }

        private bool BothEffectsPresent(Effect a, Effect b, Effect x)
        {
            if (x == a && effects.ContainsKey(b))
            {
                return true;
            }

            if (x == b && effects.ContainsKey(a))
            {
                return true;
            }

            return false;
        }
    }
}
