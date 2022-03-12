using System.Globalization;
using DataStrucs;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

public static class GraphGen
{
    public static Graph Build(string txtname)
    {
        IEnumerable<string> input = System.IO.File.ReadLines(string.Format("Data\\{0}", txtname));
        var graph = new Graph();

        string current = null;
        bool first = true;
        foreach (string next in input)
        {
            if (first)
            {
                current = next;
                first = false;
                continue;
            }
            
            string[] vertices = current.Split();
            string[] edgeInfo = next.Split();
            int weight = (int)Math.Round(double.Parse(edgeInfo[0], CultureInfo.InvariantCulture));

            var edge = new Edge(new Vertex(int.Parse(vertices[0])), new Vertex(int.Parse(vertices[1])), weight);
            graph.AddVerticesAndEdge(edge);
            graph.AddVerticesAndEdge(new Edge(edge.v2, edge.v1, edge.weight));

            current = next;
        }
        return graph;
    }

    public static Graph BuildRandom(int n, int maxWeight, double p)
    {
        Random rnd = new Random();
        Graph G = new Graph(n);
        Edge e;
        for (int i = 0; i < n-1; i++)
        {
            e = new Edge(new Vertex(i), new Vertex(i+1), rnd.Next(maxWeight)+1);
            G.AddVerticesAndEdge(e);
            G.AddVerticesAndEdge(new Edge(e.v2, e.v1, e.weight));
        }
        for (int i = 0; i < n-1; i++)
        {
            for (int j = i+2; j < n; j++)
            {
                if (rnd.NextDouble() < p)
                {
                    e = new Edge(new Vertex(i), new Vertex(j), rnd.Next(maxWeight)+1);
                    G.AddVerticesAndEdge(e);
                    G.AddVerticesAndEdge(new Edge(e.v2, e.v1, e.weight));
                }
            }
        }
        
        
        return G;
    }

    public static async Task BuildRandomToTxt(int n, int maxWeight, double p, string txtname)
    {
        Random rnd = new Random();
        Graph G = new Graph(n);
        string filePath = "Data\\" + txtname.ToString();
        using (FileStream fs = File.Create(filePath)) {}
        using (StreamWriter file = File.AppendText(filePath))
        for (int i = 0; i < n-1; i++)
        {
            await file.WriteLineAsync(string.Format("{0} {1}\n{2}", i, i+1, rnd.Next(maxWeight)+1));
        }
        using (StreamWriter file = File.AppendText(filePath))
        for (int i = 0; i < n-1; i++)
        {
            for (int j = i+2; j < n; j++)
            {
                if (rnd.NextDouble() < p)
                    await file.WriteLineAsync(string.Format("{0} {1}\n{2}", i, j, rnd.Next(maxWeight)+1));
            }
        }
    }

    public static de.unikiel.npr.thorup.ds.graph.WeightedGraph ConvertToThorupGraph(Graph G)
    {
        var thorupGraph = new de.unikiel.npr.thorup.ds.graph.AdjacencyListWeightedDirectedGraph(G.Vertices.Count);
        foreach (Edge e in G.Edges)
        {
            thorupGraph.addEdge(new de.unikiel.npr.thorup.ds.graph.WeightedEdge(e.v1.id, e.v2.id, e.weight));
        }

        return thorupGraph;
    }
}
