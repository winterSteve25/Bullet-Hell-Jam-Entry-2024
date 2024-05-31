using System;
using UnityEngine;
using Utils;

namespace Projectiles
{
    public class Positioner
    {
        public delegate Vector2 Position(Vector2 currentPosition, float time, int index);

        public static Position Directional(Vector2 dir)
        {
            dir.Normalize();
            return (_, _, _) => dir;
        }

        public static Position Outward(Vector2 origin)
        {
            return (currPos, _, _) => (currPos - origin).normalized;
        }

        public static Position Outward(Vector2 origin, float rot)
        {
            return (currPos, _, _) => (currPos - origin).normalized.Rotate(rot);
        }

        public static Position Sin()
        {
            return (_, _, i) => new Vector2(Mathf.Sin(i), Mathf.Sin(i));
        }

        public static Position Fan(Vector2 direction, float radius, float spanOverDegrees, int numberOfBullets)
        {
            float spanOverRad = Mathf.Deg2Rad * spanOverDegrees;
            float angle = Mathf.Atan2(direction.y, direction.x);
            angle -= spanOverRad / 2;
            float increments = spanOverRad / numberOfBullets;
            return (_, _, i) => new Vector2(radius * Mathf.Cos(angle + i * increments), radius * Mathf.Sin(angle + i * increments));
        }

        public static Position Circle(int numberOfBullets, float radius, float initialRad = 0)
        {
            float spanOverRad = Mathf.PI * 2;
            float increments = spanOverRad / numberOfBullets;
            return (_, _, i) => new Vector2(radius * Mathf.Cos(initialRad + i * increments), radius * Mathf.Sin(initialRad + i * increments));
        }

        public static Position Polar(float spanOver, int numberOfBullets, Func<float, float> polarFunction, float initialRad = 0)
        {
            float spanOverRad = spanOver;
            float increments = spanOverRad / numberOfBullets;
            return (_, _, i) =>
            {
                float theta = initialRad + i * increments;
                float function = polarFunction(theta);
                return new Vector2(function * Mathf.Cos(theta), function * Mathf.Sin(theta));
            };
        }

        public static Position Polar(float spanOver, int numberOfBullets, Func<Vector2, float, float, Vector2> polarFunction, float initialRad = 0)
        {
            float spanOverRad = spanOver;
            float increments = spanOverRad / numberOfBullets;
            return (curr, t, i) =>
            {
                float theta = initialRad + i * increments;
                return polarFunction(curr, t, theta);
            };
        }

        public static Position Spiral()
        {
            return (pos, _, i) =>
            {
                float r = i / pos.magnitude * 2;
                return new Vector2(r * Mathf.Cos(r), r * Mathf.Sin(r));
            };
        }

        public static Position Identity()
        {
            return (currPos, _, _) => currPos;
        }

        public static Position Zero()
        {
            return (_, _, _) => Vector2.zero;
        }

        public static Position Homing(Transform transform)
        {
            return (currentPos, _, _) => ((Vector2)transform.position - currentPos).normalized;
        }
    }
}
