using Implementation.Heaps;

namespace Implementation.Algorithms;
public static class Prims {
    public readonly struct KeyedVertex : IComparable<KeyedVertex> {

        public KeyedVertex(int weight, int vertex) {

            Weight = weight;
            Vertex = vertex;
        }
        public int Weight { get; }
        public int Vertex { get; }

        public int CompareTo(KeyedVertex other) {

            int c = Weight.CompareTo(other.Weight);
            return c != 0 ? c : Vertex.CompareTo(other.Vertex);
        }

        public override string ToString() => $"(v={Vertex}, w={Weight})";
    }

    public static (int[] parent, int totalWeight)
            MinimumSpanningTree<T>(Graph<T> graph, IHeap<KeyedVertex> heap, T start) {

        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(heap);

        int nodeCount = graph.Count;
        int src = graph.IndexOf(start);

        int[] minEdgeWeight = new int[nodeCount];
        int[] parent = new int[nodeCount];
        bool[] inMst = new bool[nodeCount];

        for (int i = 0; i < nodeCount; i++) {
            minEdgeWeight[i] = int.MaxValue;
            parent[i] = -1;
        }

        minEdgeWeight[src] = 0;

        var heapNodes = new Node<KeyedVertex>[nodeCount];

        for (int i = 0; i < nodeCount; i++) {
            heapNodes[i] = heap.Insert(new KeyedVertex(minEdgeWeight[i], i));
        }

        int totalWeight = 0;
        int added = 0;

        while (added < nodeCount)
        {
                var minNode = heap.ExtractMin();
                int u = minNode.Vertex;

                if (inMst[u])
                    continue;

                inMst[u] = true;
                totalWeight += minNode.Weight;
                added++;

                var neighbors = graph.GetNeighbors(u).ToList();
                for (int i = 0; i < neighbors.Count; i++)
                {

                    int v = neighbors[i].neighbor;
                    int w = neighbors[i].weight;

                    if (!inMst[v] && w < minEdgeWeight[v])
                    {
                        minEdgeWeight[v] = w;
                        parent[v] = u;
                        heap.DecreaseKey(heapNodes[v], new KeyedVertex(w, v));
                    }
                }
            }
            return (parent, totalWeight);
    }
}
