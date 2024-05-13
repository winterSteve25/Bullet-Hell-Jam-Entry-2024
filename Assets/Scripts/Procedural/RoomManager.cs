using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
    public class Room
    {
        public int Length { get; set; } = 0;
        public int Height { get; set; } = 0;
        public List<(Vector3Int doorPos, int doorDirection)> Doors { get; set; } =
            new List<(Vector3Int, int)>();

        public Room(int length, int height)
        {
            this.Length = length;
            this.Height = height;
        }

        public Room() { }
    }

    public class RoomManager : MonoBehaviour
    {
        public List<GameObject> objUpRooms;
        public List<GameObject> objDownRooms;
        public List<GameObject> objLeftRooms;
        public List<GameObject> objRightRooms;
        public List<GameObject> objAllRooms;
        public int roomLimits;
        public GameObject objFullRoom;

        public static bool IsLength(Vector3Int check)
        {
            return check.x != 0;
        }

        public static bool IsLength(Direction direction)
        {
            return (direction == Direction.Up || direction == Direction.Down) ? true : false;
        }

        public static Vector3Int[] CornersPos(Vector3Int startPos, int length, int height)
        {
            Vector3Int[] corners = new Vector3Int[4];
            corners[0] = new Vector3Int(startPos.x, startPos.y);
            corners[1] = new Vector3Int(startPos.x, startPos.y - height + 1);
            corners[2] = new Vector3Int(startPos.x + length - 1, startPos.y - height + 1);
            corners[3] = new Vector3Int(startPos.x + length - 1, startPos.y);
            return corners;
        }

        public static Vector3Int RandomPlacement(Direction direction, Vector3Int[] corners)
        {
            return direction switch
            {
                Direction.Up
                    => new Vector3Int(
                        UnityEngine.Random.Range(corners[0].x + 1, corners[3].x - 3),
                        corners[0].y
                    ),
                Direction.Left
                    => new Vector3Int(
                        corners[1].x,
                        UnityEngine.Random.Range(corners[1].y + 3, corners[0].y - 1)
                    ),
                Direction.Down
                    => new Vector3Int(
                        UnityEngine.Random.Range(corners[1].x + 1, corners[2].x - 3),
                        corners[1].y
                    ),
                Direction.Right
                    => new Vector3Int(
                        corners[2].x,
                        UnityEngine.Random.Range(corners[2].y + 3, corners[3].y - 1)
                    ),
                _ => new Vector3Int(0, 0, 0)
            };
        }
    }
}
