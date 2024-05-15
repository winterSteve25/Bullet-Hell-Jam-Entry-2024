using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
    public class Layout
    {
        public readonly List<(Vector3Int, int, int)> Rooms;

        public Layout()
        {
            Rooms = new List<(Vector3Int, int, int)>();
        }

        public void AddRoom(Vector3Int pos, int width, int height)
        {
            Rooms.Add((pos, width, height));
        }

        public bool IsOccupied(Vector3Int position)
        {
            foreach (var (pos, width, height) in Rooms)
            {
                if (IsWithin(pos, width, height, position))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsTwoRoomsColliding(Vector3Int position1, int width1, int height1, Vector3Int position2, int width2, int height2)
        {
            Vector3Int min1 = position1;
            Vector3Int max1 = position1 + new Vector3Int(width1, height1, 0);

            Vector3Int min2 = position2;
            Vector3Int max2 = position2 + new Vector3Int(width2, height2, 0);

            bool overlapX = min1.x < max2.x && max1.x > min2.x;
            bool overlapY = min1.y < max2.y && max1.y > min2.y;

            return overlapX && overlapY;
        }

        public bool IsColliding(Vector3Int pos1, int width1, int height1)
        {
            foreach (var (pos, width, height) in Rooms)
            {
                if (IsTwoRoomsColliding(pos1, width1, height1, pos, width, height))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsWithin(Vector3Int position, int width, int height, Vector3Int point)
        {
            return point.x > position.x && point.x < position.x + width && point.y > position.y && point.y < position.y + height;
        }
    }
}
