using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Procedural
{
    public class RoomGenerator : MonoBehaviour
    {
        [SerializeField] private TileBase wallUp;
        [SerializeField] private TileBase wallSide;
        [SerializeField] private TileBase wallDown;
        [SerializeField] private TileBase floor;

        public FurnishedRoom Create(int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            return Create(Random.Range(minWidth, maxWidth + 1), Random.Range(minHeight, maxHeight + 1));
        }

        public FurnishedRoom Create(int width, int height)
        {
            return null;
        }
    }
}
