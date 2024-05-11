using System;
using UnityEngine;

namespace Elements
{
    public class ElementObject : MonoBehaviour
    {
        public event Action OnDeath;
        
        [SerializeField] private int reactionPriority;
        [SerializeField] private int hp;
        
        public ElementStack element;

        public int ReactionPriority => reactionPriority;
        public int Hp
        {
            get => hp;
            set
            {
                hp = value;
                if (hp > 0) return;
                hp = 0;
                OnDeath?.Invoke();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.TryGetComponent(out ElementObject container)) return;
            Reaction.React(this, container);
        }

        public override string ToString()
        {
            return $"Element: {element}\nHP: {hp}";
        }
    }
}