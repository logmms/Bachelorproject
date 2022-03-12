using DataStrucs;
using System.Collections.Generic;
using System;

namespace Algorithms
{

    
    public class ThorupZwick
    {

        private HashTable<Vertex, Vertex>[] bunches;
        private LinkedList<KeyValPair<Vertex, Vertex>>[] bunchArray;
        private Vertex[][] p;
        private Vertex[] exceptArray;
        private int[][] distance;
        public ISSSP SSPAlgorithm;

        public ThorupZwick(ISSSP SSPAlgorithm)
        {
            this.SSPAlgorithm = SSPAlgorithm;
        }

        /// <summary>
        /// Genereats the lists A_0, ..., A_k
        /// </summary>
        /// <param name="g">The graph.</param>
        /// <param name="k">The index of the last list generated.</param>
        /// <param name="n">Number of vertices in the graph</param>
        /// <returns>An array with all the lists A_0, ..., A_k</returns>
        private LinkedList<Vertex>[] generateAs(Graph G, int k)
        {
            
            // Initialize A_i lists
            LinkedList<Vertex>[] A = new LinkedList<Vertex>[k+1];
            A[0] = new LinkedList<Vertex>(G.Vertices);
            A[k] = new LinkedList<Vertex>();

            double n = G.Vertices.Count;
            double prop = 1d/Math.Pow(n, 1d/(double)k);
            Random rand = new Random();
            for (int i = 1; i < k; i++)
            {
                A[i] = new LinkedList<Vertex>();
                foreach (Vertex v in A[i-1])
                {
                    if (rand.NextDouble() <= prop)
                        A[i].AddLast(v);
                }

            }
            if (A[k-1].Count == 0) return generateAs(G, k);

            return A;
        }


        /// <summary>
        /// Gets the witnesses p_i(v) s.t. delta(A_i, v) = delta(p_i(v), v) for all v in V
        /// </summary>
        /// <param name="S">The vertices returned by Dijkstra's algorithm</param>
        /// <param name="s">The source algorithm applied with Dijkstra's algirthm</param>
        /// <returns>The array of witness p_i</returns>
        private Vertex[] getWitnesses(LinkedList<Vertex> S, Vertex s, int size)
        {
            
            // Create witness array
            Vertex[] p_i = new Vertex[size];
            
            LinkedList<Vertex> verticesOnPath; 
            foreach (Vertex v in S)
            {
                if (v.Equals(s)) continue;
                else if (v.p.Equals(s))
                {
                    p_i[v.id] = v;
                    continue;
                }    
                verticesOnPath = new LinkedList<Vertex>();

                while (!v.p.p.Equals(s)) 
                {
                    verticesOnPath.AddLast(v.p);
                    v.p = v.p.p;
                }
                // Update pointers
                foreach (Vertex vertexOnPath in verticesOnPath)
                {
                    vertexOnPath.p = v.p;
                }
                p_i[v.id] = v.p;

            }

            return p_i;

        }

        private LinkedList<Vertex> except(LinkedList<Vertex> a1, LinkedList<Vertex>a2)
        {
            LinkedList<Vertex> retval = new LinkedList<Vertex>();

            foreach (Vertex v in a2)
            {
                exceptArray[v.id] = v;
            }
            foreach (Vertex v in a1)
            {
              if (exceptArray[v.id] == null)
                retval.AddLast(v);  
            }
            
            return retval;
        }

