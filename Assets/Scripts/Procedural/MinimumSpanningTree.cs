using System.Collections.Generic;
using System.Linq;

namespace Procedural
{
    public class MinimumSpanningTree
    {
        private static int FindParent(int[] parent, int vertex)
        {
            if (parent[vertex] == -1)
                return vertex;
            return FindParent(parent, parent[vertex]);
        }

        private static void Union(int[] parent, int x, int y)
        {
            int xSet = FindParent(parent, x);
            int ySet = FindParent(parent, y);
            parent[xSet] = ySet;
        }

        public static List<Edge> KruskalMST(List<Edge> edges, int numberOfVertices)
        {
            List<Edge> minimumSpanningTree = new List<Edge>();
            edges = edges.OrderBy(edge => edge.Weight).ToList();
            int[] parent = new int[numberOfVertices];
            for (int i = 0; i < numberOfVertices; i++)
                parent[i] = -1;
            int edgeCount = 0;
            int index = 0;
            while (edgeCount < numberOfVertices - 1)
            {
                Edge nextEdge = edges[index++];
                int x = FindParent(parent, nextEdge.Source);
                int y = FindParent(parent, nextEdge.Destination);
                if (x != y)
                {
                    minimumSpanningTree.Add(nextEdge);
                    Union(parent, x, y);
                    edgeCount++;
                }
            }
            return minimumSpanningTree;
        }
    }
}
