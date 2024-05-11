using Projectiles;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        private void Update()
        {
            if (GameInput.LeftClickButtonDown())
            {
                ProjectileManager.Instance.Spawn(transform.position, new Vector2(0.1f, 0), 200);
            }
        }
    }
}