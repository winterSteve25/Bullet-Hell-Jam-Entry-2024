using System;
using UnityEngine;

namespace Utils
{
    public class HitPoints : MonoBehaviour
    {
        [SerializeField] private int maxHp;
        [SerializeField] private int hp;
        [SerializeField] private float invincibleTime;
        public bool invincible;

        private int _shield;
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        public event Action OnDeath;
        public event Action OnHealed;
        public event Action OnDamaged;

        public int Hp
        {
            get => hp;
            set
            {
                int newHP = value;

                if (newHP < hp && _shield > 0)
                {
                    _shield -= hp - newHP;

                    if (_shield < 0)
                    {
                        newHP = hp + _shield;
                        _shield = 0;
                    }
                }

                int prevHp = hp;
                hp = Mathf.Min(newHP, maxHp);

                if (hp <= 0)
                {
                    hp = 0;
                    OnDeath?.Invoke();
                }
                else if (prevHp > hp)
                {
                    OnDamaged?.Invoke();
                }
                else if (prevHp < hp)
                {
                    OnHealed?.Invoke();
                }
            }
        }

        public int Shield
        {
            set => _shield = value;
        }

        private float _elapsedTime;

        private void Awake()
        {
            hp = maxHp;
            _collider = TryGetComponent(out Collider2D col) ? col : null;
            _rigidbody = TryGetComponent(out Rigidbody2D rigidbody2d) ? rigidbody2d : null;
        }

        private void Update()
        {
            if (!invincible)
            {
                return;
            }

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > invincibleTime)
            {
                invincible = false;
                if (_collider is null) return;
                _collider.excludeLayers = 0;
                _rigidbody.excludeLayers = 0;
            }
        }

        public void SetInvincible()
        {
            invincible = true;
            _elapsedTime = 0;

            if (_collider is not null)
            {
                _collider.excludeLayers = LayerMask.NameToLayer("Enemies");
                _rigidbody.excludeLayers = LayerMask.NameToLayer("Enemies");
            }
        }
    }
}
