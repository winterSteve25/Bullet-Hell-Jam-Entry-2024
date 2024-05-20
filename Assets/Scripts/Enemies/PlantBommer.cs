using System.Collections;
using Effects;
using Player;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class PlantBommer : Enemy
    {
        [SerializeField] private float rangeSqr;
        [SerializeField] private float startExplosion;
        [SerializeField] private float timeToExplode;
        [SerializeField] private float explosionRadius;
        [SerializeField] private int explosionDamage;

        private bool _isExploding;
        private HitPoints _player;

        private void Update()
        {
            var transform1 = transform;
            Vector3 rotation = transform1.eulerAngles;
            rotation.z += 1;
            transform1.eulerAngles = rotation;

            Vector2 playerPos = PlayerMovement.PlayerPos;
            Vector2 currPos = transform1.position;

            if (_isExploding)
            {
                return;
            }

            if (HasLineOfSight(playerPos, currPos, rangeSqr))
            {
                Vector2 dir = playerPos - currPos;
                if (dir.sqrMagnitude < startExplosion)
                {
                    Debug.Log("Lmao");
                    StartCoroutine(Boom());
                    return;
                }
                Move(dir, false);
            }
            else
            {
                Vector2 dir = playerPos - currPos;
                dir = GoLeftOrRight(dir, rangeSqr);
                Move(dir, false);
            }
        }

        private IEnumerator Boom()
        {
            _isExploding = true;
            _normalizeVec = Vector2.zero;
            yield return new WaitForSeconds(timeToExplode);
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (var col in cols)
            {
                if (!col.TryGetComponent(out EffectObject effectObject))
                {
                    continue;
                }

                effectObject.Hp -= explosionDamage;
                effectObject.Apply(Effect.Plant, 10, true);
            }

            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
