using System;
using UnityEngine;

namespace Utils
{
    public class HitPoints : MonoBehaviour
    {
        [SerializeField] private int maxHp;
        [SerializeField] private int hp;
        [SerializeField] private bool invincible;
        [SerializeField] private float invincibleTime;

        private int _shield;

        public event Action OnDeath;
        public event Action OnHealed;
        public event Action OnDamaged;

        public bool Invincible => invincible;
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
                } else if (prevHp > hp)
                {
                    OnDamaged?.Invoke();
                } else if (prevHp < hp)
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
            }
        }

        public void SetInvincible()
        {
            invincible = true;
            _elapsedTime = 0;
        }
    }
}
