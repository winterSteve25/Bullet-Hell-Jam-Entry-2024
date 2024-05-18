using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Player;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Procedural
{
    public class RoomGenerator : MonoBehaviour
    {
        [SerializeField] private TileBase wall;
        [SerializeField] private TileBase floor;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tilemap floorTilemap;
        [SerializeField] private List<PremadeRoom> premadeRooms;
        [SerializeField] private PlayerCombat playerPrefab;
        [SerializeField] private Camera cam;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private FurnishedRoom Create(Vector2Int offset, int width, int height)
        {
            List<(Vector3Int, int)> doors = new List<(Vector3Int, int)>();
            Dictionary<Vector3Int, TileBase> floorTiles = new Dictionary<Vector3Int, TileBase>();
            Dictionary<Vector3Int, GameObject> props = new Dictionary<Vector3Int, GameObject>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector3Int pos = new Vector3Int(i + offset.x, j + offset.y);

                    if (j == 0 && i == 0 ||
                        j == 0 && i == width - 1 ||
                        i == 0 && j == height - 1 ||
                        i == width - 1 && j == height - 1 ||
                        i == 0 ||
                        i == width - 1 ||
                        j == height - 1 ||
                        j == 0)
                    {
                        continue;
                    }

                    floorTiles.Add(pos, floor);
                }
            }

            return new FurnishedRoom(doors, floorTiles, props, width, height);
        }

        private void Place(FurnishedRoom room)
        {
            Vector3Int[] positionArray = room.FloorTiles.Keys.ToArray();
            tilemap.SetTiles(positionArray, Enumerable.Range(0, room.FloorTiles.Keys.Count).Select(_ => (TileBase)null).ToArray());
            floorTilemap.SetTiles(positionArray, room.FloorTiles.Values.ToArray());
        }

        private bool TryBuildTunnel(List<(RectInt, bool)> rooms, RectInt room1, RectInt room2, out List<TunnelJoint> joints, out Vector3Int otherDoor)
        {
            int rangeXFrom = Mathf.Max(room1.xMin, room2.xMin);
            int rangeXto = Mathf.Min(room1.xMax, room2.xMax);

            // tunnel size is 2 so
            if (rangeXto - rangeXFrom > 4)
            {
                int pathPosX = Random.Range(rangeXFrom + 1, rangeXto - 3);
                if (room1.yMax < room2.yMin)
                {
                    // vertical, from the top of the first room to the bottom of the second
                    Vector3Int point1 = new Vector3Int(pathPosX, room1.yMax);
                    Vector3Int point2 = new Vector3Int(pathPosX, room2.yMin);
                    point1.y += 1;
                    point2.y -= 1;

                    if (GoesThroughRooms(rooms, point1, point2))
                    {
                        joints = null;
                        otherDoor = new Vector3Int();
                        return false;
                    }

                    if (GoesThroughRooms(rooms, point1 + new Vector3Int(2, 0), point2 + new Vector3Int(2, 0)))
                    {
                        joints = null;
                        otherDoor = new Vector3Int();
                        return false;
                    }

                    joints = new List<TunnelJoint>
                    {
                        new TunnelJoint(point1, Direction.Up), new TunnelJoint(point2, Direction.Down)
                    };

                    otherDoor = point2;
                }
                else
                {
                    // vertical, from the bottom of the first room to the top of the second
                    Vector3Int point1 = new Vector3Int(pathPosX, room2.yMax);
                    Vector3Int point2 = new Vector3Int(pathPosX, room1.yMin);
                    point1.y += 1;
                    point2.y -= 1;

                    if (GoesThroughRooms(rooms, point1, point2))
                    {
                        joints = null;
                        otherDoor = new Vector3Int();
                        return false;
                    }

                    if (GoesThroughRooms(rooms, point1 + new Vector3Int(2, 0), point2 + new Vector3Int(2, 0)))
                    {
                        joints = null;
                        otherDoor = new Vector3Int();
                        return false;
                    }

                    joints = new List<TunnelJoint>
                    {
                        new TunnelJoint(point1, Direction.Up), new TunnelJoint(point2, Direction.Down)
                    };

                    otherDoor = point1;
                }

                return true;
            }

            int rangeYFrom = Mathf.Max(room1.yMin, room2.yMin);
            int rangeYTo = Mathf.Min(room1.yMax, room2.yMax);

            if (rangeYTo - rangeYFrom > 4)
            {
                int pathPosY = Random.Range(rangeYFrom + 1, rangeYTo - 3);
                if (room1.xMin > room2.xMax)
                {
                    // horizontal, from the left of the first to the right of the second
                    Vector3Int point1 = new Vector3Int(room2.xMax, pathPosY);
                    Vector3Int point2 = new Vector3Int(room1.xMin, pathPosY);
                    point1.x += 1;
                    point2.x -= 1;

                    if (GoesThroughRooms(rooms, point1, point2))
                    {
                        joints = null;
                        otherDoor = new Vector3Int();
                        return false;
                    }

                    if (GoesThroughRooms(rooms, point1 + new Vector3Int(0, 2), point2 + new Vector3Int(0, 2)))
                    {
                        joints = null;
                        otherDoor = new Vector3Int();
                        return false;
                    }

                    joints = new List<TunnelJoint>
                    {
                        new TunnelJoint(point1, Direction.Right), new TunnelJoint(point2, Direction.Left)
                    };

                    otherDoor = point2;
                }
                else
                {
                    // horizontal, from the right of the first to the left of the second
                    Vector3Int point1 = new Vector3Int(room1.xMax, pathPosY);
                    Vector3Int point2 = new Vector3Int(room2.xMin, pathPosY);
                    point1.x += 1;
                    point2.x -= 1;

                    if (GoesThroughRooms(rooms, point1, point2))
                    {
                        joints = null;
                        otherDoor = new Vector3Int();
                        return false;
                    }

                    if (GoesThroughRooms(rooms, point1 + new Vector3Int(0, 2), point2 + new Vector3Int(0, 2)))
                    {
                        joints = null;
                        otherDoor = new Vector3Int();
                        return false;
                    }

                    joints = new List<TunnelJoint>
                    {
                        new TunnelJoint(point1, Direction.Right), new TunnelJoint(point2, Direction.Left)
                    };

                    otherDoor = point2;
                }

                return true;
            }

            joints = null;
            otherDoor = new Vector3Int();
            return false;
        }

        private bool GoesThroughRooms(List<(RectInt, bool)> rooms, Vector3Int p1, Vector3Int p2)
        {
            foreach (var (r, _) in rooms)
            {
                if (LineRectTest(p1.x, p1.y, p2.x, p2.y, r.x, r.y, r.width, r.height))
                {
                    return true;
                }
            }

            return false;
        }

        // LINE/RECTANGLE
        private static bool LineRectTest(float x1, float y1, float x2, float y2, float rx, float ry, float rw, float rh)
        {
            if (LineLine(x1, y1, x2, y2, rx, ry, rx, ry + rh) ||
                LineLine(x1, y1, x2, y2, rx + rw, ry, rx + rw, ry + rh) ||
                LineLine(x1, y1, x2, y2, rx, ry, rx + rw, ry) ||
                LineLine(x1, y1, x2, y2, rx, ry + rh, rx + rw, ry + rh)
               )
            {
                return true;
            }
            return false;
        }

        private static bool LineLine(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            // calculate the direction of the lines
            float uA = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));
            float uB = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / ((y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1));

            // if uA and uB are between 0-1, lines are colliding
            return uA is >= 0 and <= 1 && uB is >= 0 and <= 1;
        }

        public bool BuildLayout(Layout builtLayout)
        {
            BoundsInt wholeBounds = new BoundsInt(builtLayout.Whole.x - 100, builtLayout.Whole.y - 100, 0, builtLayout.Whole.width + 200, builtLayout.Whole.height + 200, 1);
            tilemap.SetTilesBlock(wholeBounds, Enumerable.Range(0, wholeBounds.size.x * wholeBounds.size.y).Select(_ => wall).ToArray());

            List<(RectInt, bool)> lastBuild = builtLayout.LastBuild;
            Dictionary<int, (List<List<TunnelJoint>>, List<Vector3Int>)> tunnels = new Dictionary<int, (List<List<TunnelJoint>>, List<Vector3Int>)>();
            Dictionary<int, List<int>> tunnelRecord = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> tunnelRecordBackward = new Dictionary<int, List<int>>();

            foreach (var connection in builtLayout.Connection)
            {
                if (tunnelRecord.TryGetValue(connection.Item1, out var dest))
                {
                    if (dest.Contains(connection.Item2))
                    {
                        Debug.LogWarning("Duplicate connection");
                        continue;
                    }
                }

                if (tunnelRecordBackward.TryGetValue(connection.Item1, out var dest2))
                {
                    if (dest2.Contains(connection.Item2))
                    {
                        Debug.LogWarning("Duplicate connection");
                        continue;
                    }
                }

                if (!TryBuildTunnel(lastBuild, lastBuild[connection.Item1].Item1, lastBuild[connection.Item2].Item1, out var joints, out var otherDoor))
                {
                    continue;
                }

                if (!tunnels.ContainsKey(connection.Item1))
                {
                    tunnels.Add(connection.Item1, (new List<List<TunnelJoint>>(), new List<Vector3Int>()));
                }

                if (!tunnels.ContainsKey(connection.Item2))
                {
                    tunnels.Add(connection.Item2, (new List<List<TunnelJoint>>(), new List<Vector3Int>()));
                }

                if (!tunnelRecord.ContainsKey(connection.Item1))
                {
                    tunnelRecord.Add(connection.Item1, new List<int>());
                }

                if (!tunnelRecordBackward.ContainsKey(connection.Item2))
                {
                    tunnelRecordBackward.Add(connection.Item2, new List<int>());
                }

                tunnels[connection.Item1].Item1.Add(joints);
                tunnels[connection.Item2].Item2.Add(otherDoor);
                tunnelRecord[connection.Item1].Add(connection.Item2);
                tunnelRecordBackward[connection.Item2].Add(connection.Item1);
            }

            for (int i = 0; i < lastBuild.Count; i++)
            {
                var (r, premade) = lastBuild[i];

                if (!premade)
                {
                    Place(Create(r.position, r.width, r.height));
                }

                if (!tunnels.ContainsKey(i))
                {
                    if (premade)
                    {
                        return false;
                    }
                    continue;
                }

                // todo: place tunnels
                foreach (List<TunnelJoint> tunnelsJ in tunnels[i].Item1)
                {
                    PlaceTunnel(tunnelsJ);
                }
            }

            foreach (var (premadeRoom, rect) in builtLayout.PremadeRooms)
            {
                BoundsInt boundsInt = new BoundsInt(rect.x, rect.y, 0, rect.width, rect.height, 1);
                floorTilemap.SetTilesBlock(boundsInt, Enumerable.Range(0, boundsInt.size.x * boundsInt.size.y).Select(_ => floor).ToArray());
                tilemap.SetTilesBlock(boundsInt, new TileBase[boundsInt.size.x * boundsInt.size.y]);
                PremadeRoom room = Instantiate(premadeRoom, transform);
                room.transform.position = tilemap.CellToWorld(new Vector3Int(rect.x, rect.y));
            }

            return true;
        }

        private void PlaceTunnel(List<TunnelJoint> joints)
        {
            for (int i = 0; i < joints.Count - 1; i++)
            {
                TunnelJoint first = joints[i];
                TunnelJoint next = joints[i + 1];
                Vector3Int size = new Vector3Int();
                Vector3Int tunnelStartPos = first.position;

                switch (first.direction)
                {
                    case Direction.Up:
                        size.x = 2;
                        size.y = Mathf.Abs(next.position.y - first.position.y) + 4;
                        tunnelStartPos.x += 1;
                        tunnelStartPos.y -= 2;
                        break;
                    case Direction.Down:
                        size.x = 2;
                        size.y = Mathf.Abs(next.position.y - first.position.y) + 4;
                        tunnelStartPos.x += 1;
                        tunnelStartPos.y += 2;
                        break;
                    case Direction.Left:
                        size.x = Mathf.Abs(next.position.x - first.position.x) + 4;
                        size.y = 2;
                        tunnelStartPos.y += 1;
                        tunnelStartPos.x += 2;
                        break;
                    case Direction.Right:
                        size.x = Mathf.Abs(next.position.x - first.position.x) + 4;
                        size.y = 2;
                        tunnelStartPos.y += 1;
                        tunnelStartPos.x -= 2;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                size.z = 1;
                BoundsInt bounds = new BoundsInt(tunnelStartPos, size);
                TileBase[] tiles = Enumerable.Range(0, bounds.size.x * bounds.size.y).Select(_ => floor).ToArray();
                TileBase[] air = new TileBase[bounds.size.x * bounds.size.y];

                floorTilemap.SetTilesBlock(bounds, tiles);
                tilemap.SetTilesBlock(bounds, air);
            }
        }

        private Layout _layout;

        private void Start()
        {
            LayoutGenerator layoutGen = FindObjectOfType<LayoutGenerator>();

            // SURELY THIS IS OK FOR A GAME JAM GAME RIGHT
            // if this game continues make L shape corridors pls
            retry:
            _layout = layoutGen.Generate(premadeRooms);
            if (!BuildLayout(_layout))
            {
                tilemap.ClearAllTiles();
                floorTilemap.ClearAllTiles();
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }

                goto retry;
            }

            PlayerCombat player = Instantiate(playerPrefab);
            Transform playerTransform = player.transform;
            playerTransform.position = _layout.LastBuild[_layout.Spawn].Item1.center;
            player.cam = cam;
            virtualCamera.Follow = playerTransform;
        }
    }
}
