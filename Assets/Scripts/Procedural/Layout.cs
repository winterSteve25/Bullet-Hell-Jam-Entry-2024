using System;
using System.Collections.Generic;
using System.Linq;
using Procedural.Delaunay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Procedural
{
    public class Layout
    {
        public readonly List<RectInt> Rectangles = new List<RectInt>();
        public readonly List<RectInt> SacredRectangles = new List<RectInt>();
        public readonly List<(PremadeRoom, RectInt)> PremadeRooms = new List<(PremadeRoom, RectInt)>();
        public readonly RectInt Whole;

        private int _spawn;
        private int _numVertSplit;
        private List<Edge> _connections;
        private List<RectInt> _build;

        public List<RectInt> LastBuild => _build;
        public List<Edge> Connection => _connections;
        public int Spawn => _spawn;

        public Layout(int width, int height)
        {
            Whole = new RectInt(0, 0, width, height);
            Rectangles.Add(Whole);
        }

        public void SliceRandomRect(int minLength)
        {
            if (Rectangles.Count <= 0)
            {
                return;
            }

            int index = Random.Range(0, Rectangles.Count);
            SliceRect(index, minLength);
        }

        private void SliceRect(int index, int minLength)
        {
            RectInt rect = Rectangles[index];
            Rectangles.RemoveAt(index);
            RectInt newRect;

            if (rect.width < minLength * 2)
            {
                int currentHeight = rect.height;
                int newHeight = Random.Range(minLength, rect.height - minLength);
                newRect = new RectInt(rect.xMin, rect.yMin + (currentHeight - newHeight), rect.width, newHeight);
                rect.height = currentHeight - newHeight;
            }
            else if (rect.height < minLength * 2)
            {
                int currentWidth = rect.width;
                int newWidth = Random.Range(minLength, rect.width - minLength);
                newRect = new RectInt(rect.xMin + (currentWidth - newWidth), rect.yMin, newWidth, rect.height);
                rect.width = currentWidth - newWidth;
            }
            else if (Random.Range(0, 1f) < ProbabilityToVert())
            {
                _numVertSplit++;
                int currentWidth = rect.width;
                int newWidth = Random.Range(minLength, rect.width - minLength);
                newRect = new RectInt(rect.xMin + (currentWidth - newWidth), rect.yMin, newWidth, rect.height);
                rect.width = currentWidth - newWidth;
            }
            else
            {
                _numVertSplit--;
                int currentHeight = rect.height;
                int newHeight = Random.Range(minLength, rect.height - minLength);
                newRect = new RectInt(rect.xMin, rect.yMin + (currentHeight - newHeight), rect.width, newHeight);
                rect.height = currentHeight - newHeight;
            }

            if (CanBeSliced(newRect, minLength))
            {
                Rectangles.Add(newRect);
            }
            else
            {
                SacredRectangles.Add(newRect);
            }

            if (CanBeSliced(rect, minLength))
            {
                Rectangles.Add(rect);
            }
            else
            {
                SacredRectangles.Add(rect);
            }
        }

        private float ProbabilityToVert()
        {
            return Mathf.Pow(2, -(_numVertSplit + 1));
        }

        private static bool CanBeSliced(RectInt rect, int minLength)
        {
            return rect.width >= minLength * 2 || rect.height >= minLength * 2;
        }

        public List<RectInt> Build(int minGap, int gap, float removeChance, List<PremadeRoom> premadeRooms)
        {
            PremadeRooms.Clear();
            List<RectInt> rooms = new List<RectInt>();
            Vector2Int? lastRemovedPos = null;

            foreach (var rect in SacredRectangles.Concat(Rectangles))
            {
                if ((lastRemovedPos == null || (rect.position - lastRemovedPos.Value).sqrMagnitude > Whole.width / 3) && Random.Range(0, 1f) < removeChance)
                {
                    lastRemovedPos = rect.position;
                    continue;
                }

                RectInt r = rect;
                int wMod = Random.Range(minGap, Mathf.Min(gap, r.width));
                int hMod = Random.Range(minGap, Mathf.Min(gap, r.height));

                r.width -= wMod;
                r.height -= hMod;
                r.x -= Random.Range(0, wMod / 2);
                r.y -= Random.Range(0, hMod / 2);

                rooms.Add(r);
            }

            _spawn = Random.Range(0, rooms.Count);
            Direction spawnCorner = RectIsClosestToCorner(rooms, _spawn);

            foreach (var premadeRoom in premadeRooms)
            {
                Direction spawnRoomAt = premadeRoom.FromSpawn switch
                {
                    Distance.Far => spawnCorner.Opposite(),
                    Distance.Mid => Random.Range(0, 1f) > 0.5f ? spawnCorner.Adjacent() : spawnCorner.Adjacent().Opposite(),
                    Distance.Near => spawnCorner,
                    _ => throw new ArgumentOutOfRangeException()
                };

                Vector2Int spawnRoomAtPos = Whole.size * spawnRoomAt.ToCornerVectorOffset();
                switch (spawnRoomAt)
                {
                    case Direction.Up:
                        spawnRoomAtPos.x -= premadeRoom.Width + Random.Range(minGap, gap);
                        spawnRoomAtPos.y -= Random.Range(premadeRoom.Height, Whole.height - premadeRoom.Height);
                        break;
                    case Direction.Right:
                        spawnRoomAtPos.x += Random.Range(minGap, gap);
                        spawnRoomAtPos.y -= Random.Range(premadeRoom.Height, Whole.height - premadeRoom.Height);
                        break;
                    case Direction.Left:
                        spawnRoomAtPos.x -= premadeRoom.Width + Random.Range(minGap, gap);
                        spawnRoomAtPos.y += Random.Range(0, Whole.height - premadeRoom.Height);
                        break;
                    case Direction.Down:
                        spawnRoomAtPos.x += Random.Range(minGap, gap);
                        spawnRoomAtPos.y += Random.Range(0, Whole.height - premadeRoom.Height);
                        break;
                }

                RectInt rectInt = new RectInt(spawnRoomAtPos.x, spawnRoomAtPos.y, premadeRoom.Width, premadeRoom.Height);
                PremadeRooms.Add((premadeRoom, rectInt));
                rooms.Add(rectInt);
            }

            List<Edge> edges = new List<Edge>();
            List<Triangle2D> delaunays = new List<Triangle2D>();
            List<Vector2> centers = new List<Vector2>();
            Dictionary<Vector2Int, int> nodes = new Dictionary<Vector2Int, int>();

            for (int i = 0; i < rooms.Count; i++)
            {
                RectInt r = rooms[i];
                centers.Add(r.center);
                Vector2Int c = Cast(r.center);
                nodes.Add(c, i);
            }

            DelaunayTriangulation triangulation = new DelaunayTriangulation();
            triangulation.Triangulate(centers);
            triangulation.GetTrianglesDiscardingHoles(delaunays);

            foreach (var tri in delaunays)
            {
                edges.Add(new Edge(nodes[Cast(tri.p0)], nodes[Cast(tri.p1)], (int)(tri.p1 - tri.p0).magnitude));
                edges.Add(new Edge(nodes[Cast(tri.p1)], nodes[Cast(tri.p2)], (int)(tri.p2 - tri.p1).magnitude));
                edges.Add(new Edge(nodes[Cast(tri.p2)], nodes[Cast(tri.p0)], (int)(tri.p0 - tri.p2).magnitude));
            }

            edges = MinimumSpanningTree.KruskalMST(edges, rooms.Count);

            _build = rooms;
            _connections = edges;

            return rooms;
        }

        private Direction RectIsClosestToCorner(List<RectInt> rooms, int i)
        {
            if (rooms == null)
            {
                throw new ArgumentException("Layout has not been built");
            }

            RectInt r = rooms[i];
            bool x = IsCloserToFirst(r.x, Whole.x, Whole.width);
            bool y = IsCloserToFirst(r.y, Whole.y, Whole.height);

            return (x, y) switch
            {
                (true, true) => Direction.Left,
                (false, true) => Direction.Down,
                (true, false) => Direction.Up,
                (false, false) => Direction.Right
            };
        }

        private static bool IsCloserToFirst(int number, int option1, int option2)
        {
            int distanceToOption1 = Mathf.Abs(number - option1);
            int distanceToOption2 = Mathf.Abs(number - option2);
            return distanceToOption1 < distanceToOption2;
        }

        private static Vector2Int Cast(Vector2 pt)
        {
            return new Vector2Int(Mathf.FloorToInt((pt.x + 10) / 20), Mathf.FloorToInt((pt.y + 10) / 20));
        }
    }
}
