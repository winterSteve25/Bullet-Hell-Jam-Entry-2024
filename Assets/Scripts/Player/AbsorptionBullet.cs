using System;
using Effects;
using UnityEngine;
using Utils;

namespace Player
{
    public class AbsorptionBullet : MovementBase
    {
        private Action<EffectObject> _onHit;
        private Vector2 _direction;

        public void Init(Action<EffectObject> onHit, Vector2 direction)
        {
            _onHit = onHit;
            _direction = direction;
        }

        private void Update()
        {
            float hv = CalculateVelocity(Rigidbody.velocity.x, _direction.x);
            float vv = CalculateVelocity(Rigidbody.velocity.y, _direction.y);
            Rigidbody.velocity = new Vector2(hv * speed, vv * speed);
            Rigidbody.velocity.Normalize();

            if (((Vector2) transform.position - PlayerMovement.PlayerPos).sqrMagnitude > 2000)
            {
                _onHit(null);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null)
            {
                _onHit(null);
                Destroy(gameObject);
                return;
            }

            if (other.CompareTag("Player"))
            {
                return;
            }

            if (other.CompareTag("Walls"))
            {
                _onHit(null);
                Destroy(gameObject);
                return;
            }

            if (!other.TryGetComponent(out EffectObject effectObject))
            {
                _onHit(null);
                Destroy(gameObject);
                return;
            }

            _onHit(effectObject);
            Destroy(gameObject);
        }
    }
}
