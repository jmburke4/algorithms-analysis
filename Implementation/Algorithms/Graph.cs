namespace Implementation.Algorithms;

public class Graph<T>
{
    private readonly int nodeCount;

    private readonly List<T> nodes;

    private readonly int[,] edges;

    public Graph(int nodeCount, List<T> nodes)
    {
        this.nodeCount = nodeCount;

        if (nodes.Count > nodes.Distinct().Count())
            throw new ArgumentException("List of nodes must contain unique elements");

        this.nodes = nodes;
        this.edges = new int[nodeCount, nodeCount];
    }

    public void AddEdge(int from, int to, int weight)
    {
        if (from < 0 || from >= nodeCount || to < 0 || to >= nodeCount)
        {
            throw new ArgumentOutOfRangeException($"Node indices must be between 0 and {nodeCount - 1}");
        }

        edges[from, to] = weight;
    }

    public void AddEdge(T from, T to, int weight)
    {
        if (!nodes.Contains(from) || !nodes.Contains(to)) throw new ArgumentException("");
        edges[nodes.IndexOf(from), nodes.IndexOf(to)] = weight;
    }

    public int[,] Edges => edges;

    public T NodeAt(int i) => nodes.ElementAt(i);
}
