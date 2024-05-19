using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Procedural
{
    public class RoomTrigger : MonoBehaviour
    {
        public static List<int> Triggered;

        [SerializeField] private int roomIdx;
        private List<(RectInt, bool)> _build;
        private GameObject[] _enemies;
        private Tilemap _tm;

        public void Init(Tilemap tm, List<(RectInt, bool)> build, GameObject[] enemies, int roomIdx, Vector3Int constAxis, Vector3Int size)
        {
            this.roomIdx = roomIdx;
            _tm = tm;
            _build = build;
            _enemies = enemies;

            Vector3Int scale = size * constAxis;
            if (scale.x == 0)
            {
                scale.x = 1;
            }
            if (scale.y == 0)
            {
                scale.y = 1;
            }
            transform.localScale = new Vector3(scale.x, scale.y, 1);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!Triggered.Contains(roomIdx))
            {
                Destroy(gameObject);
                return;
            }

            if (!other.CompareTag("Player")) return;
            var (rect, premade) = _build[roomIdx];
            if (premade) return;

            for (int i = 0; i < 5; i++)
            {
                GameObject en = Instantiate(_enemies[Random.Range(0, _enemies.Length)]);
                en.transform.position = _tm.CellToWorld(new Vector3Int(Random.Range(rect.x + 2, rect.xMax - 2), Random.Range(rect.y + 2, rect.yMax - 2)));
            }

            Triggered.Add(roomIdx);
        }
    }
}
