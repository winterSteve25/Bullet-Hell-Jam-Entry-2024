using System.Collections.Generic;
using Effects.Status;
using UnityEngine;
using Utils;

namespace Effects
{
    [RequireComponent(typeof(HitPoints))]
    public class EffectObject : MonoBehaviour
    {
        [SerializeField] private EffectStackDic effects;

        private HitPoints _hp;
        public int Hp
        {
            get => _hp.Hp;
            set => _hp.Hp = value;
        }

        public bool Invincible => _hp.Invincible;

        private void Start()
        {
            _hp = GetComponent<HitPoints>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.TryGetComponent(out EffectObject container)) return;
            container.Apply(effects);
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
                EffectsChanged(eff, half);
                return half;
            }
            
            effects.Add(eff, amount);
            EffectsChanged(eff, amount);
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

        private void EffectsChanged(Effect effectAdded, float amount)
        {
            if (BothEffectsPresent(Effect.Oil, Effect.Fire, effectAdded))
            {
                StatusEffect.Add<Burning>(this);
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