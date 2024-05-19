using System;
using DG.Tweening;
using Effects;
using Player;
using UnityEngine;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        private static int s_enemyMask;

        [SerializeField]
        private SpriteRenderer _renderer;
        private float _elapsedTime;

        private Effect _effect;
        private float _effAmount;
        private Positioner.Position _position;
        private Action<Projectile> _onDestroy;
        private float _speed;
        private int _graceLayerMask;
        private Collider2D[] _colliders;

        private void Start()
        {
            _colliders = new Collider2D[3];
        }

        public void Init(
            Positioner.Position position,
            float speed,
            Effect effect,
            float effAmount,
            Action<Projectile> onDestroy,
            int graceLayerMask,
            Sprite sprite = null
        )
        {
            s_enemyMask = IgnoreMode.Enemies.GetLayerMask();

            if (sprite is not null)
                _renderer.sprite = sprite;

            // ReSharper disable once Unity.NoNullPropagation
            _renderer.color = effect?.Color ?? Color.white;
            _position = position;
            _effect = effect;
            _effAmount = effAmount;
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
            transform.DORotate(new Vector3(0, 0, Mathf.Atan2(translation.y, translation.x)), 0.1f);

            if (((Vector2) transform.position - PlayerMovement.PlayerPos).sqrMagnitude > 2000)
            {
                _onDestroy(this);
            }
        }

        private void FixedUpdate()
        {
            if (!gameObject.activeSelf) return;

            for (var i = 0; i < _colliders.Length; i++)
            {
                _colliders[i] = null;
            }

            if (_graceLayerMask == s_enemyMask || _elapsedTime < 0.5)
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

                EffectObject effectObject;

                if (col.gameObject.TryGetComponent(out EffectObject obj))
                {
                    effectObject = obj;
                } else if (col.gameObject.TryGetComponent(out ChildEffectObject child))
                {
                    effectObject = child.Parent;
                }
                else
                {
                    continue;
                }

                if (effectObject.Invincible) continue;
                effectObject.Apply(_effect, _effAmount, true);
                effectObject.Hp -= 1;
                _onDestroy(this);
            }
        }
    }
}
