using System;
using UnityEngine;

namespace Utils
{
    public class HitPoints : MonoBehaviour
    {
        [SerializeField] private int hp;
        [SerializeField] private bool invincible;
        [SerializeField] private float invincibleTime;

        public event Action OnDeath;
        public event Action OnHealed;
        public event Action OnDamaged;

        public bool Invincible => invincible;
        public int Hp
        {
            get => hp;
            set
            {
                int prevHp = hp;
                hp = value;

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

        private float _elapsedTime;

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
