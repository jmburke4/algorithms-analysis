using Implementation.Algorithms;

namespace Implementation;

public static class GraphTest
{
    public static void Run()
    {
        Console.WriteLine("=== Graph Tests ===");
        var testNumber = 0;
        var passed = 0;
        var failed = 0;

        RunBasicConstruction(ref testNumber, ref passed, ref failed);
        RunEdgeOperations(ref testNumber, ref passed, ref failed);

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

    private static void RunBasicConstruction(ref int testNumber, ref int passed, ref int failed)
    {
        var nodes = new[] { "A", "B", "C", "D" };
        var g = new Graph<string>(nodes);

        if (g.Count != nodes.Length)
        {
            Fail(ref testNumber, "Graph construction: Count matches input length", $"expected {nodes.Length}, got {g.Count}", ref failed);
            return;
        }
        Pass(ref testNumber, "Graph construction: Count matches input length", ref passed);

        try
        {
            var idxA = g.IndexOf("A");
            var nodeAt0 = g.NodeAt(0);
            if (idxA != 0 || nodeAt0 != "A")
            {
                Fail(ref testNumber, "IndexOf/NodeAt return consistent values for 'A'", $"IndexOf A={idxA}, NodeAt(0)={nodeAt0}", ref failed);
                return;
            }
            Pass(ref testNumber, "IndexOf/NodeAt return consistent values for 'A'", ref passed);
        }
        catch (Exception ex)
        {
            Fail(ref testNumber, "IndexOf/NodeAt accessors work without throwing", ex.GetType().Name + ": " + ex.Message, ref failed);
        }
    }

    private static void RunEdgeOperations(ref int testNumber, ref int passed, ref int failed)
    {
        var nodes = new[] { "A", "B", "C", "D" };
        var g = new Graph<string>(nodes);

        // Add directed edges A->B (5), A->C (2), C->D (1)
        g.AddEdge("A", "B", 5);
        g.AddEdge("A", "C", 2);
        g.AddEdge("C", "D", 1);

        // Add undirected edge B<->D weight 3
        g.AddUndirectedEdge("B", "D", 3);

        // Verify neighbor enumeration for A
        var neighA = g.GetNeighbors("A").ToList();
        if (neighA.Count != 2 || !neighA.Any(n => n.neighbor == "B" && n.weight == 5) || !neighA.Any(n => n.neighbor == "C" && n.weight == 2))
        {
            Fail(ref testNumber, "GetNeighbors for A returns expected neighbors and weights", "unexpected neighbor list", ref failed);
        }
        else
        {
            Pass(ref testNumber, "GetNeighbors for A returns expected neighbors and weights", ref passed);
        }

        // TryGetWeight existing and non-existing
        if (!g.TryGetWeight("A", "B", out var w) || w != 5)
        {
            Fail(ref testNumber, "TryGetWeight for existing edge A->B returns weight 5", $"got {w}", ref failed);
        }
        else
        {
            Pass(ref testNumber, "TryGetWeight for existing edge A->B returns weight 5", ref passed);
        }

        if (g.TryGetWeight("B", "A", out _))
        {
            Fail(ref testNumber, "TryGetWeight for non-existing B->A returns false", "edge unexpectedly present", ref failed);
        }
        else
        {
            Pass(ref testNumber, "TryGetWeight for non-existing B->A returns false", ref passed);
        }

        // Test duplicate edge weight replacement: A->B currently 5, add A->B with 3
        g.AddEdge("A", "B", 3);
        if (!g.TryGetWeight("A", "B", out var w2) || w2 != 3)
        {
            Fail(ref testNumber, "AddEdge replaces existing edge with smaller weight", $"expected 3, got {w2}", ref failed);
        }
        else
        {
            Pass(ref testNumber, "AddEdge replaces existing edge with smaller weight", ref passed);
        }

        // Test RemoveEdge
        var removed = g.RemoveEdge("A", "C");
        if (!removed || g.TryGetWeight("A", "C", out _))
        {
            Fail(ref testNumber, "RemoveEdge removes A->C", "edge still present after removal", ref failed);
        }
        else
        {
            Pass(ref testNumber, "RemoveEdge removes A->C", ref passed);
        }
    }
}
