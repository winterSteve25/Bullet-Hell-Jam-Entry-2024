using UnityEngine;

namespace Projectiles
{
    public class Positioner
    {
        public delegate Vector2 Position(Vector2 currentPosition, float time);

        public static Position Directional(Vector2 dir)
        {
            dir.Normalize();
            return (_, _) => dir;
        }

        public static Position Outward(Vector2 origin)
        {
            return (currPos, _) => (currPos - origin).normalized;
        }

        public static Position Polar(float startAngleInRad, float radius, float tMul)
        {
            return (_, t) => new Vector2(radius * Mathf.Cos(startAngleInRad + t * tMul), radius * Mathf.Sin(startAngleInRad + t * tMul));
        }

        public static Position Identity()
        {
            return (currPos, _) => currPos;
        }

        public static Position Zero()
        {
            return (_, _) => Vector2.zero;
        }
    }
}
