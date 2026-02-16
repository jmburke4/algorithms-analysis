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

    private static void RunBasicScenario(ref int testNumber, ref int passed, ref int failed)
    {
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
}
