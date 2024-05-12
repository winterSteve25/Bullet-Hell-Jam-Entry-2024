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
                switch (i)
                {
                    case 0:
                        spawnPlace += new Vector2(0, 5);
                    break;
                    case 1:
                        spawnPlace += new Vector2(-5, 0);
                    break;
                    case 2:
                        spawnPlace += new Vector2(0, -5);
                    break;
                    case 3:
                        spawnPlace += new Vector2(5, 0);
                    break;
                    default:
                        spawnPlace += new Vector2(0, 0);
                    break;
                }
                if(Physics2D.OverlapBox(spawnPlace, new Vector2(1,1), 0) == null)
                {
                    spawner.Spawn(i, spawnPlace);
                }
            }
        }
    }
}
