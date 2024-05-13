using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;


namespace Procedural
{
    public class Spawner : MonoBehaviour
    {
        public RoomManager roomManager;
        private int _spawnLimits;
        [SerializeField]
        private Tilemap tilemap;

        [SerializeField]
        private TileBase tileBase;

        private void Start()
        {
            _spawnLimits = roomManager.roomLimits;
            StartCoroutine(delay());
        }
        IEnumerator delay()
        {
            yield return new WaitForSeconds(1f);
            Spawn(2, transform.position);
        }
        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
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
                GameObject room = Instantiate(spawnableRoom, spawnPlace, Quaternion.identity);
                room.GetComponent<Checker>().room = spawnableRoom.GetComponent<Checker>().room;
                room.GetComponent<Checker>().CheckSpawn();
                roomManager.roomLimits--;
            }
            /*else
            {
                Instantiate(roomManager.objFullRoom, pos, Quaternion.identity);
            }*/
        }
        public void DrawRooms(int length, int height, Vector3 centerPos)
        {
            Debug.Log("drawing "+ centerPos);
            Vector3Int startPos = tilemap.WorldToCell(
                new Vector3(-length / 2, height / 2, 0) + centerPos
            );

            for (int j = startPos.y; j > startPos.y - height; j--)
            {
                for (int i = startPos.x; i < startPos.x + length; i++)
                {
                    if (
                        j == startPos.y
                        || j == startPos.y - height + 1
                        || i == startPos.x
                        || i == startPos.x + length - 1
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
        }
    }
}
