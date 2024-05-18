using System;
using Effects;
using Player;
using UnityEngine;
using Utils;

namespace Projectiles
{
    public class AbsorptionBullet : MonoBehaviour
    {
        private Action<EffectObject> _onHit;
        private Vector2 _direction;
        private float _elapsedTime;
        private Positioner.Position _position;
        private float _speed;

        public void Init(
            float speed,
            Action<EffectObject> onHit,
            Vector2 direction,
            Positioner.Position position
        )
        {
            _speed = speed;
            _position = position;
            _onHit = onHit;
            _direction = direction;
            _elapsedTime = 0;
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _elapsedTime += dt;
            Vector2 translation = _position(transform.position, _elapsedTime) * (dt * _speed);
            transform.Translate(translation);

            if (((Vector2)transform.position - PlayerMovement.PlayerPos).sqrMagnitude > 2000)
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

            if (other.TryGetComponent(out EffectObject effectObject))
            {
                _onHit(effectObject);
                Destroy(gameObject);
                return;
            }

            if (other.TryGetComponent(out ChildEffectObject child))
            {
                _onHit(child.Parent);
                Destroy(gameObject);
                return;
            }

            _onHit(null);
            Destroy(gameObject);
        }
    }
}
