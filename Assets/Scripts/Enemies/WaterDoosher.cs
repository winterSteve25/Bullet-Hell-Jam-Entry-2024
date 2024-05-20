using DG.Tweening;
using Effects;
using Player;
using Procedural;
using Projectiles;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class WaterDoosher : Enemy
    {
        [SerializeField] private float dashCooldown;
        [SerializeField] private float dashTime;
        [SerializeField] private float maxDashLength;
        [SerializeField] private float chargingMultiplier;
        [SerializeField] private float chargeTime;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private Transform sprite;

        private float _lastDash;
        private float _lastChangeDir;
        private float _walkDuration;
        private bool _charging;
        private Vector2 _dir;

        protected override void Start()
        {
            base.Start();
            _walkDuration = Random.Range(1, 1.5f);
            _dir = DirectionExt.Random(null).ToVectorOffset().Rotate(Random.Range(0, 60));
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            if (!_charging) _lastDash += dt;
            _lastChangeDir += dt;

            if (_lastChangeDir > _walkDuration)
            {
                _lastChangeDir = 0;
                _dir = (PlayerMovement.PlayerPos - (Vector2)transform.position).normalized.Rotate(Random.Range(30, 60));
                _walkDuration = Random.Range(1, 1.5f);
                transform.DORotate(new Vector3(0, 0, Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg), 0.2f)
                    .SetEase(Ease.OutCubic);
            }

            if (!_charging && _lastDash > dashCooldown)
            {
                Dash(_dir);
            }

            if (_charging)
            {
                Move(_dir, multiplier: chargingMultiplier);
            }
            else
            {
                Move(_dir);
            }
        }

        private void Dash(Vector2 dir)
        {
            _lastDash = 0;
            _charging = true;
            sprite.DOScaleY(0.8f, chargeTime)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    Rigidbody.DOMove(Rigidbody.position + dir.normalized * maxDashLength, dashTime)
                        .SetEase(Ease.OutCubic)
                        .OnComplete(() => _charging = false);

                    sprite.DOScaleY(1, dashTime * 0.5f)
                        .SetEase(Ease.OutCubic)
                        .SetDelay(dashTime * 0.3f)
                        .OnComplete(Shoot);
                });
        }

        private void Shoot()
        {
            var transform1 = transform;
            Vector2 currPos = transform1.position;
            ProjectileManager.Spawn(
                currPos,
                Positioner.Circle(4, 1, transform1.eulerAngles.z % 360),
                Positioner.Outward(currPos),
                Effect.Water,
                10,
                IgnoreMode.Enemies,
                amount: 4,
                speed: projectileSpeed
            );
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider is null) return;
            if (!other.collider.CompareTag("Player")) return;
            other.collider.GetComponent<HitPoints>().Hp--;
        }
    }
}
