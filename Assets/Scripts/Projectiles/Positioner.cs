using UnityEngine;

namespace Projectiles
{
    public class Positioner
    {
        public delegate Vector3 Position(Vector3 currentPosition, float time);

        public static Position Directional(Vector3 dir)
        {
            dir.Normalize();
            return (_, _) => dir;
        }

        public static Position Polar()
        {
            return (_, t) => new Vector3(t * Mathf.Cos(t), t * Mathf.Sin(t));
        }

        public static Position Identity()
        {
            return (currPos, _) => currPos;
        }
        
        public static Position Zero()
        {
            return (_, _) => Vector3.zero;
        }
    }
}