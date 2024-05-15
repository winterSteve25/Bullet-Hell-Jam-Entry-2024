using System;
using UnityEngine;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using NaughtyAttributes;
#endif

namespace Procedural
{
    public class LayoutGenerator : MonoBehaviour
    {
        [SerializeField] private int tunnelLength;
        [SerializeField] private int minRoomLength;
        [SerializeField] private int maxRoomLength;
        [SerializeField] private int minRoomNum;
        [SerializeField] private int maxRoomNum;

        public Layout Generate(BlockOutRoom startingRoom)
        {
            Layout layout = new Layout();
            layout.AddRoom(Vector3Int.zero, startingRoom.Width, startingRoom.Height);
            int numOfRooms = Random.Range(minRoomNum, maxRoomNum);
            Direction previousDirection = DirectionExt.Random(null);
            Vector3Int previousPosition = GetPositionOnWall(previousDirection, Vector3Int.zero, startingRoom.Width, startingRoom.Height);
            int counter = 0;

            while (counter < numOfRooms)
            {
                (Vector3Int pos, BlockOutRoom room) = Generate(layout, previousDirection, previousPosition);
                if (room == null)
                {
                    previousDirection = DirectionExt.Random(previousDirection);
                    continue;
                }
                previousDirection = DirectionExt.Random(null);
                previousPosition = GetPositionOnWall(previousDirection, pos, room.Width, room.Height);
                counter++;
            }

            return layout;
        }

        private (Vector3Int, BlockOutRoom) Generate(Layout layout, Direction direction, Vector3Int position)
        {
            Vector3Int newOrigin = position + direction.GetCellOffset() * tunnelLength;

            int width = minRoomLength;
            int height = minRoomLength;

            switch (direction)
            {
                case Direction.Down:
                    newOrigin.y -= height;
                    newOrigin.x -= Random.Range(1, width);
                    break;
                case Direction.Left:
                    newOrigin.x -= width;
                    newOrigin.y -= Random.Range(1, height);
                    break;
                case Direction.Right:
                    newOrigin.y -= Random.Range(1, height);
                    break;
                case Direction.Up:
                    newOrigin.x -= Random.Range(1, width);
                    break;
            }

            if (layout.IsColliding(newOrigin, width, height))
            {
                return (Vector3Int.zero, null);
            }

            while (true)
            {
                Direction growthDirection = DirectionExt.Random(direction.Opposite());
                Vector3Int growthOffset = growthDirection.GetCellOffset();

                int newWidth = width + Mathf.Abs(growthOffset.x);
                int newHeight = height + Mathf.Abs(growthOffset.y);

                if (newWidth > maxRoomLength || newHeight > maxRoomLength)
                {
                    break;
                }

                switch (growthDirection)
                {
                    case Direction.Left:
                    case Direction.Down:
                        newOrigin += growthOffset;
                        break;
                }

                // 10% chance to just stop growing the room
                if (Random.Range(0, 1f) < 0.10f)
                {
                    break;
                }

                if (layout.IsColliding(newOrigin, newWidth, newHeight))
                {
                    break;
                }

                width = newWidth;
                height = newHeight;
            }

            layout.AddRoom(newOrigin, width, height);
            return (newOrigin, new BlockOutRoom(width, height));
        }

        private Vector3Int GetPositionOnWall(Direction direction, Vector3Int origin, int width, int height)
        {
            switch (direction)
            {
                case Direction.Up:
                    int x = Random.Range(1, width);
                    origin.x += x;
                    origin.y += height;
                    return origin;
                case Direction.Down:
                    int x1 = Random.Range(1, width);
                    origin.x += x1;
                    return origin;
                case Direction.Left:
                    int y = Random.Range(1, height);
                    origin.y += y;
                    return origin;
                case Direction.Right:
                    int y1 = Random.Range(1, height);
                    origin.x += width;
                    origin.y += y1;
                    return origin;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        #region Debug
        #if UNITY_EDITOR
        private Layout _test;

        private void OnDrawGizmos()
        {
            if (_test == null) return;
            Gizmos.color = Color.red;
            bool a = true;
            foreach (var (pos, width, height) in _test.Rooms)
            {
                Gizmos.DrawSphere(pos, 0.5f);
                Mesh mesh = GenerateQuadMesh(pos, width, height);
                Gizmos.DrawWireMesh(mesh);

                if (a)
                {
                    Gizmos.color = Color.cyan;
                    a = false;
                }
            }
        }

        [Button]
        private void TestGenerate()
        {
            _test = Generate(BlockOutRoom.Default());
        }

        public Mesh GenerateQuadMesh(Vector3 bottomLeftCorner, float width, float height)
        {
            // Create a new mesh
            Mesh mesh = new Mesh();

            // Define vertices
            Vector3[] vertices = new Vector3[4];
            vertices[0] = bottomLeftCorner; // Bottom-left
            vertices[1] = bottomLeftCorner + new Vector3(width, 0, 0); // Bottom-right
            vertices[2] = bottomLeftCorner + new Vector3(0, height, 0); // Top-left
            vertices[3] = bottomLeftCorner + new Vector3(width, height, 0); // Top-right

            // Define triangles
            int[] triangles = new int[]
            {
                0, 2, 1, 1, 2, 3
            };

            // Define UVs
            Vector2[] uvs = new Vector2[4];
            uvs[0] = new Vector2(0, 0);
            uvs[1] = new Vector2(1, 0);
            uvs[2] = new Vector2(0, 1);
            uvs[3] = new Vector2(1, 1);

            // Assign vertices, triangles, and UVs to the mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;

            // Recalculate normals for correct shading
            mesh.RecalculateNormals();

            return mesh;
        }
        #endif
        #endregion
    }
}
