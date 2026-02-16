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
    }

    public static (int[] parent, int totalWeight)
            MinimumSpanningTree<T>(Graph<T> graph, IHeap<KeyedVertex> heap, T start) {

        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(heap);

        int nodeCount = graph.Count;
        int src = graph.IndexOf(start);

        int[] key = new int[nodeCount];
        int[] parent = new int[nodeCount];
        bool[] inMst = new bool[nodeCount];

        const int INF = int.MaxValue;

        for (int i = 0; i < nodeCount; i++) {
            key[i] = int.MaxValue;
            parent[i] = -1;
            inMst[i] = false;
        }

        key[src] = 0;

        var heapNodes = new Node<KeyedVertex>[nodeCount];

        for (int i = 0; i < nodeCount; i++) {
            headNodes[i] = heap.Insert(new KeyedVertex(key[i], i));
        }

        int totWeight = 0;

        while (!heap.IsEmpty()){
           
            var minNode = heap.ExtractMin();
            int u = minNode.Vertex;

            if (inMst[u])
               continue;

            inMst[u] = true;
            totWeight += minNode.Weight;

            foreach (var (v, w) in graph.GetNeighbors(u)){
                if (!inMst[v] && w<key[v]){
                        key[v] = w;
                        parent[v] = u;
                        heap.DecreaseKey(heapNodes[v], new KeyedVertex(w, v));
                    }
                }
            }
            return (parent, totWeight);
    }
}
