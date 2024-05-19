using Player;
using UnityEngine;

namespace Effects
{
    public class Droplet : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        private Effect _effect;

        public void Init(Effect effect, Vector2 position)
        {
            _effect = effect;
            _sprite.color = _effect.Color;
            transform.position = position;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
            if (playerCombat.ElementSelected is not null) return;
            playerCombat.ElementSelected = _effect;
            playerCombat.ElementAmount = playerCombat.maxElementAmount;
            Destroy(gameObject);
        }
    }
}
