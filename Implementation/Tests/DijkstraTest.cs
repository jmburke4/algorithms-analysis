using Implementation.Algorithms;
using Implementation.Heaps;

namespace Implementation;

public static class DijkstraTest
{
    public static void Run()
    {
        Console.WriteLine("=== Dijkstra Tests ===");
        var testNumber = 0;
        var passed = 0;
        var failed = 0;

        RunBasicScenario(ref testNumber, ref passed, ref failed);
        RunWorstCaseLinearChain(ref testNumber, ref passed, ref failed);
        RunBestCaseSingleEdge(ref testNumber, ref passed, ref failed);
        RunComplexMultiPathGraph(ref testNumber, ref passed, ref failed);
        RunDenseGraphWithCycles(ref testNumber, ref passed, ref failed);
        RunLargeGraphLayered(ref testNumber, ref passed, ref failed);

        Console.WriteLine();
        Console.WriteLine($"Results: {passed} passed, {failed} failed.");
        Console.WriteLine(failed == 0 ? "All tests passed." : "Some tests failed.");
        Console.WriteLine();
    }

    private static void Pass(ref int testNumber, string description, ref int passed)
    {
        MetricsHandler.EndTest();
        testNumber++;
        Console.WriteLine($"Test {testNumber} - {description} - Passed");
        MetricsHandler.PrintMetrics();
        passed++;
        MetricsHandler.StartTest();
    }

    private static void Fail(ref int testNumber, string description, string reason, ref int failed)
    {
        testNumber++;
        Console.WriteLine($"Test {testNumber} - {description} - FAILED: {reason}");
        failed++;
        MetricsHandler.StartTest();
    }

    private static void RunBasicScenario(ref int testNumber, ref int passed, ref int failed)
    {
        MetricsHandler.StartTest();
        var nodes = new[] { "A", "B", "C", "D" };
        var g = new Graph<string>(nodes);

        g.AddEdge("A", "B", 1);
        g.AddEdge("B", "C", 2);
        g.AddEdge("A", "D", 4);
        g.AddEdge("D", "C", 1);

        var heap = new FibonacciHeap<Dijkstra.KeyedVertex>();

        try
        {
            var (distance, previous) = Dijkstra.ShortestPathsFrom(g, "A", heap);
            var dist = distance;
            var prev = previous;

            // Check distance to C == 3
            if (!dist.TryGetValue("C", out long value))
            {
                Fail(ref testNumber, "Distance map contains 'C'", "missing key 'C'", ref failed);
                return;
            }

            var distC = Convert.ToDouble(value);
            if (Math.Abs(distC - 3.0) > 1e-9)
            {
                Fail(ref testNumber, "Shortest distance A->C is 3", $"expected 3, got {distC}", ref failed);
                return;
            }
            Pass(ref testNumber, "Shortest distance A->C is 3", ref passed);

            // Reconstruct path A -> ... -> C
            var path = Dijkstra.ReconstructPath(prev, "A", "C");
            var pathStr = string.Join(" -> ", path);
            if (pathStr != "A -> B -> C")
            {
                Fail(ref testNumber, "Reconstructed path A->C is A -> B -> C", $"got '{pathStr}'", ref failed);
                return;
            }
            Pass(ref testNumber, "Reconstructed path A->C is A -> B -> C", ref passed);
        }
        catch (Exception ex)
        {
            Fail(ref testNumber, "Dijkstra run completes without exception", ex.GetType().Name + ": " + ex.Message, ref failed);
        }
    }

