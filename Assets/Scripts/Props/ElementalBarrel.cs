using Effects;
using UnityEngine;
using Utils;

namespace Props
{
    public class ElementalBarrel : EffectObject
    {
        [SerializeField] private ParticleSystem explosionParticle;
        [SerializeField] private float radius;
        [SerializeField] private float amount;

        protected override void Start()
        {
            base.Start();
            HitPoints.OnDeath += OnDeath;
        }

        protected override void EffectsChanged(EffectObject obj, Effect effectAdded, float amount)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemies", "Player", "Environment"));

            foreach (var col in cols)
            {
                if (col.transform == transform) continue;
                if (col.CompareTag("Walls")) continue;
                if (!col.TryGetComponent(out EffectObject effectObject)) continue;
                effectObject.Apply(effectAdded, amount, true);

                if (col.CompareTag("Barrel"))
                {
                    col.GetComponent<HitPoints>().Hp--;
                }
            }
        }

        private void OnDeath()
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemies", "Player", "Environment"));

            foreach (var col in cols)
            {
                if (col.transform == transform) continue;
                if (col.CompareTag("Walls")) continue;
                if (!col.TryGetComponent(out EffectObject effectObject)) continue;
                effectObject.Apply(InheritElement, amount, true);
            }

            // ReSharper disable once Unity.PreferNonAllocApi
            ParticleSystem p = Instantiate(explosionParticle, transform.position, Quaternion.identity);
 #pragma warning disable CS0618 // Type or member is obsolete
            p.startColor = InheritElement.Color;
 #pragma warning restore CS0618 // Type or member is obsolete
            Destroy(p.gameObject, 2f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
