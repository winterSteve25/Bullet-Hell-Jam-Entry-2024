using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    [SerializeField]
    private bool[] openSide = new bool[4];
    
    Spawner spawner;


    void Start()
    {
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        for (int i = 0; i < openSide.Length; i++)
        {
            Vector2 spawnPlace = transform.position;
            if(openSide[i])
            {
                // 1 = Up, 2 = Left, 3 = Down, 4 = Right
                spawnPlace += RoomManager.DirCheck(i)*5;
                if(Physics2D.OverlapBox(spawnPlace, new Vector2(1,1), 0) == null)
                {
                    spawner.Spawn(i, spawnPlace);
                }
            }
        }
    }
}
