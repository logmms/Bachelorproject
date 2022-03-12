using Algorithms;
using DataStrucs;
using System;

class TestClass
{
    static void Main(string[] args)
    {
        int n = 100000;
        int maxWeight = 100000;
        double p = 0.001;
        string txtname = string.Format("n{0}weight{1}p{2}.txt", n, maxWeight, p);
        // await GraphGen.BuildRandomToTxt(n, maxWeight, p, txtname);

        //var G = GraphGen.Build(txtname);
        var G = GraphGen.BuildRandom(100, 1000, 0.2);
        var s = new Vertex(0);
        var t = new Vertex(50);

        // Run Dijkstra
        ISSSP dijsktra = new Dijkstra();
        dijsktra.Prepro(G);
        dijsktra.Run(s);
        Console.WriteLine(G.GetVertex(t.id).d);

        // Run ThorupZwick
        ThorupZwick thorupZwick = new ThorupZwick(new MyThorup());
        thorupZwick.Prepro(G, 2);
        Console.WriteLine(thorupZwick.Dist(s, t));
    }
}
