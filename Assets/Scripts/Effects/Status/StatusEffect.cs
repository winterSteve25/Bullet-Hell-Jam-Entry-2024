using UnityEngine;

namespace Effects.Status
{
    public abstract class StatusEffect : MonoBehaviour
    {
        private EffectObject _effectObject;
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
            
            if (ShouldEnd(_effectObject))
            {
                Destroy(this);
            }
            else
            {
                Act(_effectObject);
            }
        }

        protected abstract bool ShouldEnd(EffectObject effectObject);

        protected abstract void Act(EffectObject effectObject);
        
        public static void Add<T>(EffectObject go) where T : StatusEffect
        {
            T s = go.gameObject.AddComponent<T>();
            s._effectObject = go;
        }
    }
}