    private static void RunWorstCaseLinearChain(ref int testNumber, ref int passed, ref int failed)
    {
        // Worst case: Linear chain A->B->C->D->E->F where algorithm must relax all edges
        MetricsHandler.StartTest();
        
        var nodes = new[] { "A", "B", "C", "D", "E", "F" };
        var g = new Graph<string>(nodes);

        g.AddEdge("A", "B", 1);
        g.AddEdge("B", "C", 1);
        g.AddEdge("C", "D", 1);
        g.AddEdge("D", "E", 1);
        g.AddEdge("E", "F", 1);

        var heap = new FibonacciHeap<Dijkstra.KeyedVertex>();

        try
        {
            var (distance, previous) = Dijkstra.ShortestPathsFrom(g, "A", heap);

            // Check distance to F == 5 (path A->B->C->D->E->F)
            if (!distance.TryGetValue("F", out long value) || value != 5)
            {
                Fail(ref testNumber, "Linear chain: Distance A->F is 5", $"expected 5, got {value}", ref failed);
                return;
            }
            Pass(ref testNumber, "Linear chain: Distance A->F is 5", ref passed);

            // Verify path reconstruction
            var path = Dijkstra.ReconstructPath(previous, "A", "F");
            var pathStr = string.Join(" -> ", path);
            if (pathStr != "A -> B -> C -> D -> E -> F")
            {
                Fail(ref testNumber, "Linear chain: Path A->F is A -> B -> C -> D -> E -> F", $"got '{pathStr}'", ref failed);
                return;
            }
            Pass(ref testNumber, "Linear chain: Path reconstruction correct", ref passed);
        }
        catch (Exception ex)
        {
            Fail(ref testNumber, "Linear chain: Dijkstra completes without exception", ex.GetType().Name + ": " + ex.Message, ref failed);
        }
    }

    private static void RunBestCaseSingleEdge(ref int testNumber, ref int passed, ref int failed)
    {
        // Best case: Direct edge from source to all other nodes
        MetricsHandler.StartTest();
        
        var nodes = new[] { "S", "A", "B", "C" };
        var g = new Graph<string>(nodes);

        g.AddEdge("S", "A", 10);
        g.AddEdge("S", "B", 20);
        g.AddEdge("S", "C", 15);

        var heap = new FibonacciHeap<Dijkstra.KeyedVertex>();

        try
        {
            var (distance, previous) = Dijkstra.ShortestPathsFrom(g, "S", heap);

            // Check all direct distances
            if (!distance.TryGetValue("A", out long distA) || distA != 10)
            {
                Fail(ref testNumber, "Direct edges: Distance S->A is 10", $"expected 10, got {distA}", ref failed);
                return;
            }
            Pass(ref testNumber, "Direct edges: Distance S->A is 10", ref passed);

            if (!distance.TryGetValue("C", out long distC) || distC != 15)
            {
                Fail(ref testNumber, "Direct edges: Distance S->C is 15", $"expected 15, got {distC}", ref failed);
                return;
            }
            Pass(ref testNumber, "Direct edges: Distance S->C is 15", ref passed);
        }
        catch (Exception ex)
        {
            Fail(ref testNumber, "Direct edges: Dijkstra completes without exception", ex.GetType().Name + ": " + ex.Message, ref failed);
        }
    }

    private static void RunComplexMultiPathGraph(ref int testNumber, ref int passed, ref int failed)
    {
        // Intermediate case: Multiple paths to destination, requires choosing optimal
        //        1     3
        //    A ---> B ---> D
        //    |             ^
        //  2 |             | 1
        //    v             |
        //    C ----------->+
        //         4
        MetricsHandler.StartTest();

        var nodes = new[] { "A", "B", "C", "D" };
        var g = new Graph<string>(nodes);

        g.AddEdge("A", "B", 1);
        g.AddEdge("A", "C", 2);
        g.AddEdge("B", "D", 3);
        g.AddEdge("C", "D", 4);

        var heap = new FibonacciHeap<Dijkstra.KeyedVertex>();

        try
        {
            var (distance, previous) = Dijkstra.ShortestPathsFrom(g, "A", heap);

            // Shortest path to D is A->B->D with cost 4
            if (!distance.TryGetValue("D", out long distD) || distD != 4)
            {
                Fail(ref testNumber, "Multi-path: Distance A->D is 4", $"expected 4, got {distD}", ref failed);
                return;
            }
            Pass(ref testNumber, "Multi-path: Distance A->D is 4", ref passed);

            // Verify optimal path chosen
            var path = Dijkstra.ReconstructPath(previous, "A", "D");
            var pathStr = string.Join(" -> ", path);
            if (pathStr != "A -> B -> D")
            {
                Fail(ref testNumber, "Multi-path: Optimal path is A -> B -> D", $"got '{pathStr}'", ref failed);
                return;
            }
            Pass(ref testNumber, "Multi-path: Optimal path chosen correctly", ref passed);

            // Verify distance to C
            if (!distance.TryGetValue("C", out long distC) || distC != 2)
            {
                Fail(ref testNumber, "Multi-path: Distance A->C is 2", $"expected 2, got {distC}", ref failed);
                return;
            }
            Pass(ref testNumber, "Multi-path: Distance A->C calculated correctly", ref passed);
        }
        catch (Exception ex)
        {
            Fail(ref testNumber, "Multi-path: Dijkstra completes without exception", ex.GetType().Name + ": " + ex.Message, ref failed);
        }
    }

