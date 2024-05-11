using Elements;
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
                ProjectileManager.Instance.Spawn(
                    transform.position,
                    Positioner.Polar(),
                    (currPos, _) => (currPos - transform.position).normalized,
                    new ElementStack(Element.Fire, 10),
                    amount: 3
                );
            }
        }
    }
}