using Player;
using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class Hound : Enemy
    {
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float rangeSqr;
        [SerializeField] private float meleeRangeThreshold;
        [SerializeField] private float meleeRange;

        private float _elapsedTime;

        private void Update()
        {
            Vector2 playerPos = PlayerMovement.PlayerPos;
            Vector2 currPos = transform.position;
            _elapsedTime += Time.deltaTime;

            if (HasLineOfSight(playerPos, currPos, rangeSqr))
            {
                if (_elapsedTime < attackCooldown)
                {
                    return;
                }

                Vector2 dir = playerPos - currPos;
                if (dir.sqrMagnitude < meleeRangeThreshold)
                {
                    if (dir.sqrMagnitude < meleeRange)
                    {
                        _elapsedTime = 0;
                        Melee();
                        return;
                    }

                    Move(dir.normalized);
                    return;
                }

                _elapsedTime = 0;
                Shoot(playerPos, currPos);
                Move(Vector2.zero);
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
            Vector2 dir = target - currPos;
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

        private void Melee()
        {
        }
    }
}
