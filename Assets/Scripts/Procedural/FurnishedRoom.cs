using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Procedural
{
    public class FurnishedRoom
    {
        public readonly List<(Vector3Int, int)> Doors;
        public readonly Dictionary<Vector3Int, TileBase> Tiles;
        public readonly Dictionary<Vector3Int, GameObject> Props;
        public readonly int Width;
        public readonly int Height;

        public FurnishedRoom(List<(Vector3Int, int)> doors, Dictionary<Vector3Int, TileBase> tiles, Dictionary<Vector3Int, GameObject> props, int width, int height)
        {
            Doors = doors;
            Tiles = tiles;
            Props = props;
            Width = width;
            Height = height;
        }
    }
}
