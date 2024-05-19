using System.Collections;
using Effects;
using Player;
using Projectiles;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class FireShooter : Enemy
    {
        [SerializeField] private float runAwayRangeSqr;
        [SerializeField] private float rangeSqr;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float shootCooldown;
        [SerializeField] private float timeBetweenBullets;

        [SerializeField] private Front front;

        private float _elapsedTime;

        private void Update()
        {
            Vector2 playerPos = PlayerMovement.PlayerPos;
            Vector2 currPos = transform.position;
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime < shootCooldown)
            {
                return;
            }

            if (HasLineOfSight(playerPos, currPos, rangeSqr))
            {
                Vector2 dir = playerPos - currPos;

                if (dir.sqrMagnitude < runAwayRangeSqr)
                {
                    if (front.other is not null && front.other.CompareTag("Walls"))
                    {
                        Rigidbody.velocity = Vector2.zero;
                        StartCoroutine(Shoot(PredictPlayerPos(), currPos));
                        return;
                    }

                    Move(currPos - playerPos);
                    return;
                }

                if (dir.sqrMagnitude < rangeSqr)
                {
                    Rigidbody.velocity = Vector2.zero;
                    StartCoroutine(Shoot(PredictPlayerPos(), currPos));
                    return;
                }

                Move(dir);
            }
            else
            {
                Vector2 dir = playerPos - currPos;
                dir = GoLeftOrRight(dir, rangeSqr);
                Move(dir);
            }
        }

        private IEnumerator Shoot(Vector2 target, Vector2 currPos)
        {
            _elapsedTime = 0;
            Vector2 dir = target - currPos;
            LookAt(dir);

            for (int i = 0; i < 3; i++)
            {
                dir = dir.Rotate(Random.Range(-90 * inaccuracy, 90 * inaccuracy));
                ProjectileManager.Spawn(
                    currPos,
                    Positioner.Zero(),
                    Positioner.Directional(dir),
                    Effect.Fire,
                    10,
                    IgnoreMode.Enemies,
                    amount: 1,
                    speed: projectileSpeed
                );

                yield return new WaitForSeconds(timeBetweenBullets);
            }
        }

        private void OnDrawGizmos()
        {
            Vector2 currPos = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(currPos, PredictPlayerPos() - currPos);

            Gizmos.color = Color.green;
            float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
            Gizmos.DrawRay(currPos, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 10);
        }

        protected override void OnDeath()
        {
            Vector2 currPos = transform.position;
            ProjectileManager.Spawn(
                currPos,
                Positioner.Circle(8, 1),
                Positioner.Outward(currPos),
                Effect.Fire,
                10,
                IgnoreMode.Enemies,
                amount: 8,
                speed: projectileSpeed
            );

            base.OnDeath();
        }
    }
}
