using System;
using UnityEngine;

namespace Procedural
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    public static class DirectionExt
    {
        public static int ToIndex(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => 0,
                Direction.Down => 2,
                Direction.Left => 1,
                Direction.Right => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Vector2 ToUnitVector(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Vector2.up,
                Direction.Down => Vector2.down,
                Direction.Left => Vector2.left,
                Direction.Right => Vector2.right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Vector3Int ToUnitVector3Int(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Vector3Int.up,
                Direction.Down => Vector3Int.down,
                Direction.Left => Vector3Int.left,
                Direction.Right => Vector3Int.right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Direction FromIndex(int i)
        {
            return i switch
            {
                0 => Direction.Up,
                1 => Direction.Left,
                2 => Direction.Down,
                3 => Direction.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
            };
        }

        public static Vector3Int FromVector3(Vector3 vector3)
        {
            return new Vector3Int((int)vector3.x, (int)vector3.y, (int)vector3.z);
        }
    }
}
