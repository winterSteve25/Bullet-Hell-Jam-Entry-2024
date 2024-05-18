using Player;
using Projectiles;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class RangedRobot : Enemy
    {
        [SerializeField] private float rangeSqr;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float shootCooldown;

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
                if (dir.sqrMagnitude < rangeSqr)
                {
                    Shoot(PredictPlayerPos(), currPos);
                    Move(Vector2.zero);
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

        private void Shoot(Vector2 target, Vector2 currPos)
        {
            _elapsedTime = 0;
            Vector2 dir = target - currPos;
            LookAt(dir);
            dir = dir.Rotate(Random.Range(-90 * inaccuracy, 90 * inaccuracy));

            ProjectileManager.Spawn(
                currPos,
                Positioner.Zero(),
                Positioner.Directional(dir),
                null,
                0,
                GraceIgnoreMode.Enemies,
                amount: 1,
                speed: projectileSpeed
            );
        }

        private void OnDrawGizmos()
        {
            Vector2 currPos = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(currPos, PredictPlayerPos() - currPos);
        }
    }
}
