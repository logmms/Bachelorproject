using System.Collections.Generic;

namespace DataStrucs
{
    public class MyTuple
    {
        public Vertex V;

        public LinkedList<Edge> Edges;
        

        public MyTuple(Vertex v, LinkedList<Edge> edges)
        {
            this.V = v;
            this.Edges = edges;
        }
    }
}