using UnityEngine;

#if UNITY_EDITOR
using System.Collections.Generic;
using NaughtyAttributes;
#endif

namespace Procedural
{
    public class LayoutGenerator : MonoBehaviour
    {
        [SerializeField] private int minRoomLength;
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private int sliceCount;

        public Layout Generate()
        {
            Layout layout = new Layout(width, height);

            for (int i = 0; i < sliceCount; i++)
            {
                layout.SliceRandomRect(minRoomLength);
            }

            return layout;
        }

        #region Debug
        #if UNITY_EDITOR
        private List<RectInt> _test;

        private void OnDrawGizmos()
        {
            if (_test == null) return;
            Gizmos.color = Color.cyan;

            foreach (RectInt rect in _test)
            {
                Vector3 pos = new Vector3(rect.xMin, rect.yMin);
                Gizmos.DrawSphere(pos, 0.5f);
                Mesh mesh = GenerateQuadMesh(pos, rect.width, rect.height);
                Gizmos.DrawWireMesh(mesh);
            }
        }

        [Button]
        private void TestGenerate()
        {
            _test = Generate().Complete();
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
