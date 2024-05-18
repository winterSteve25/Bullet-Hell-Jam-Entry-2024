using Effects;
using UnityEngine;
using Utils;

namespace Props
{
    public class ElementalBarrel : EffectObject
    {
        [SerializeField] private float radius;
        [SerializeField] private float amount;

        protected override void EffectsChanged(EffectObject obj, Effect effectAdded, float amount)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemies", "Player", "Environment"));

            foreach (var col in cols)
            {
                if (col.CompareTag("Walls")) continue;
                if (!col.TryGetComponent(out EffectObject effectObject)) continue;

                if (col.CompareTag("Barrel"))
                {
                    effectObject.Apply(InheritElement, amount, true);
                    col.GetComponent<HitPoints>().Hp--;
                }

                base.EffectsChanged(effectObject, effectAdded, amount);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
