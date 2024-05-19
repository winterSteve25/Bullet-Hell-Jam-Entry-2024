using DG.Tweening;
using Player;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class WaterDasher : Enemy
    {
        [SerializeField] private float dashCooldown;
        [SerializeField] private float dashTime;
        [SerializeField] private float maxDashLength;
        [SerializeField] private float range;
        [SerializeField] private float moveRange;
        [SerializeField] private float chargingMultiplier;
        [SerializeField] private float chargeTime;

        [SerializeField] private Transform sprite;

        private float _lastDash;
        private bool _charging;

        private void Update()
        {
            float dt = Time.deltaTime;
            if (!_charging) _lastDash += dt;

            Vector2 dir = PlayerMovement.PlayerPos - (Vector2)transform.position;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

            if (!_charging && _lastDash > dashCooldown && dir.sqrMagnitude < range)
            {
                Dash();
            }

            if (dir.sqrMagnitude < moveRange)
            {
                if (!_charging)
                {
                    Move(Vector2.zero);
                }
                return;
            }

            if (_charging)
            {
                Move(dir, multiplier: chargingMultiplier);
            }
            else
            {
                Move(dir);
            }
        }

        private void Dash()
        {
            _lastDash = 0;
            _charging = true;
            sprite.DOScaleY(0.15f, chargeTime)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    Rigidbody.DOMove(Rigidbody.position + (PlayerMovement.PlayerPos - (Vector2)transform.position).normalized * maxDashLength, dashTime)
                        .SetEase(Ease.OutCubic)
                        .OnComplete(() => _charging = false);

                    sprite.DOScaleY(1, dashTime * 0.2f)
                        .SetEase(Ease.OutCubic)
                        .SetDelay(dashTime * 0.5f);
                });
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider is null) return;
            if (!other.collider.CompareTag("Player")) return;
            other.collider.GetComponent<HitPoints>().Hp--;
        }
    }
}
