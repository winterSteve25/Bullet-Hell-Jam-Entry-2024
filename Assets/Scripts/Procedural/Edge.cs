namespace Procedural
{
    public class Edge
    {
        public readonly int Source;
        public readonly int Destination;
        public readonly int Weight;

        public Edge(int source, int destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }
    }
}
