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

        public static Position Fan(Vector2 direction, float radius, float spanOverDegrees, int numberOfBullets)
        {
            float spanOverRad = Mathf.Deg2Rad * spanOverDegrees;
            float angle = Mathf.Atan2(direction.y, direction.x);
            angle -= spanOverRad / 2;
            float increments = spanOverRad / numberOfBullets;
            return (_, t) => new Vector2(radius * Mathf.Cos(angle + t * increments), radius * Mathf.Sin(angle + t * increments));
        }

        public static Position Circle(int numberOfBullets, float radius, float initialRad = 0)
        {
            float spanOverRad = Mathf.PI * 2;
            float increments = spanOverRad / numberOfBullets;
            return (_, t) => new Vector2(radius * Mathf.Cos(initialRad + t * increments), radius * Mathf.Sin(initialRad + t * increments));
        }

        public static Position Identity()
        {
            return (currPos, _) => currPos;
        }

        public static Position Zero()
        {
            return (_, _) => Vector2.zero;
        }

        public static Position Homing(Transform transform)
        {
            return (currentPos, _) => ((Vector2)transform.position - currentPos).normalized;
        }
    }
}
