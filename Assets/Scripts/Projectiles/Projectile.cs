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
        private int _graceLayerMask;
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
            int graceLayerMask,
            Sprite sprite = null
        )
        {
            if (sprite is not null)
                _renderer.sprite = sprite;

            _position = position;
            _element = new ElementStack();
            _element.Set(element);
            _onDestroy = onDestroy;
            _graceLayerMask = graceLayerMask;
            _speed = speed;
            _elapsedTime = 0;
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _elapsedTime += dt;
            Vector2 translation = _position(transform.position, _elapsedTime) * (dt * _speed);
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

            if (_elapsedTime < 0.5)
            {
                Physics2D.OverlapCircleNonAlloc(transform.position, 0.25f, _colliders, _graceLayerMask);
            }
            else
            {
                Physics2D.OverlapCircleNonAlloc(transform.position, 0.25f, _colliders);
            }

            foreach (var col in _colliders)
            {
                if (col == null) continue;
                if (col.CompareTag("Walls"))
                {
                    _onDestroy(this);
                    return;
                }

                if (!col.gameObject.TryGetComponent(out ElementObject obj)) continue;
                
                Reaction.React(_element, obj);
                obj.Hp -= 1; 
                
                _onDestroy(this);
            }
        }
    }
}