using DataStrucs;

namespace Algorithms
{
    public static class MyKruskal 
    {


        public static Graph Run(Graph G)
        {
            var sortedEdges = Utils.SortEdges(G);
            var newGraph = new Graph(G.Vertices);
            var DisjointSetForrest = new DisjointSets(G.Vertices.Count);
            foreach (Vertex v in G.Vertices)
            {
                DisjointSetForrest.MakeSet(v);
            }
            foreach (Edge e in sortedEdges)
            {
                if (!DisjointSetForrest.Find(e.v1).Equals(DisjointSetForrest.Find(e.v2)))
                {
                    newGraph.AddEdge(e);
                    DisjointSetForrest.Union(e.v1, e.v2);
                }
            }

            return newGraph;
        }
    }
}