using Effects;
using UnityEngine;
using Utils;

namespace Props
{
    [RequireComponent(typeof(HitPoints))]
    public class OilBarrel : MonoBehaviour
    {
        [SerializeField] private float splashRadius = 1;
        [SerializeField] private float oilAmount = 10;
        
        private HitPoints _hp;

        private void OnEnable()
        {
            _hp = GetComponent<HitPoints>();
            _hp.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            _hp.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            // not called often should be ok
            foreach (var col in Physics2D.OverlapCircleAll(transform.position, splashRadius))
            {
                if (col.CompareTag("Walls")) continue;
                if (!col.TryGetComponent(out EffectObject effectObject)) continue;
                effectObject.Apply(Effect.Oil, oilAmount, true);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, splashRadius);
        }
    }
}