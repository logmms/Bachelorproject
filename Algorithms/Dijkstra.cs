using QuikGraph;
using FibonacciHeap;
using DataStrucs;
using System.Collections.Generic;

namespace Algorithms
{
    public class Dijkstra : ISSSP
    {

        private Graph graph;

        private FibonacciHeap<int, int>init_fib_heap() 
        {
            var heap = new FibonacciHeap<int, int>(0);
            foreach (Vertex v in graph.Vertices)
            {
                heap.Insert(v.FibNode);
            }

            return heap;
        }
        private void init_single_src(Vertex s) 
        {
            foreach (Vertex v in graph.Vertices)
            {
                if (v.Equals(s)) 
                    v.d = 0;
                else 
                    v.d = int.MaxValue;
                v.p = null;
            }
        }

        private void relax(Vertex u, Vertex v, int w, FibonacciHeap<int, int> heap) 
        {
            if (v.d > u.d + w)
            {
                heap.DecreaseKey(v.FibNode, u.d + w);
                v.p = u;
            }
        }

        public void Prepro(Graph G)
        {
            this.graph = G;
        }

        public LinkedList<Vertex> Run(Vertex s) 
        {
            init_single_src(s);
            FibonacciHeap<int, int> Q = init_fib_heap();
            LinkedList<Vertex> S = new LinkedList<Vertex>();
            while (!Q.IsEmpty())
            {
                Vertex u = graph.GetVertex(Q.RemoveMin().Data);
                if (u.d == int.MaxValue) return S;
                S.AddLast(u);
                foreach (var edge in graph.Adj(u))
                {
                    relax(u, edge.v2, edge.weight, Q);                
                }
            }

            return S;
        }

        public LinkedList<Vertex> RunModified(Vertex s, int[][] dist, int i) 
        {
            init_single_src(s);
            FibonacciHeap<int, int> Q = init_fib_heap();
            LinkedList<Vertex> S = new LinkedList<Vertex>();
            while (!Q.IsEmpty())
            {
                Vertex u = graph.GetVertex(Q.RemoveMin().Data).Copy();
                if (u.d == int.MaxValue) return S;
                S.AddLast(u);
                foreach (var edge in graph.Adj(u))
                {
                    if (u.d + edge.weight < dist[i+1][edge.v2.id])
                        relax(u, edge.v2, edge.weight, Q);
                }
            }

            return S;
        }
    }
}