using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class RandomUtils
    {
        public static int RangeExcludeMany(int min, int maxExc, params int[] exclude)
        {
            List<int> availableValues = new List<int>();
            for (int i = min; i < maxExc; i++)
            {
                if (System.Array.IndexOf(exclude, i) == -1)
                {
                    availableValues.Add(i);
                }
            }

            return availableValues[Random.Range(0, availableValues.Count)];
        }

        public static Mesh GenerateQuadMesh(Vector3 bottomLeftCorner, float w, float h)
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
    }
}
