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

        public List<StatusEffect> statusEffects;

        protected HitPoints HitPoints;
        public int Hp
        {
            get => HitPoints.Hp;
            set => HitPoints.Hp = value;
        }
        public int Shield
        {
            set => HitPoints.Shield = value;
        }

        public bool Invincible
        {
            get => HitPoints.invincible;
            set => HitPoints.invincible = value;
        }

        public Effect InheritElement => inheritElement;

        protected virtual void Start()
        {
            HitPoints = GetComponent<HitPoints>();
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

            float half = amount * 0.5f;

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

        protected virtual void EffectsChanged(EffectObject obj, Effect effectAdded, float amount)
        {
            if (BothEffectsPresent(Effect.Water, Effect.Fire, effectAdded))
            {
                StatusEffect.Add<Blinded>(obj, b => b.Init(amount * 0.25f));
                RemoveEffect(Effect.Water, 10);
                RemoveEffect(Effect.Fire, 10);
            }

            if (BothEffectsPresent(Effect.Water, Effect.Plant, effectAdded))
            {
                StatusEffect.Add<Bounded>(obj, b => b.Init(amount * 0.5f));
                RemoveEffect(Effect.Water, 10);
                RemoveEffect(Effect.Plant, 10);
            }

            if (BothEffectsPresent(Effect.Water, Effect.Wind, effectAdded))
            {
                // todo ice
            }

            if (BothEffectsPresent(Effect.Water, Effect.Electricity, effectAdded))
            {
                // todo chain
            }

            if (BothEffectsPresent(Effect.Fire, Effect.Plant, effectAdded))
            {
                StatusEffect.Add<DOT>(obj);
            }

            if (BothEffectsPresent(Effect.Fire, Effect.Wind, effectAdded))
            {
                CombatUtils.Explode(transform.position, 6, 60, 2, Effect.Fire, 10, 0);
            }

            if (BothEffectsPresent(Effect.Fire, Effect.Electricity, effectAdded))
            {
                // todo chain
            }

            if (BothEffectsPresent(Effect.Plant, Effect.Wind, effectAdded))
            {
                // todo aoe heal?
            }

            if (BothEffectsPresent(Effect.Plant, Effect.Earth, effectAdded))
            {
                // todo trap?
            }

            if (BothEffectsPresent(Effect.Plant, Effect.Electricity, effectAdded))
            {
                // todo chain
            }

            if (BothEffectsPresent(Effect.Earth, Effect.Electricity, effectAdded))
            {
                // todo nullify
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
