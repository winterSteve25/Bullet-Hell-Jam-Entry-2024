using Effects;
using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class PlantShooter : Enemy
    {
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float shootCooldown;

        private float _elapsedTime;

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            Vector2 currPos = transform.position;

            if (_elapsedTime < shootCooldown)
            {
                return;
            }

            Shoot(currPos);
        }
        private void Shoot(Vector2 currPos)
        {
            _elapsedTime = 0;
            var transform1 = transform;

            ProjectileManager.Spawn(
                currPos,
                Positioner.Circle(8, 1, transform1.eulerAngles.z % 360),
                Positioner.Outward(currPos),
                Effect.Plant,
                10,
                IgnoreMode.Enemies,
                amount: 8,
                speed: projectileSpeed
            );
        }
    }
}
