using System.Collections.Generic;
using DataStrucs;
using de.unikiel.npr.thorup.algs;
using de.unikiel.npr.thorup.ds;
using System;

namespace Algorithms
{
    public class MyThorup : ISSSP
    {
        private Thorup thorupAlg;
        private Graph graph;
        public void Prepro(Graph G)
        {
            graph = G;
            var thorupGraph = GraphGen.ConvertToThorupGraph(G);
            thorupAlg = new Thorup();
            thorupAlg.constructMinimumSpanningTree(thorupGraph,
                            new Kruskal(new UnionFindStructureTarjan()));
            thorupAlg.constructOtherDataStructures
                        (new UnionFindStructureTarjan(),
                        new SplitFindminStructureGabow(thorupGraph.getNumberOfVertices()));
        }

        public LinkedList<Vertex> Run(Vertex s)
        {
            int[] d = thorupAlg.findShortestPaths(s.id);
            int[] p = thorupAlg.getPredecessors();
            LinkedList<Vertex> S = new LinkedList<Vertex>();
            foreach (Vertex v in graph.Vertices)
            {
                v.d = d[v.id];
                if (v.Equals(s))
                    v.p = null;
                else
                    v.p = graph.GetVertex(p[v.id]);
                S.AddLast(v);
            }

            return S;
        }

        public LinkedList<Vertex> RunModified(Vertex s, int[][] dist, int i)
        {
            int[] d = thorupAlg.findShortestPathsModified(s.id, dist, i);
            int[] p = thorupAlg.getPredecessors();
            LinkedList<Vertex> S = new LinkedList<Vertex>();
            foreach (Vertex v in graph.Vertices)
            {
                v.d = d[v.id];
                if (v.Equals(s))
                {
                    v.p = null;
                    S.AddLast(v.Copy());
                }
                else if (v.d != int.MaxValue)
                {   
                        v.p = graph.GetVertex(p[v.id]);
                        S.AddLast(v.Copy());
                }
            }
            thorupAlg.cleanUpBetweenQueries(new SplitFindminStructureGabow(graph.Vertices.Count));
            return S;
        }
    }
}