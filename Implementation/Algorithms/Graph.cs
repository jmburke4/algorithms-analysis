namespace Implementation.Algorithms;

public class Graph<T>
{
    private readonly List<T> nodes;
    private readonly Dictionary<T, int> indexByNode;

    private readonly List<List<(int v, int w)>> adjacency;

    public Graph(IEnumerable<T> nodes)
    {
        ArgumentNullException.ThrowIfNull(nodes);

        this.nodes = [.. nodes];
        if (this.nodes.Count == 0)
            throw new ArgumentException("Graph must contain at least one node.", nameof(nodes));

        if (this.nodes.Count != this.nodes.Distinct().Count())
            throw new ArgumentException("List of nodes must contain unique elements.", nameof(nodes));

        indexByNode = new Dictionary<T, int>(this.nodes.Count);
        for (int i = 0; i < this.nodes.Count; i++)
        {
            indexByNode[this.nodes[i]] = i;
        }

        adjacency = Enumerable.Range(0, this.nodes.Count).Select(_ => new List<(int v, int w)>()).ToList();
    }

    public int Count => nodes.Count;
    public IReadOnlyList<T> Nodes => nodes;

    public int IndexOf(T node)
    {
        if (!indexByNode.TryGetValue(node, out var idx))
            throw new ArgumentException("Node does not exist in the graph.", nameof(node));
        return idx;
    }

    public T NodeAt(int i)
    {
        if (i < 0 || i >= nodes.Count) throw new ArgumentOutOfRangeException(nameof(i));
        return nodes[i];
    }

    /// <summary>
    /// Adds a directed edge u -> v with a non-negative weight.
    /// </summary>
    public void AddEdge(T from, T to, int weight)
    {
        int u = IndexOf(from);
        int v = IndexOf(to);
        AddEdge(u, v, weight);
    }

    /// <summary>
    /// Adds a directed edge u -> v with a non-negative weight.
    /// </summary>
    public void AddEdge(int from, int to, int weight)
    {
        if (from < 0 || from >= Count) throw new ArgumentOutOfRangeException(nameof(from));
        if (to < 0 || to >= Count) throw new ArgumentOutOfRangeException(nameof(to));
        if (weight < 0)
            throw new ArgumentOutOfRangeException(nameof(weight), "Dijkstra's algorithm requires non-negative weights.");

        // If you want to store only the smallest weight for duplicate edges:
        var list = adjacency[from];
        int idx = list.FindIndex(e => e.v == to);
        if (idx >= 0)
        {
            if (weight < list[idx].w) list[idx] = (to, weight);
        }
        else
        {
            list.Add((to, weight));
        }
    }

    /// <summary>
    /// Adds an undirected edge by adding u->v and v->u.
    /// </summary>
    public void AddUndirectedEdge(T a, T b, int weight)
    {
        AddEdge(a, b, weight);
        AddEdge(b, a, weight);
    }

    public IEnumerable<(int neighbor, int weight)> GetNeighbors(int u)
    {
        if (u < 0 || u >= Count) throw new ArgumentOutOfRangeException(nameof(u));
        return adjacency[u];
    }

    public IEnumerable<(T neighbor, int weight)> GetNeighbors(T node)
    {
        int u = IndexOf(node);
        return adjacency[u].Select(e => (nodes[e.v], e.w));
    }

    public bool RemoveEdge(T from, T to)
    {
        int u = IndexOf(from);
        int v = IndexOf(to);
        var list = adjacency[u];
        int idx = list.FindIndex(e => e.v == v);
        if (idx >= 0) { list.RemoveAt(idx); return true; }
        return false;
    }

    public bool TryGetWeight(T from, T to, out int weight)
    {
        int u = IndexOf(from);
        int v = IndexOf(to);
        foreach (var e in adjacency[u])
        {
            if (e.v == v) { weight = e.w; return true; }
        }
        weight = default;
        return false;
    }

    /// <summary>
    /// Heap element for a vertex: (distance from source, vertex index).
    /// Comparable by distance (then by index as a stable tiebreaker).
    /// </summary>
    public readonly struct KeyedVertex : IComparable<KeyedVertex>
    {
        public KeyedVertex(long distance, int vertex)
        {
            Distance = distance;
            Vertex = vertex;
        }

        public long Distance { get; }
        public int Vertex { get; }

        public int Weight => (int)Distance; // For Prim's algorithm where we use KeyedVertex to store edge weights

        public int CompareTo(KeyedVertex other)
        {
            int c = Distance.CompareTo(other.Distance);
            return c != 0 ? c : Vertex.CompareTo(other.Vertex);
        }

        public override string ToString() => $"(v={Vertex}, d={Distance})";
    }
}

