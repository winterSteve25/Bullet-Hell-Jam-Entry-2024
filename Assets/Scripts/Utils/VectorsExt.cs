using UnityEngine;

namespace Utils
{
    public static class VectorsExt
    {
        public static Vector2 Rotate(this Vector2 vector, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);
            float x = vector.x;
            float y = vector.y;

            vector.x = x * cos - y * sin;
            vector.y = x * sin + y * cos;

            return vector;
        }

        public static Vector2 Rotate(this Vector3Int vector, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);
            float x = vector.x;
            float y = vector.y;
            return new Vector2(x * cos - y * sin, x * sin + y * cos);
        }
    }
}
