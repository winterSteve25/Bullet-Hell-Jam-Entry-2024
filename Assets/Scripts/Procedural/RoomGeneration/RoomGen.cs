using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGen : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tileBase;
    public int Length;
    public int Height;
    bool[] doorBool = new bool[4];

    void Start()
    {
        Length = Random.Range(10, 32);
        Height = Random.Range(10, 32);
        generate(Length, Height);
    }

    void Update() { }

    void generate(int x, int y)
    {
        Vector3Int startPos = tilemap.WorldToCell(
            new Vector3(-x / 2, y / 2, 0) + transform.position
        );
        for (int j = startPos.y - 1; j > startPos.y - y; j--)
        {
            for (int i = startPos.x; i < startPos.x + x; i++)
            {
                if (
                    j == startPos.y - 1
                    || j == startPos.y - y + 1
                    || i == startPos.x
                    || i == startPos.x + x - 1
                )
                {
                    tilemap.SetTile(new Vector3Int(i, j, 0), tileBase);
                }
                else
                {
                    //draw floors
                }
            }
        }
        int doorsCount = Random.Range(1, 4);
        int k = 0;
        while (k < doorsCount)
        {
            int ranDir = Random.Range(1, 4);
            if (!doorBool[ranDir])
            {
                doorBool[ranDir] = true;
                Vector3Int[] corners = RoomManager.CornersPos(startPos, Length, Height);

                addDoors(RoomManager.RandomPlacement(ranDir, corners), ranDir);
                k++;
            }
        }
    }

    void addDoors(Vector3Int startPos, int DirID)
    {
        Debug.Log(startPos);
        for (int i = 0; i < 3; i++)
        {
            tilemap.SetTile(startPos, null);
            Vector3Int test = RoomManager.IsLength(RoomManager.DirCheckVec3Int(DirID))
                ? new Vector3Int(0, -1, 0)
                : new Vector3Int(1, 0, 0);
            startPos += test;
        }
    }
}
