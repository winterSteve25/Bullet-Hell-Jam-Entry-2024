using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Procedural
{
    public class RoomTrigger : MonoBehaviour
    {
        public static List<int> Triggered;
        public static event Action<int> EnemyDiedInRoom;
        public static event Action<int> EnteredRoom;

        [SerializeField] private int roomIdx;

        private List<(RectInt, bool)> _build;
        private Enemy[] _enemies;
        private Tilemap _tm;
        private int _numberOfEnemies;
        private int _numberDied;
        private TileBase _tile;
        private List<(Vector3Int, Vector3Int)> _fills;

        private void OnDisable()
        {
            EnemyDiedInRoom -= OnEnemyDied;
            EnteredRoom -= OnEnteredRoom;
        }

        public void Init(Tilemap tm, List<(RectInt, bool)> build, Enemy[] enemies, int room, Vector3Int constAxis, Vector3Int size, Vector3Int pos, TileBase tile, int maxEnemies, int minEnemies)
        {
            roomIdx = room;
            _tm = tm;
            _build = build;
            _enemies = enemies;
            _numberOfEnemies = Random.Range(minEnemies, maxEnemies);
            _tile = tile;
            _fills = new List<(Vector3Int, Vector3Int)>();
            AddFill(pos, size, constAxis);
            RectInt roomRect = build[room].Item1;
            Rect rect = new Rect(roomRect.x, roomRect.y, roomRect.width, roomRect.height);

            rect.xMin += 2f;
            rect.yMin += 2f;
            rect.xMax -= 2f;
            rect.yMax -= 2f;

            var transform1 = transform;
            transform1.localScale = new Vector3(rect.width, rect.height, 1);
            transform1.position = rect.center;
            EnemyDiedInRoom += OnEnemyDied;
            EnteredRoom += OnEnteredRoom;
        }

        public void AddFill(Vector3Int pos, Vector3Int tunnelSize, Vector3Int constAxis)
        {
            Vector3Int s = tunnelSize * constAxis;
            s.z = 1;

            if (s.x == 0)
            {
                s.x = 1;
            }
            if (s.y == 0)
            {
                s.y = 1;
            }

            _fills.Add((pos, s));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Triggered.Contains(roomIdx))
            {
                return;
            }

            if (!other.CompareTag("Player")) return;

            var (rect, premade) = _build[roomIdx];
            EnteredRoom?.Invoke(roomIdx);
            if (premade) return;

            for (int i = 0; i < _numberOfEnemies; i++)
            {
                Enemy en = Instantiate(_enemies[Random.Range(0, _enemies.Length)]);
                en.transform.position = _tm.CellToWorld(new Vector3Int(Random.Range(rect.x + 2, rect.xMax - 2), Random.Range(rect.y + 2, rect.yMax - 2)));
                en.belongsToRoom = roomIdx;
            }

            Triggered.Add(roomIdx);
        }

        private void OnEnemyDied(int room)
        {
            if (room != roomIdx) return;

            _numberDied++;
            if (_numberDied >= _numberOfEnemies)
            {
                Clear();
            }
        }

        private void OnEnteredRoom(int room)
        {
            if (room != roomIdx) return;
            foreach (var (pos, size) in _fills)
            {
                _tm.SetTilesBlock(new BoundsInt(pos, size), Enumerable.Range(0, size.x * size.y).Select(_ => _tile).ToArray());
            }
        }

        private void Clear()
        {
            foreach (var (pos, size) in _fills)
            {
                _tm.SetTilesBlock(new BoundsInt(pos, size), Enumerable.Range(0, size.x * size.y).Select(_ => (TileBase)null).ToArray());
            }

            Destroy(gameObject);
        }

        public static void TriggerEnemyDiedInRoom(int room)
        {
            EnemyDiedInRoom?.Invoke(room);
        }
    }
}