        public void Prepro(Graph G, int k)
        {
            // Generate A lists
            LinkedList<Vertex>[] A = generateAs(G, k);

            // Create distance array
            int n = G.Vertices.Count;
            distance = new int[k+1][];

            // Create array for except set operation
            exceptArray = new Vertex[n];
            

            for (int i = 0; i < k+1; i++)
            {
                distance[i] = new int[n+k];
            }

            // Initialize dist(A_k, v) = infty for all v in V.
            for (int i = 0; i < n+k; i++)
            {
                distance[k][i] = int.MaxValue;
            }

            // Initalize witness array
            p = new Vertex [k+1][];

            // Initalize cluster array
            LinkedList<Vertex>[] C = new LinkedList<Vertex>[n];

            // Copy graph
            Graph copyGraph = G.Copy();

            for (int i = k-1; i >= 0; i--)
            {
                // Add source vertex S
                Vertex s = new Vertex(copyGraph.Vertices.Count);
                copyGraph.AddVertex(s);

                // Add edges (s, w) for all w in A_i
                foreach (Vertex w in A[i])
                {
                    copyGraph.AddEdge(new Edge(s, w, 1));
                    copyGraph.AddEdge(new Edge(w, s, 1));
                }

                // Run single source shortest path algorithm.
                SSPAlgorithm.Prepro(copyGraph);
                var S = SSPAlgorithm.Run(s);

                // Get distances
                foreach (Vertex v in S)
                {
                        distance[i][v.id] = v.Equals(s) ? v.d : v.d-1;
                }

                // Get witnesses p_i(v) for all v in V
                p[i] = getWitnesses(S, s, n+k);
                
                foreach (Vertex v in copyGraph.Vertices)
                {
                    if (distance[i][v.id] == distance[i+1][v.id])
                        p[i][v.id] = p[i+1][v.id];
                }

                // Generate clusters
                SSPAlgorithm.Prepro(G);
                foreach (Vertex w in except(A[i], A[i+1]))
                {
                    C[w.id] = SSPAlgorithm.RunModified(w, distance, i);
                }
            }

            // Create clusters
            bunches = new HashTable<Vertex, Vertex>[G.Vertices.Count];
            bunchArray = new LinkedList<KeyValPair<Vertex, Vertex>>[G.Vertices.Count];
            for (int i = 0; i < G.Vertices.Count; i++)
            {
                bunchArray[i] = new LinkedList<KeyValPair<Vertex, Vertex>>();
            }
            foreach (Vertex w in G.Vertices)
            {
                foreach (Vertex v in C[w.id])
                {
                    bunchArray[v.id].AddLast(new KeyValPair<Vertex, Vertex>(w, v));
                }
            }
            foreach (Vertex v in G.Vertices)
            {
                bunches[v.id] = new HashTable<Vertex, Vertex>(bunchArray[v.id]);
            }
        }


        public int Dist(Vertex u, Vertex v)
        {
            Vertex w = u;
            int i = 0;
            Vertex new_u = u;
            Vertex new_v = v;
            Vertex tmp;
            while (!bunches[new_v.id].ContainsKey(w))
            {
                i++;
                tmp = new_u;
                new_u = new_v;
                new_v = tmp;
                w = p[i][new_u.id];
            }
            return distance[i][new_u.id] + bunches[new_v.id].GetValue(w).d;
        }

        public LinkedList<Vertex> Path(Vertex u, Vertex v)
        {
            
            Vertex w = u;
            int i = 0;
            Vertex new_u = u;
            Vertex new_v = v;
            Vertex tmp;
            while (!bunches[new_v.id].ContainsKey(w))
            {
                i++;
                tmp = new_u;
                new_u = new_v;
                new_v = tmp;
                w = p[i][new_u.id];
            }

            // Initalialize the list of vertices on the path from u to the 
            // least common ancestor (LCA) of u and v
            LinkedList<Vertex> path1 = new LinkedList<Vertex>();

            // Initalialize the list of vertices on the path from v to the LCA of u and v
            LinkedList<Vertex> path2 = new LinkedList<Vertex>();

            // If v.d is set to visited it indicates that the notes has been visited
            int visited = -1;

            // The id for the LCA of u and v
            int LCA;

            // Find LCA
            LinkedList<Tuple<Vertex, int>> modifiedVertices = new LinkedList<Tuple<Vertex, int>>();
            new_u = bunches[u.id].GetValue(w);
            new_v = bunches[v.id].GetValue(w);
            while (true)
            {
                if (new_u != null)
                {
                    if (new_u.d == visited)
                    {
                        path1.AddLast(new_u);
                        LCA = new_u.id;
                        break;
                    }
                    modifiedVertices.AddLast(Tuple.Create(new_u, new_u.d));
                    new_u.d = visited;
                    path1.AddLast(new_u);
                    new_u = new_u.p;
                }
                if (new_v != null)
                {
                    if (new_v.d == visited)
                    {
                        path2.AddLast(new_v);
                        LCA = new_v.id;
                        break;
                    }
                    modifiedVertices.AddLast(Tuple.Create(new_v, new_v.d));
                    new_v.d = visited;
                    path2.AddLast(new_v);
                    new_v = new_v.p;

                }
            }

            // Restore vertex distances
            foreach (var tuple in modifiedVertices)
            {
                tuple.Item1.d = tuple.Item2;
            }

            // Initalize list of vertices on the path from u to v
            LinkedList<Vertex> path = new LinkedList<Vertex>();

            // Construct shortest path from u to v
            var nodeOnPath1 = path1.First;
            while (nodeOnPath1.Value.id != LCA)
            {
                path.AddLast(nodeOnPath1.Value);
                nodeOnPath1 = nodeOnPath1.Next;
            }

            var nodeOnPath2 = path2.Last;
            while (nodeOnPath2 != null)
            {
                path.AddLast(nodeOnPath2.Value);
                nodeOnPath2 = nodeOnPath2.Previous;
            }
            return path;
        }

    }
}