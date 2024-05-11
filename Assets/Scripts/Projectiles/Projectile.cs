using System;
using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private Rigidbody2D _rigidbody;
        private float _elapsedTime;
        
        private Func<float, Vector2, Vector2> _position;
        private Action<Projectile> _onDestroy;
        private float _speed;
        private Collider2D[] _colliders;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _colliders = new Collider2D[10];
        }

        public void Init(Vector2 position, Func<float, Vector2, Vector2> fn, float speed, Action<Projectile> onDestroy, Sprite sprite = null)
        {
            if (sprite != null) 
                _renderer.sprite = sprite;
            
            _position = fn;
            _onDestroy = onDestroy;
            _speed = speed;
            _elapsedTime = 0;
            transform.position = position;
        }
        
        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            _position(_elapsedTime, transform.position);
            Physics2D.OverlapBoxNonAlloc(transform.position, new Vector2(2, 2), 0, _colliders);
        }
    }
}