using FibonacciHeap;
using System;

namespace DataStrucs
{
    public class Vertex : IComparable, IEquatable<Vertex>
    {

        public FibonacciHeapNode<int, int> FibNode;

        public int id
        {
            get { return FibNode.Data; }
            set 
            {
                FibNode = new FibonacciHeapNode<int, int>(value, this.d);
            }
        }

        public int d
        {
            get { return FibNode.Key; }
            set 
            { 
                FibNode = new FibonacciHeapNode<int, int>(this.id, value);
            }
        }

        public Vertex p;


        public Vertex(int id) 
        {
            this.FibNode = new FibonacciHeapNode<int, int>(id, int.MaxValue);
        }

        public Vertex Copy()
        {
            Vertex vertexCopy = new Vertex(this.id);
            vertexCopy.d = this.d;
            vertexCopy.p = this.p;

            return vertexCopy;
        }

        public override string ToString()
        {
            return string.Format("{0}", this.id);
        }

        public int CompareTo(object obj) 
        {
            if (obj == null) return 1;

            Vertex otherVertex = obj as Vertex;
            if (otherVertex!= null)
                if (this.id < otherVertex.id) return -1;
                else if (this.id == otherVertex.id) return 0;
                else return 1;
            else
                throw new ArgumentException("Object is not a Vertex.");
        }

        public bool Equals(Vertex other)
        {
            if (other == null) return false;
            return this.id == other.id;
        }

        public override int GetHashCode()
        {
            return this.id;
        }
    }
}