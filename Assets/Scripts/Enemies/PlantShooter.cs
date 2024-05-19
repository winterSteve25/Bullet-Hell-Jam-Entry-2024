using System.Collections;
using Effects;
using Player;
using Projectiles;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class PlantShooter : Enemy
    {

        [SerializeField]
        private float projectileSpeed;

        [SerializeField]
        private float shootCooldown;

        [SerializeField]
        private float timeBetweenBullets;

        private float _elapsedTime;

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            Vector2 currPos = transform.position;

            if (_elapsedTime < shootCooldown)
            {
                return;
            }
            StartCoroutine(Shoot(currPos));
        }
        private IEnumerator Shoot(Vector2 currPos)
        {
            _elapsedTime = 0;
            var transform1 = transform;

            ProjectileManager.Spawn(
                currPos,
                Positioner.Circle(8, 1, transform1.eulerAngles.z % 720),
                Positioner.Outward(currPos),
                Effect.Plant,
                10,
                IgnoreMode.Enemies,
                amount: 8,
                speed: projectileSpeed
            );
            yield return new WaitForSeconds(timeBetweenBullets);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("Player"))
                return;
            other.collider.GetComponent<HitPoints>().Hp--;
        }
    }
}
