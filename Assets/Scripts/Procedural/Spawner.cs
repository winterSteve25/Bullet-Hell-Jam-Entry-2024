using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public RoomManager roomManager;
    int spawnLimits;

    void Start()
    {
        spawnLimits = roomManager.RoomLimits;
    }
    public void Spawn(int dir, Vector2 pos)
    {
        Debug.Log("Spawning");
        if(roomManager.RoomLimits > 0)
        {
            Debug.Log("Spawning " + dir);
            GameObject spawnableRoom;
            switch (dir)
            {
                case 0:
                    spawnableRoom = roomManager.obj_DownRooms[Random.Range(0, roomManager.obj_UpRooms.Length)];
                    break;
                case 1:
                    spawnableRoom = roomManager.obj_RightRooms[Random.Range(0, roomManager.obj_LeftRooms.Length)];
                    break;
                case 2:
                    spawnableRoom = roomManager.obj_UpRooms[Random.Range(0, roomManager.obj_DownRooms.Length)];
                    break;
                case 3:
                    spawnableRoom = roomManager.obj_LeftRooms[Random.Range(0, roomManager.obj_RightRooms.Length)];
                    break;
                default:
                    spawnableRoom = null;
                break;
            }
            Instantiate(spawnableRoom, pos, Quaternion.identity);
            roomManager.RoomLimits--;
        }else
        {
            Instantiate(roomManager.obj_FullRoom, pos, Quaternion.identity);
        }
    }
}
