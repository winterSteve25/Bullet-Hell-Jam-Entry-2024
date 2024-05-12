using UnityEngine;

namespace Procedural
{
    public class Spawner : MonoBehaviour
    {
        public RoomManager roomManager;
        private int _spawnLimits;

        private void Start()
        {
            _spawnLimits = roomManager.roomLimits;
        }

        public void Spawn(int dir, Vector2 pos)
        {
            Debug.Log("Spawning");
            if (roomManager.roomLimits > 0)
            {
                Debug.Log("Spawning " + dir);
                GameObject spawnableRoom = dir switch
                {
                    0 => roomManager.objDownRooms[Random.Range(0, roomManager.objUpRooms.Length)],
                    1 => roomManager.objRightRooms[Random.Range(0, roomManager.objLeftRooms.Length)],
                    2 => roomManager.objUpRooms[Random.Range(0, roomManager.objDownRooms.Length)],
                    3 => roomManager.objLeftRooms[Random.Range(0, roomManager.objRightRooms.Length)],
                    _ => null
                };
                Instantiate(spawnableRoom, pos, Quaternion.identity);
                roomManager.roomLimits--;
            }
            else
            {
                Instantiate(roomManager.objFullRoom, pos, Quaternion.identity);
            }
        }
    }
}