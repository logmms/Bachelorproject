using System.Collections.Generic;
using System;

namespace DataStrucs 
{
    public class Graph
    {
        private MyTuple[] adj;

        public LinkedList<Vertex> Vertices { get; private set; }

        public LinkedList<Edge> Edges { get; private set; }
        public int MaxWeight = 0;




        /// <summary>
        /// Constructs Graph
        /// </summary>
        /// <param name="vertices">Array of vertices with ids 0...vertices.Length-1</param>
        /// <param name="edges">Array of edges with ids 0...vertices.Length-1</param>
        public Graph(LinkedList<Vertex> vertices, LinkedList<Edge> edges)
        {

            this.adj = new MyTuple[vertices.Count];
            int i = 0;
            foreach (Vertex v in vertices)
            {
                adj[i] = new MyTuple(v, new LinkedList<Edge>());
                i++;
            }

            this.Vertices = vertices;
            this.Edges = new LinkedList<Edge>();


            foreach (Edge e in edges)
            {
                this.AddEdge(e);
                this.MaxWeight = e.weight > this.MaxWeight ? e.weight : this.MaxWeight;
            }
        }

        public Graph(int n)
        {
            this.adj = new MyTuple[n];
            for (int i = 0; i < n; i++)
            {
                adj[i] = new MyTuple(null, new LinkedList<Edge>());
            }

            this.Vertices = new LinkedList<Vertex>();
            this.Edges = new LinkedList<Edge>();
        }
        public Graph(LinkedList<Vertex> vertices)
        {
            this.adj = new MyTuple[vertices.Count];
            int i = 0;
            foreach (Vertex v in vertices)
            {
                adj[i] = new MyTuple(v, new LinkedList<Edge>());
                i++;
            }

            this.Vertices = vertices;
            this.Edges = new LinkedList<Edge>();
        }

        public Graph()
        {
            this.adj = new MyTuple[0];
            this.Vertices = new LinkedList<Vertex>();
            this.Edges = new LinkedList<Edge>();
        }

        private T[] resizeArr<T>(T[] arr, int n)
        {
            T[] newArr = new T[n];
            arr.CopyTo(newArr, 0);
            
            return newArr;
        }

        private MyTuple[] resizeAdjLst(MyTuple[] adj, int n)
        {
            var oldLength = adj.Length;
            var newAdj = resizeArr(adj, n);
            for (int i = oldLength; i < n; i++)
            {
                newAdj[i] = new MyTuple(null, new LinkedList<Edge>());
            }

            return newAdj;
        }


        public void AddVertex(Vertex v) 
        {
            if (v.id >= this.adj.Length)
            {
                this.adj = resizeAdjLst(this.adj, v.id+1);
            }
            this.adj[v.id].V = v;
            Vertices.AddLast(v);
        }

        public void AddEdge(Edge e)
        {
            var n = this.adj.Length;
            if (e.v1.id >= n || e.v2.id >= n)
            {
                throw new ArgumentException("One of the edges' endpoints is not in the graph.");
            }

            this.MaxWeight = e.weight > this.MaxWeight ? e.weight : this.MaxWeight;
            e.v1 = GetVertex(e.v1.id);
            e.v2 = GetVertex(e.v2.id);
            this.Edges.AddLast(e);
            this.adj[e.v1.id].Edges.AddLast(e);
        }

        public void AddVerticesAndEdge(Edge e)
        {
            var n = this.adj.Length;
            if (e.v1.id >= n || e.v2.id >= n)
            {
                n = (e.v1.id > e.v2.id ? e.v1.id+1 : e.v2.id+1);
                this.adj = resizeAdjLst(this.adj, n);
            }

            
            if (this.adj[e.v1.id].V == null)
            {
                this.adj[e.v1.id].V = e.v1;
                this.Vertices.AddLast(e.v1);
            }
            else
                e.v1 = GetVertex(e.v1.id);
            if (this.adj[e.v2.id].V == null)
            {    
                this.adj[e.v2.id].V = e.v2;
                this.Vertices.AddLast(e.v2);
            }
            else
                e.v2 = GetVertex(e.v2.id);

            this.MaxWeight = e.weight > this.MaxWeight ? e.weight : this.MaxWeight;
            this.Edges.AddLast(e);
            this.adj[e.v1.id].Edges.AddLast(e);
        }

        public LinkedList<Edge> Adj(Vertex v)
        {
            return this.adj[v.id].Edges;
        }

        public Vertex GetVertex(int id) {
            if (id >= adj.Length || adj[id].V == null)
                throw new ArgumentException("This vertex does not exist.");
            
            return adj[id].V;
        }

        public Graph Copy()
        {
            return new Graph(new LinkedList<Vertex>(this.Vertices), new LinkedList<Edge>(this.Edges));;
        }
    }
}