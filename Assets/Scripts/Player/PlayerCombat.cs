using Effects;
using Projectiles;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        
        private void Update()
        {
            if (!GameInput.LeftClickButton()) return;
            
            Vector2 position = transform.position;
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - position;
            
            ProjectileManager.Instance.Spawn(
                position,
                Positioner.Zero(),
                Positioner.Directional(dir),
                Effect.Fire, 
                10,
                GraceIgnoreMode.Player,
                amount: 1, 
                speed: 15
            );
        }
    }
}