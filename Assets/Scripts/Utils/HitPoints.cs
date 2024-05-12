using System;
using UnityEngine;

namespace Utils
{
    public class HitPoints : MonoBehaviour
    {
        [SerializeField] private int hp;
        
        public event Action OnDeath;
        public event Action OnHealed;
        public event Action OnDamaged;
        
        public int Hp
        {
            get => hp;
            set
            {
                int prevHp = hp;
                hp = value;

                if (hp < 0)
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
    }
}