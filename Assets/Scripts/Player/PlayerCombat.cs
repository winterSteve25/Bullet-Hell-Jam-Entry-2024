using Effects;
using Projectiles;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        public Camera cam;
        [SerializeField] private float bulletSpeed;

        private void Update()
        {
            if (!GameInput.LeftClickButtonDown()) return;

            Vector2 position = transform.position;
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - position;

            ProjectileManager.Spawn(
                position,
                Positioner.Zero(),
                Positioner.Directional(dir),
                Effect.Fire,
                10,
                GraceIgnoreMode.Player,
                amount: 1,
                speed: bulletSpeed
            );
        }
    }
}
