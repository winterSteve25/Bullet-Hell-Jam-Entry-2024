using System;
using UnityEngine;

namespace Effects.Status
{
    public abstract class StatusEffect : MonoBehaviour
    {
        protected EffectObject EffectObject;
        private float _elapsedTime;

        protected abstract float TickSpeed { get; }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime <= TickSpeed)
            {
                return;
            }

            _elapsedTime = 0;

            if (ShouldEnd(EffectObject))
            {
                CleanUp(EffectObject);
                EffectObject.statusEffects.Remove(this);
                Destroy(this);
            }
            else
            {
                Act(EffectObject);
            }
        }

        protected abstract bool ShouldEnd(EffectObject effectObject);

        protected abstract void Act(EffectObject effectObject);

        protected abstract void CleanUp(EffectObject effectObject);

        public static void Add<T>(EffectObject go, Action<T> init = null) where T : StatusEffect
        {
            T s = go.gameObject.AddComponent<T>();
            s.EffectObject = go;
            go.statusEffects.Add(s);
            init?.Invoke(s);
        }
    }
}
