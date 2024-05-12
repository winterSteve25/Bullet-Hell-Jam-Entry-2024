using System;
using Elements;
using Player;
using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private float _elapsedTime;

        private ElementStack _element;
        private Positioner.Position _position;
        private Action<Projectile> _onDestroy;
        private float _speed;
        private Collider2D[] _colliders;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _colliders = new Collider2D[3];
        }

        public void Init(
            Positioner.Position position,
            float speed,
            ElementStack element,
            Action<Projectile> onDestroy,
            Sprite sprite = null
        )
        {
            if (sprite != null)
                _renderer.sprite = sprite;

            _position = position;
            _element = new ElementStack();
            _element.Set(element);
            _onDestroy = onDestroy;
            _speed = speed;
            _elapsedTime = 0;
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _elapsedTime += dt;
            Vector3 translation = _position(transform.position, _elapsedTime) * (dt * _speed);
            translation.z = 0;
            transform.Translate(translation);

            if (((Vector2) transform.position - PlayerMovement.PlayerPos).sqrMagnitude > 2000)
            {
                _onDestroy(this);
            }
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < _colliders.Length; i++)
            {
                _colliders[i] = null;
            }

            Physics2D.OverlapCircleNonAlloc(transform.position, 0.25f, _colliders);

            foreach (var col in _colliders)
            {
                if (col == null) continue;

                if (col.CompareTag("Walls"))
                {
                    _onDestroy(this);
                    return;
                }

                if (!col.gameObject.TryGetComponent(out ElementObject obj)) continue;
                
                _onDestroy(this);
                Reaction.React(_element, obj);
            }
        }
    }
}