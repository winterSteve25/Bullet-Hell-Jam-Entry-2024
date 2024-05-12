using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomManager : MonoBehaviour
{
    public GameObject[] obj_UpRooms;
    public GameObject[] obj_DownRooms;
    public GameObject[] obj_LeftRooms;
    public GameObject[] obj_RightRooms;
    public GameObject[] obj_AllRooms;
    public int RoomLimits = 0;
    public GameObject obj_FullRoom;

    public static Vector2 DirCheck(int DirID)
    {
        switch (DirID)
        {
            case 0:
                return new Vector2(0, 1);
            case 1:
                return new Vector2(-1, 0);
            case 2:
                return new Vector2(0, -1);
            case 3:
                return new Vector2(1, 0);
            default:
                return new Vector2(0, 0);
        }
    }

    public static Vector3Int DirCheckVec3Int(int DirID)
    {
        switch (DirID)
        {
            case 0:
                return new Vector3Int(0, 1, 0);
            case 1:
                return new Vector3Int(-1, 0, 0);
            case 2:
                return new Vector3Int(0, -1, 0);
            case 3:
                return new Vector3Int(1, 0, 0);
            default:
                return new Vector3Int(0, 0, 0);
        }
    }

    public static bool IsLength(Vector3Int check)
    {
        return check.x != 0;
    }

    public static Vector3Int[] CornersPos(Vector3Int startPos, int length, int height)
    {
        Vector3Int[] Corners = new Vector3Int[4];
        Corners[0] = new Vector3Int(startPos.x, startPos.y);
        Corners[1] = new Vector3Int(startPos.x, startPos.y - height);
        Corners[2] = new Vector3Int(startPos.x + length, startPos.y - height);
        Corners[3] = new Vector3Int(startPos.x + length, startPos.y);
        return Corners;
    }
    public static Vector3Int RandomPlacement(int DirID, Vector3Int[] Corners)
    {
        switch (DirID)
        {
            case 0:
                return new Vector3Int(Random.Range(Corners[0].x+1,Corners[3].x-1), Corners[0].y);
            case 1:
                return new Vector3Int(Corners[0].x, Random.Range(Corners[1].y+1,Corners[0].y-1));
            case 2:
                return new Vector3Int(Random.Range(Corners[1].x+1,Corners[2].x-1), Corners[1].y+1);
            case 3:
                return new Vector3Int(Corners[3].x-1, Random.Range(Corners[2].y+1,Corners[3].y-1));
            default:
                return new Vector3Int(0, 0, 0);
        }
    }
}
