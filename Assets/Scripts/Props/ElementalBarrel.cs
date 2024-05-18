using Effects;
using UnityEngine;

namespace Props
{
    public class ElementalBarrel : EffectObject
    {
        [SerializeField] private float radius;

        protected override void EffectsChanged(EffectObject obj, Effect effectAdded, float amount)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemies", "Player"));

            foreach (var col in cols)
            {
                if (col.TryGetComponent(out EffectObject effectObject))
                {

                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
