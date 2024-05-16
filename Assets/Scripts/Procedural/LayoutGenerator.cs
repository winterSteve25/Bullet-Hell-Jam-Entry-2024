using System;
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
        [SerializeField] private int minGap;
        [SerializeField] private int gap;
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private int sliceCount;
        [SerializeField] private float removeChance;

        [SerializeField] private List<PremadeRoom> pmRooms;

        public Layout Generate(List<PremadeRoom> premadeRooms)
        {
            Layout layout = new Layout(width, height);

            for (int i = 0; i < sliceCount; i++)
            {
                layout.SliceRandomRect(minRoomLength + minGap);
            }

            retry:
            try
            {
                layout.Build(minGap, gap, removeChance, premadeRooms);
            }
            catch (Exception)
            {
                // dont flame me its actually optimal
                goto retry;
            }

            return layout;
        }

        #region Debug
        #if UNITY_EDITOR
        private List<RectInt> _rooms;
        private List<Edge> _connections;

        private void OnDrawGizmos()
        {
            if (_rooms == null) return;
            Gizmos.color = Color.cyan;

            foreach (RectInt rect in _rooms)
            {
                Vector3 pos = new Vector3(rect.xMin, rect.yMin);
                Gizmos.DrawSphere(pos, 0.5f);
                Mesh mesh = GenerateQuadMesh(pos, rect.width, rect.height);
                Gizmos.DrawWireMesh(mesh);
            }

            if (_connections == null) return;
            Gizmos.color = Color.red;

            foreach (var connection in _connections)
            {
                Gizmos.DrawLine(_rooms[connection.Source].center, _rooms[connection.Destination].center);
            }
        }

        [Button]
        private void TestGenerate()
        {
            Layout layout = Generate(pmRooms);
            _rooms = layout.LastBuild;
            _connections = layout.Connection;
        }

        private static Mesh GenerateQuadMesh(Vector3 bottomLeftCorner, float w, float h)
        {
            // Create a new mesh
            Mesh mesh = new Mesh();

            // Define vertices
            Vector3[] vertices = new Vector3[4];
            vertices[0] = bottomLeftCorner; // Bottom-left
            vertices[1] = bottomLeftCorner + new Vector3(w, 0, 0); // Bottom-right
            vertices[2] = bottomLeftCorner + new Vector3(0, h, 0); // Top-left
            vertices[3] = bottomLeftCorner + new Vector3(w, h, 0); // Top-right

            // Define triangles
            int[] triangles =
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