    private static void RunDenseGraphWithCycles(ref int testNumber, ref int passed, ref int failed)
    {
        // Intermediate case: Dense graph with cycles and multiple paths
        //     2       5
        //    A --- B --- C
        //    |\    |    /|
        //  1 | 4  3|  2 | 1
        //    |  \  |  /  |
        //    D---+-E----+
        //        6  4

        MetricsHandler.StartTest();

        var nodes = new[] { "A", "B", "C", "D", "E" };
        var g = new Graph<string>(nodes);

        g.AddEdge("A", "B", 2);
        g.AddEdge("A", "D", 1);
        g.AddEdge("A", "E", 4);
        g.AddEdge("B", "C", 5);
        g.AddEdge("B", "E", 3);
        g.AddEdge("C", "E", 2);
        g.AddEdge("D", "E", 6);
        g.AddEdge("E", "C", 4);
        g.AddEdge("D", "A", 7); // Creates cycle

        var heap = new FibonacciHeap<Dijkstra.KeyedVertex>();

        try
        {
            var (distance, previous) = Dijkstra.ShortestPathsFrom(g, "A", heap);

            // Shortest path to C: A->E->C with cost 4+4=8 (not A->B->C with cost 2+5=7, so A->B->C is better)
            // Actually: A->B->C = 2+5 = 7, A->E->C = 4+4 = 8, so A->B->C wins
            if (!distance.TryGetValue("C", out long distC) || distC != 7)
            {
                Fail(ref testNumber, "Dense graph: Distance A->C is 7", $"expected 7, got {distC}", ref failed);
                return;
            }
            Pass(ref testNumber, "Dense graph: Distance A->C is 7", ref passed);

            // Check distance to E
            if (!distance.TryGetValue("E", out long distE) || distE != 4)
            {
                Fail(ref testNumber, "Dense graph: Distance A->E is 4", $"expected 4, got {distE}", ref failed);
                return;
            }
            Pass(ref testNumber, "Dense graph: Distance A->E is 4", ref passed);

            // Check distance to D (should be 1 - direct edge)
            if (!distance.TryGetValue("D", out long distD) || distD != 1)
            {
                Fail(ref testNumber, "Dense graph: Distance A->D is 1", $"expected 1, got {distD}", ref failed);
                return;
            }
            Pass(ref testNumber, "Dense graph: Distance A->D is 1", ref passed);
        }
        catch (Exception ex)
        {
            Fail(ref testNumber, "Dense graph: Dijkstra completes without exception", ex.GetType().Name + ": " + ex.Message, ref failed);
        }
    }

