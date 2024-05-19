using System.Collections;
using DG.Tweening;
using Effects;
using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class FireWorm : Enemy
    {
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float shootCooldown;
        [SerializeField] private Transform tail;

        private float _elapsedTime;

        protected override void Start()
        {
            base.Start();
            _elapsedTime = shootCooldown;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime < shootCooldown)
            {
                return;
            }

            StartCoroutine(Shoot());
        }

        private IEnumerator Shoot()
        {
            _elapsedTime = 0;

            tail.DORotate(new Vector3(0, 0, -360), shootCooldown * 0.9f, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutQuint)
                .OnComplete(() => tail.eulerAngles = Vector3.zero);

            float interval = shootCooldown * 0.3f / 9;
            yield return new WaitForSeconds(shootCooldown * 0.3f);

            for (int i = 0; i < 9; i++)
            {
                Transform child = tail.GetChild(i % 3);
                Vector2 pos = child.position;

                ProjectileManager.Spawn(
                    pos,
                    (_, _) => child.up,
                    Positioner.Outward(pos),
                    Effect.Fire,
                    10,
                    IgnoreMode.Enemies,
                    amount: 1,
                    speed: projectileSpeed
                );

                yield return new WaitForSeconds(interval);
            }
        }
    }
}
