namespace DataStrucs
{

    public struct MarkVertex
    {
        public Vertex Vertex;
        public bool Marked;
        public MarkVertex(Vertex Vertex, bool Marked)
        {
            this.Vertex = Vertex;
            this.Marked = Marked;
        }
    }
}