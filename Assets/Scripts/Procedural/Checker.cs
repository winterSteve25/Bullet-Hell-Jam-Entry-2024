using UnityEngine;

namespace Procedural
{
    public class Checker : MonoBehaviour
    {
        [SerializeField]
        private bool[] openSide = new bool[4];
        private Spawner _spawner;

        private void Start()
        {
            _spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
            for (int i = 0; i < openSide.Length; i++)
            {
                Vector2 spawnPlace = transform.position;
                if (!openSide[i]) continue;

                spawnPlace += DirectionExt.FromIndex(i).ToUnitVector() * 5;
                if (Physics2D.OverlapBox(spawnPlace, new Vector2(1, 1), 0) == null)
                {
                    _spawner.Spawn(i, spawnPlace);
                }
            }
        }
    }
}