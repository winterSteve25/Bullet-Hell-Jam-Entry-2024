using UnityEngine;
using UnityEngine.Tilemaps;

namespace Procedural.RoomGeneration
{
    public class RoomGen : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase tileBase;
        [SerializeField] private int minLength;
        [SerializeField] private int maxLength;
        [SerializeField] private int minHeight;
        [SerializeField] private int maxHeight;

        private bool[] _doorBool = new bool[4];

        private void Start()
        {
            // + 1 b/c Range is exclusive on the top
            Generate(Random.Range(minLength, maxLength + 1), Random.Range(minHeight, maxHeight + 1));
        }

        private void Generate(int length, int height)
        {
            Vector3Int startPos = tilemap.WorldToCell(new Vector3(-length / 2, height / 2, 0) + transform.position);

            for (int j = startPos.y - 1; j > startPos.y - height; j--)
            {
                for (int i = startPos.x; i < startPos.x + length; i++)
                {
                    if (j == startPos.y - 1
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

            int doorsCount = Random.Range(1, 4);
            int k = 0;
            while (k < doorsCount)
            {
                int ranDir = Random.Range(1, 4);
                if (_doorBool[ranDir]) continue;
                _doorBool[ranDir] = true;
                Vector3Int[] corners = RoomManager.CornersPos(startPos, length, height);
                AddDoors(RoomManager.RandomPlacement(ranDir, corners), DirectionExt.FromIndex(ranDir));
                k++;
            }
        }

        private void AddDoors(Vector3Int startPos, Direction direction)
        {
            Debug.Log(startPos);
            for (int i = 0; i < 3; i++)
            {
                tilemap.SetTile(startPos, null);
                Vector3Int test = RoomManager.IsLength(direction.ToUnitVector3Int())
                    ? new Vector3Int(0, -1, 0)
                    : new Vector3Int(1, 0, 0);
                startPos += test;
            }
        }
    }
}