using DataStrucs;
using System.Collections.Generic;

namespace Algorithms
{
    public static class FloydWarshall
    {
        
        public static int[][] dist; 
        public static Vertex[][] next;

        public static void Prepro(Graph G)
        {
            // Initalize distance and next arrays 
            dist = new int[G.Vertices.Count][];
            next = new Vertex[G.Vertices.Count][];
            for (int i = 0; i < G.Vertices.Count; i++)
            {
                dist[i] = new int[G.Vertices.Count];
                next[i] = new Vertex[G.Vertices.Count];
            }

            foreach (Edge e in G.Edges)
            {
                dist[e.v1.id][e.v2.id] = e.weight;
                dist[e.v2.id][e.v1.id] = e.weight;
                next[e.v1.id][e.v2.id] = e.v2;
                next[e.v2.id][e.v1.id] = e.v1;
            }
            foreach (Vertex v in G.Vertices)
            {
                dist[v.id][v.id] = 0;
                next[v.id][v.id] = v;
            }
            for (int k = 0; k < G.Vertices.Count; k++)
            {
                for (int i = 0; i < G.Vertices.Count; i++)
                {
                    for (int j = 0; j < G.Vertices.Count; j++)
                    {
                        if (dist[i][j] > dist[i][k] + dist[k][j])
                        {
                            dist[i][j] = dist[i][k] + dist[k][j];
                            next[i][j] = next[i][k];
                        }
                    }
                }
            }
        }

        public static int Dist(Vertex u, Vertex v)
        {
            return dist[u.id][v.id];
        }

        public static LinkedList<Vertex> Path(Vertex u, Vertex v)
        {
            LinkedList<Vertex> path = new LinkedList<Vertex>();
            if (next[u.id][v.id] == null) return path;
            path.AddLast(u);
            while (!u.Equals(v))
            {
                u = next[u.id][v.id];
                path.AddLast(u);
            }
            return path;
        }
    }
}