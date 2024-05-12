using UnityEngine;

namespace Procedural
{
    public class RoomManager : MonoBehaviour
    {
        public GameObject[] objUpRooms;
        public GameObject[] objDownRooms;
        public GameObject[] objLeftRooms;
        public GameObject[] objRightRooms;
        public GameObject[] objAllRooms;
        public int roomLimits;
        public GameObject objFullRoom;

        public static bool IsLength(Vector3Int check)
        {
            return check.x != 0;
        }

        public static Vector3Int[] CornersPos(Vector3Int startPos, int length, int height)
        {
            Vector3Int[] corners = new Vector3Int[4];
            corners[0] = new Vector3Int(startPos.x, startPos.y);
            corners[1] = new Vector3Int(startPos.x, startPos.y - height);
            corners[2] = new Vector3Int(startPos.x + length, startPos.y - height);
            corners[3] = new Vector3Int(startPos.x + length, startPos.y);
            return corners;
        }

        public static Vector3Int RandomPlacement(int dirID, Vector3Int[] corners)
        {
            return dirID switch
            {
                0 => new Vector3Int(Random.Range(corners[0].x + 1, corners[3].x - 1), corners[0].y),
                1 => new Vector3Int(corners[0].x, Random.Range(corners[1].y + 1, corners[0].y - 1)),
                2 => new Vector3Int(Random.Range(corners[1].x + 1, corners[2].x - 1), corners[1].y + 1),
                3 => new Vector3Int(corners[3].x - 1, Random.Range(corners[2].y + 1, corners[3].y - 1)),
                _ => new Vector3Int(0, 0, 0)
            };
        }
    }
}