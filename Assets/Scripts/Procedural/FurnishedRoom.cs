using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Procedural
{
    public class FurnishedRoom : BlockOutRoom
    {
        public readonly List<(Vector3Int, int)> Doors;
        public readonly Dictionary<Vector3Int, TileBase> Tiles;
        public readonly Dictionary<Vector3Int, GameObject> Props;

        public FurnishedRoom(List<(Vector3Int, int)> doors, Dictionary<Vector3Int, TileBase> tiles, Dictionary<Vector3Int, GameObject> props, int width, int height) : base(width, height)
        {
            Doors = doors;
            Tiles = tiles;
            Props = props;
        }
    }
}
