
using DataStrucs;
using System.Collections.Generic;

namespace Algorithms
{
    public static class Utils
    {
        /// <summary>
        /// Get most significant bit
        /// </summary>
        public static int GetMSB(int w)
        {
            int r = 0;
            while ((w >>= 1) != 0) 
            {
                r++;
            }
            return r;
        }

        public static LinkedList<Edge> SortEdges(Graph G)
        {
            LinkedList<Edge>[] buckets = new LinkedList<Edge>[GetMSB(G.MaxWeight)+1];

            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new LinkedList<Edge>();
            }

            foreach (Edge e in G.Edges)
            {
                buckets[GetMSB(e.weight)].AddLast(e);              
            }

            
            LinkedList<Edge> sortedEdges = new LinkedList<Edge>();
            for (int i = 0; i < buckets.Length; i++)
            {
                foreach (Edge e in buckets[i])
                {
                    sortedEdges.AddLast(e);
                }
            }

            return sortedEdges;
        }
    }
}