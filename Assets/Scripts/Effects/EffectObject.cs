using System;
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

        private void Start()
        {
            _hp = GetComponent<HitPoints>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.TryGetComponent(out EffectObject container)) return;
            container.Apply(effects);
        }

        public float Apply(Effect eff, float amount, bool forceApplyAll = false)
        {
            if (effects.ContainsKey(eff))
            {
                if (forceApplyAll)
                {
                    effects[eff] += amount;
                    return 0;
                }
                
                effects[eff] += amount * 0.5f;
                return amount * 0.5f;
            }

            if (!forceApplyAll)
            {
                effects.Add(eff, amount * 0.5f);
                return amount * 0.5f;
            }
            
            effects.Add(eff, amount);
            return 0;
        }

        private void Apply(EffectStackDic effectStack)
        {
            foreach (var (eff, amount) in effectStack)
            {
                effectStack[eff] = Apply(eff, amount);
            }
        }
    }
}