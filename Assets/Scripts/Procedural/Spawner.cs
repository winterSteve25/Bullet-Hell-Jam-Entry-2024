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

        public void Spawn(int dir, Vector3 pos)
        {
            Debug.Log("Spawning");
            if (roomManager.roomLimits > 0)
            {
                Debug.Log("Spawning " + dir);
                GameObject spawnableRoom = dir switch
                {
                    0 => roomManager.objDownRooms[Random.Range(0, roomManager.objUpRooms.Count)],
                    1 => roomManager.objRightRooms[Random.Range(0, roomManager.objLeftRooms.Count)],
                    2 => roomManager.objUpRooms[Random.Range(0, roomManager.objDownRooms.Count)],
                    3 => roomManager.objLeftRooms[Random.Range(0, roomManager.objRightRooms.Count)],
                    _ => null
                };
                Checker roomChecker = spawnableRoom.GetComponent<Checker>();
                Vector3 spawnPlace = RoomManager.IsLength(DirectionExt.FromIndex(dir))
                        ? pos
                            + DirectionExt.ToUnitVector3Int(DirectionExt.FromIndex(dir))
                                * (roomChecker.room.Length/2)
                        : pos
                            + DirectionExt.ToUnitVector3Int(DirectionExt.FromIndex(dir))
                                * (roomChecker.room.Height/2);
                Instantiate(spawnableRoom, spawnPlace, Quaternion.identity);
                roomManager.roomLimits--;
            }
            else
            {
                Instantiate(roomManager.objFullRoom, pos, Quaternion.identity);
            }
        }
    }
}
