using Implementation.Heaps;

namespace Implementation.Algorithms;

/// <summary>
/// Dijkstra's shortest path algorithm using an external heap that supports decrease-key.
/// Works with the provided Graph<T> (adjacency list) and IHeap<T> interfaces.
/// Assumes all edge weights are non-negative (enforced by Graph.AddEdge). 
/// </summary>
public static class Dijkstra
{
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

        public int CompareTo(KeyedVertex other)
        {
            int c = Distance.CompareTo(other.Distance);
            return c != 0 ? c : Vertex.CompareTo(other.Vertex);
        }

        public override string ToString() => $"(v={Vertex}, d={Distance})";
    }

    /// <summary>
    /// Compute single-source shortest paths from <paramref name="source"/> to all nodes.
    /// Returns a distance map and a predecessor map (for path reconstruction).
    /// </summary>
    /// <typeparam name="T">Node label type</typeparam>
    /// <param name="graph">Graph instance</param>
    /// <param name="source">Source node label</param>
    /// <param name="heap">
    /// A min-heap instance (empty) whose element type is KeyedVertex and supports decrease-key.
    /// </param>
    /// <returns>(distanceByNode, previousByNode)</returns>
    public static (Dictionary<T, long> distance, Dictionary<T, T?> previous)
        ShortestPathsFrom<T>(Graph<T> graph, T source, IHeap<KeyedVertex> heap)
    {
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(heap);

        int n = graph.Count;
        int src = graph.IndexOf(source);

        var dist = new long[n];
        var prev = new int?[n];

        const long INF = long.MaxValue / 4;    // "infinite" distance
        for (int i = 0; i < n; i++) { dist[i] = INF; prev[i] = null; }
        dist[src] = 0;

        // Keep a heap node handle per vertex so we can call DecreaseKey efficiently.
        var heapNodes = new Node<KeyedVertex>[n];

        // Build the heap with all vertices (classic Dijkstra variant with a mutable key).
        for (int i = 0; i < n; i++)
        {
            heapNodes[i] = heap.Insert(new KeyedVertex(dist[i], i));  // Insert returns Node<T>
        }

        // Extract exactly n times (we inserted exactly n items).
        for (int extracted = 0; extracted < n; extracted++)
        {
            var u = heap.ExtractMin();   // next vertex with minimum tentative distance
            if (u.Distance >= INF) break;
            
            // Relax all outgoing edges (u -> v with weight w)
            foreach (var (v, w) in graph.GetNeighbors(u.Vertex))       // neighbor enumeration
            {
                long alt = dist[u.Vertex] + w; // use long to avoid overflow on accumulation
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u.Vertex;

                    // DecreaseKey to reflect shorter distance for v
                    heap.DecreaseKey(heapNodes[v], new KeyedVertex(alt, v));  // decrease-key operation
                }
            }
        }

        // Convert arrays back to label-based dictionaries
        var distanceByNode = new Dictionary<T, long>(n);
        var previousByNode = new Dictionary<T, T?>(n);
        for (int i = 0; i < n; i++)
        {
            distanceByNode[graph.NodeAt(i)] = dist[i];                        // index -> label
            previousByNode[graph.NodeAt(i)] = prev[i] is int pi ? graph.NodeAt(pi) : default;  // predecessor label
        }

        return (distanceByNode, previousByNode);
    }

    /// <summary>
    /// Reconstruct a path from source to target using the 'previous' map produced by ShortestPathsFrom.
    /// Returns an empty list if target is unreachable.
    /// </summary>
    public static List<T> ReconstructPath<T>(Dictionary<T, T?> previous, T source, T target)
    {
        var path = new List<T>();
        if (!previous.ContainsKey(target)) return path;

        var current = target;
        var seen = new HashSet<T>(); // cycle guard

        while (!EqualityComparer<T>.Default.Equals(current, source))
        {
            if (!previous.TryGetValue(current, out var p) || p is null) return []; // unreachable
            if (!seen.Add(current)) return []; // safety break

            path.Add(current);
            current = p;
        }

        path.Add(source);
        path.Reverse();
        return path;
    }
}
