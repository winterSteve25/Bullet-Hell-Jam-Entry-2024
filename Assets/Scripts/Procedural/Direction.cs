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
        public static Vector3Int ToVectorOffset(this Direction direction)
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

        public static Vector2Int ToCornerVectorOffset(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Vector2Int(0, 1),
                Direction.Down => new Vector2Int(1, 0),
                Direction.Left => new Vector2Int(0, 0),
                Direction.Right => new Vector2Int(1, 1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Direction Random(Direction? exclude)
        {
            if (exclude == null)
            {
                return (Direction)UnityEngine.Random.Range(0, 4);
            }

            int num = UnityEngine.Random.Range(0, 4);
            if ((Direction)num == exclude)
            {
                return (Direction)((num + UnityEngine.Random.Range(1, 4)) % 4);
            }

            return (Direction)num;
        }

        public static Direction Opposite(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static Direction Adjacent(this Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Left,
                Direction.Down => Direction.Right,
                Direction.Left => Direction.Down,
                Direction.Right => Direction.Up,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}
