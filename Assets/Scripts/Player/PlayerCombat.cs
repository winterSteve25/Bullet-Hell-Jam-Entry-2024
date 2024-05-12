using Elements;
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
            if (!GameInput.LeftClickButtonDown()) return;
            
            Vector3 position = transform.position;
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = mousePos - position;
            dir.Normalize();
                
            ProjectileManager.Instance.Spawn(
                position + dir * 3,
                Positioner.Zero(),
                Positioner.Directional(dir),
                new ElementStack(Element.Fire, 10),
                amount: 1, 
                speed: 25
            );
        }

        private void OnDrawGizmos()
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            Vector3 dir = mousePos - position;
            dir.Normalize();
            Gizmos.DrawRay(position, dir);
        }
    }
}