using Effects;
using UnityEngine;

namespace Utils
{
    public class CombatUtils
    {

        public static void AddForce(Vector2 dir, float multiplier, EffectObject obj)
        {
            if (!obj.TryGetComponent(out Rigidbody2D rb)) return;

            if (rb.velocity != Vector2.zero)
            {
                rb.AddForce(dir * multiplier, ForceMode2D.Impulse);
            }
            {
                rb.AddForce(dir * (multiplier * 0.1f), ForceMode2D.Impulse);
            }
        }

        public static void Explode(Vector2 origin, float radius, float force, int damage, Effect applied, float amountElementApplied, int layerMask)
        {
            ParticlesUtils.Explode(origin, radius);
            Collider2D[] targets = Physics2D.OverlapCircleAll(origin, radius, layerMask);
            foreach (var target in targets)
            {
                if (target is null) continue;
                EffectObject effectObject;

                if (target.TryGetComponent(out EffectObject obj))
                {
                    effectObject = obj;
                }
                else if (target.TryGetComponent(out ChildEffectObject childEffectObject))
                {
                    effectObject = childEffectObject.Parent;
                }
                else
                {
                    continue;
                }

                effectObject.Hp -= damage;
                effectObject.Apply(applied, amountElementApplied, true);

                AddForce((Vector2) effectObject.transform.position - origin, force, effectObject);
            }
        }
    }
}
