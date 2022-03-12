namespace DataStrucs
{
    public class Edge
    {
        public Vertex v1;
        public Vertex v2;
        public int weight;

        public Edge(Vertex v1, Vertex v2)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.weight = 0;
        }

        public Edge(Vertex v1, Vertex v2, int weight)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.weight = weight;
        }
        public override string ToString()
        {
            return string.Format("{0}->{1}", this.v1, this.v2);
        }
    }
}