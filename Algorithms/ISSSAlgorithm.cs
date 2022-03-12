using DataStrucs;
using System.Collections.Generic;

namespace Algorithms
{
    public interface ISSSP
    {
        void Prepro(Graph G);

        LinkedList<Vertex> Run(Vertex s);

        LinkedList<Vertex> RunModified(Vertex s, int[][] dist, int i);
    }
}