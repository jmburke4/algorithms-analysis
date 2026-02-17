using Implementation.Algorithms;
using Implementation.Heaps;

namespace Implementation;

public static class PrimsTest
{
    public static void Run()
    {
        Console.WriteLine("=== Prims Tests ===");
        var testNumber = 0;
        var passed = 0;
        var failed = 0;

        RunPrimScenario<FibonacciHeap<Prims.KeyedVertex>>(ref testNumber, ref passed, ref failed, "FibonacciHeap");

        RunPrimScenario<PairingHeap<Prims.KeyedVertex>>(ref testNumber, ref passed, ref failed, "PairingHeap");

        Console.WriteLine();
        Console.WriteLine($"Results: {passed} passed, {failed} failed.");
        Console.WriteLine(failed == 0 ? "All tests passed." : "Some tests failed.");
        Console.WriteLine();
    }

    private static void Pass(ref int testNumber, string description, ref int passed)
    {
        testNumber++;
        Console.WriteLine($"Test {testNumber} - {description} - Passed");
        passed++;
    }

    private static void Fail(ref int testNumber, string description, string reason, ref int failed)
    {
        testNumber++;
        Console.WriteLine($"Test {testNumber} - {description} - FAILED: {reason}");
        failed++;
    }

    private static void RunPrimScenario<THeap>(ref int testNumber, ref int passed, ref int failed, string heapName)
        where THeap : IHeap<Prims.KeyedVertex>, new()
    {
        var nodes = new[] { "A", "B", "C", "D" };
        var g = new Graph<string>(nodes);

        g.AddUndirectedEdge("A", "B", 4);
        g.AddUndirectedEdge("B", "C", 3);
        g.AddUndirectedEdge("A", "D", 2);
        g.AddUndirectedEdge("D", "C", 1);

        var heap = new THeap();

        var (parent, totWeight) = Prims.MinimumSpanningTree(g, heap, "A");

        //Test 1 - Total MST weight
        if (totWeight != 6)
        {
            Fail(ref testNumber, $"Prim MST total weight using {heapName}", $"expected 6, got {totWeight}", ref failed);
            return;
        }
        else {
            Pass(ref testNumber, $"Prim MST total weight using {heapName}", ref passed);
        }

        //Test 2 - Structure Check
        int edgeCount = 0;
        for (int i = 0; i < parent.Length; i++)
        {
            if (parent[i] != -1)
                edgeCount++;
        }

        if(edgeCount != nodes.Length - 1)
        {
            Fail(ref testNumber, $"Prim MST edge count using {heapName}", $"expected {nodes.Length - 1}, got {edgeCount}", ref failed);
            return;
        }
        Pass(ref testNumber, $"Prim MST edge count using {heapName}", ref passed);
    }
}