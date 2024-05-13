using UnityEngine;
using UnityEngine.Tilemaps;

namespace Procedural.RoomGeneration
{
    public class RoomGen : MonoBehaviour
    {

        [SerializeField]
        private GameObject objRoom;

        [SerializeField]
        private RoomManager roomManager;

        [SerializeField]
        private int roomLimits;

        [Header("Room Sizes")]
        [SerializeField]
        private int minLength;

        [SerializeField]
        private int maxLength;

        [SerializeField]
        private int minHeight;

        [SerializeField]
        private int maxHeight;

        private void Start()
        {
            for (int i = 0; i < roomLimits; i++)
            {
                // + 1 b/c Range is exclusive on the top
                GenerateRooms(
                    Random.Range(minLength, maxLength + 1),
                    Random.Range(minHeight, maxHeight + 1),
                    DirectionExt.FromVector3(transform.position)
                );
            }
        }

        public void AddRoom(GameObject room, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    roomManager.objUpRooms.Add(room);
                    break;
                case Direction.Left:
                    roomManager.objLeftRooms.Add(room);
                    break;
                case Direction.Down:
                    roomManager.objDownRooms.Add(room);
                    break;
                case Direction.Right:
                    roomManager.objRightRooms.Add(room);
                    break;
            }
        }

        private void GenerateRooms(int length, int height, Vector3Int startPos)
        {
            GameObject room = Instantiate(objRoom, new Vector2(100,100), Quaternion.identity);
            Checker roomChecker = room.GetComponent<Checker>();
            roomChecker.room = new Room(length, height);
            int doorsCount = Random.Range(1, 5);
            int k = 0;
            while (k < doorsCount)
            {
                int ranDir = Random.Range(0, 4);
                Vector3Int[] corners = RoomManager.CornersPos(startPos, length, height);
                roomChecker.room.Doors.Add(
                    (RoomManager.RandomPlacement(DirectionExt.FromIndex(ranDir), corners), ranDir)
                );
                AddRoom(room, DirectionExt.FromIndex(ranDir));
                k++;
            }
        }

        private void AddDoors(Vector3Int startPos, Direction direction)
        {
            Debug.Log(direction);
            for (int i = 0; i < 3; i++)
            {
                //tilemap.SetTile(startPos, null);
                Vector3Int test = RoomManager.IsLength(direction.ToUnitVector3Int())
                    ? new Vector3Int(0, -1, 0)
                    : new Vector3Int(1, 0, 0);
                startPos += test;
            }
        }
    }
}
