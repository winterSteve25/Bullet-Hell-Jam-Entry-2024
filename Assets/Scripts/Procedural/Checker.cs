using UnityEngine;

namespace Procedural
{
    public class Checker : MonoBehaviour
    {
        public Room room { get; set; }
        private Spawner _spawner;

        public void CheckSpawn()
        {
            _spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
            foreach ((Vector3Int doorPos, int doorDirection) in room.Doors)
            {
                Vector3 spawnPlace = doorPos;

                if (Physics2D.OverlapBox((Vector2)spawnPlace, new Vector2(1, 1), 0) == null)
                {
                    _spawner.DrawRooms(room.Length, room.Height, transform.position);
                    _spawner.Spawn(doorDirection, spawnPlace);
                }
            }
        }
    }
}