    private static void RunLargeGraphLayered(ref int testNumber, ref int passed, ref int failed)
    {
        // Large graph with 25 nodes (5 layers of 5 nodes each)
        // Layer structure: Each layer fully connects to next layer
        // Layer 0: A0, A1, A2, A3, A4
        // Layer 1: B0, B1, B2, B3, B4
        // Layer 2: C0, C1, C2, C3, C4
        // Layer 3: D0, D1, D2, D3, D4
        // Layer 4: E0, E1, E2, E3, E4
        
        MetricsHandler.StartTest();

        var nodes = new string[25];
        var nodeIdx = 0;
        var layers = new[] { "A", "B", "C", "D", "E" };
        for (int l = 0; l < 5; l++)
        {
            for (int n = 0; n < 5; n++)
            {
                nodes[nodeIdx++] = $"{layers[l]}{n}";
            }
        }

        var g = new Graph<string>(nodes);

        // Connect layers: each node in layer i connects to all nodes in layer i+1
        // Weight increases based on position difference to create multiple paths
        for (int layer = 0; layer < 4; layer++)
        {
            for (int from = 0; from < 5; from++)
            {
                var fromNode = $"{layers[layer]}{from}";
                for (int to = 0; to < 5; to++)
                {
                    var toNode = $"{layers[layer + 1]}{to}";
                    int weight = Math.Abs(from - to) + 1; // Weight 1-5 based on position difference
                    g.AddEdge(fromNode, toNode, weight);
                }
            }
        }

        var heap = new FibonacciHeap<Dijkstra.KeyedVertex>();

        try
        {
            var (distance, previous) = Dijkstra.ShortestPathsFrom(g, "A0", heap);

            // Optimal path from A0 to E0: follow diagonal (same position) = 1+1+1+1 = 4
            if (!distance.TryGetValue("E0", out long distE0) || distE0 != 4)
            {
                Fail(ref testNumber, "Large graph (25 nodes): Distance A0->E0 is 4", $"expected 4, got {distE0}", ref failed);
                return;
            }
            Pass(ref testNumber, "Large graph (25 nodes): Distance A0->E0 is 4", ref passed);

            // Path from A0 to E4: diagonal + 4 steps right = 1+2+3+4+5 = 15 (or other paths)
            // Actually: A0->B0(1)->C0(1)->D0(1)->E0(1)->E4(4) = 8 or
            //           A0->B4(5)->C4(1)->D4(1)->E4(1) = 8 or
            //           various middle paths. Let's check a reachable node.
            if (!distance.TryGetValue("B0", out long distB0) || distB0 != 1)
            {
                Fail(ref testNumber, "Large graph (25 nodes): Distance A0->B0 is 1", $"expected 1, got {distB0}", ref failed);
                return;
            }
            Pass(ref testNumber, "Large graph (25 nodes): Distance A0->B0 is 1", ref passed);

            // Check intermediate layer nodes
            if (!distance.TryGetValue("C0", out long distC0) || distC0 != 2)
            {
                Fail(ref testNumber, "Large graph (25 nodes): Distance A0->C0 is 2", $"expected 2, got {distC0}", ref failed);
                return;
            }
            Pass(ref testNumber, "Large graph (25 nodes): Distance A0->C0 is 2", ref passed);

            // Check a corner node (A0 to E4)
            if (!distance.TryGetValue("E4", out long distE4))
            {
                Fail(ref testNumber, "Large graph (25 nodes): Distance to E4 exists", $"missing key 'E4'", ref failed);
                return;
            }
            Pass(ref testNumber, "Large graph (25 nodes): Distance A0->E4 is reachable", ref passed);

            // Verify path reconstruction works for a distant node
            var path = Dijkstra.ReconstructPath(previous, "A0", "D0");
            if (path.Count < 2)
            {
                Fail(ref testNumber, "Large graph (25 nodes): Path reconstruction returns path to D0", $"got empty or single-node path", ref failed);
                return;
            }
            Pass(ref testNumber, "Large graph (25 nodes): Path reconstruction works for distant nodes", ref passed);
        }
        catch (Exception ex)
        {
            Fail(ref testNumber, "Large graph (25 nodes): Dijkstra completes without exception", ex.GetType().Name + ": " + ex.Message, ref failed);
        }
    }
}
