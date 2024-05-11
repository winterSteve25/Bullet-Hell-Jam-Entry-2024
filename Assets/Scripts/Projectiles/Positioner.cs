using UnityEngine;

namespace Projectiles
{
    public class Positioner
    {
        public delegate Vector3 Position(Vector3 currentPosition, float time);

        public static Position Directional(Vector3 dir)
        {
            return (_, _) => dir;
        }

        public static Position Polar()
        {
            return (_, t) => new Vector3(t * Mathf.Cos(t), t * Mathf.Sin(t));
        }
    }
}