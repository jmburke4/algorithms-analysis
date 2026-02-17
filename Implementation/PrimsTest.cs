using Implementation.Algorithms;
using Implementation.Heaps;
using System;
using System.Reflection.Metadata.Ecma335;

namespace Implementation;

public static class PrimsTest
{
    public static void Run()
    {
        Console.WriteLine("=== Prims Tests ===");
        var testNumber = 0;
        var passed = 0;
        var failed = 0;

        RunBasicPrimScenario<FibonacciHeap<Prims.KeyedVertex>>(ref testNumber, ref passed, ref failed, "FibonacciHeap");
        RunBasicPrimScenario<PairingHeap<Prims.KeyedVertex>>(ref testNumber, ref passed, ref failed, "PairingHeap");

        RunRandomGraphScenario<FibonacciHeap<Prims.KeyedVertex>>(ref testNumber, ref passed, ref failed, "FibonacciHeap");
        RunRandomGraphScenario<PairingHeap<Prims.KeyedVertex>>(ref testNumber, ref passed, ref failed, "PairingHeap");

        RunGridGraphScenarios<FibonacciHeap<Prims.KeyedVertex>>(ref testNumber, ref passed, ref failed, "FibonacciHeap");
        RunGridGraphScenarios<PairingHeap<Prims.KeyedVertex>>(ref testNumber, ref passed, ref failed, "PairingHeap");

        RunWorstCaseScenario<FibonacciHeap<Prims.KeyedVertex>>(ref testNumber, ref passed, ref failed, "FibonacciHeap");
        RunWorstCaseScenario<PairingHeap<Prims.KeyedVertex>>(ref testNumber, ref passed, ref failed, "PairingHeap");


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

    //Simple undirected graph, checks MST total weight and edge count
    private static void RunBasicPrimScenario<THeap>(ref int testNumber, ref int passed, ref int failed, string heapName)
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
        else
        {
            Pass(ref testNumber, $"Prim MST total weight is 6 using {heapName}", ref passed);
        }

        //Test 2 - Structure Check
        int edgeCount = 0;
        for (int i = 0; i < parent.Length; i++)
        {
            if (parent[i] != -1)
                edgeCount++;
        }

        if (edgeCount != nodes.Length - 1)
        {
            Fail(ref testNumber, $"Prim MST edge count using {heapName}", $"expected {nodes.Length - 1}, got {edgeCount}", ref failed);
            return;
        }
        Pass(ref testNumber, $"Prim MST edge count is {edgeCount} using {heapName}", ref passed);
    }

    //Tests Prim's on a random undirected graph
    //Each possible edge includes a fixed probability and random weight
    private static void RunRandomGraphScenario<THeap>(ref int testNumber, ref int passed, ref int failed, string heapName)
           where THeap : IHeap<Prims.KeyedVertex>, new()
    {
        var rValues = new (int nodes, double p)[]
        {
            (10, 0.3), (25, 0.5), (20, 0.7), (45, 0.2)
        };

        foreach (var (n, p) in rValues)
        {
            var graph = CreateRandomGraph(n, p);
            var heap = new THeap();

            var (parent, totWeight) = Prims.MinimumSpanningTree(graph, heap, "Node0");

            int edgeCount = parent.Count(x => x != -1);

            if(edgeCount != n - 1) 
            {
                Fail(ref testNumber, $"Random graph n = {n}, p={p} using {heapName}", $"expected {n - 1} edges, got {edgeCount}", ref failed);
            }
            Pass(ref testNumber, $"Random graph n = {n}, p={p} using {heapName}", ref passed);
        }
    }

    //Tests Prim's on various grid graphs
    //Each node is connected to (if possible) it's right and bottom neighbors
    private static void RunGridGraphScenarios<THeap>(ref int testNumber, ref int passed, ref int failed, string heapName)
       where THeap : IHeap<Prims.KeyedVertex>, new()
    {
        var gridSizes = new (int rows, int cols)[]
        {
            (2,2), (4,4), (5,5), (7,7)
        };

        foreach (var (rows, cols) in gridSizes)
        {
            var graph = CreateGridGraph(rows, cols);
            var heap = new THeap();

            var (parent, totWeight) = Prims.MinimumSpanningTree(graph, heap, "(0,0)");

            int expWeight = rows * cols - 1;
            if (totWeight != expWeight)
            {
                Fail(ref testNumber, $"Prim MST total weight using {heapName} on Grid Graph", $"Expected {expWeight}, got {totWeight}", ref failed);
                return;
            }
            Pass(ref testNumber, $"Prim MST total weight is {totWeight} using {heapName} on Grid Graph", ref passed);
        }
    }

    //Tests Prim's on a simple worst-case graph
    //this graph acts as a stress test for the heap's decrease-key
    private static void RunWorstCaseScenario<THeap>(ref int testNumber, ref int passed, ref int failed, string heapName)
       where THeap : IHeap<Prims.KeyedVertex>, new()
    {
        int n = 8;
        var graph = CreateWorstCaseGraph(n);
        var heap = new THeap();

        var (parent, totWeight) = Prims.MinimumSpanningTree(graph, heap, "Node0");

        int edgeCount = parent.Count(p => p != -1);

        if (edgeCount != n - 1)
        {
            Fail(ref testNumber, $"Simple worst-case MST edge count using {heapName}", $"expected {n - 1}, got {edgeCount}", ref failed);
            return;
        }
        if(totWeight <= 0)
        {
            Fail(ref testNumber, $"Simple worst-case MST weight using {heapName}", $"weight should be positive, got {totWeight}", ref failed);
            return;
        }
        Pass(ref testNumber, $"Simple worst-case MST valid using {heapName} ( weight = {totWeight})", ref passed);
    }
    public static Graph<string> CreateGridGraph(int rows, int cols)
    {
        var nodes = new List<string>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                nodes.Add($"({i},{j})");
            }
        }

        var graph = new Graph<string>(nodes);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                int currIndex = i * cols + j;
                if (j < cols - 1)
                {
                    graph.AddUndirectedEdge(nodes[currIndex], nodes[currIndex + 1], 1);
                }
                if (i < rows - 1)
                {
                    graph.AddUndirectedEdge(nodes[currIndex], nodes[currIndex + cols], 1);
                }
            }
        }
        return graph;
    }

    public static Graph<string> CreateRandomGraph(int numNodes, double edgeProb)
    {
        var nodes = new List<string>();
        for (int i = 0; i < numNodes; i++)
        {
            nodes.Add($"Node{i}");
        }

        var graph = new Graph<string>(nodes);
        Random r = new Random();

        for (int i = 0; i < numNodes; i++)
        {
            for (int j = 0; j < numNodes; j++)
            {
                if (r.NextDouble() < edgeProb)
                {
                    int weight = r.Next(1, 11);
                    graph.AddUndirectedEdge(nodes[i], nodes[j], weight);
                }
            }
        }
        return graph;
    }

    public static Graph<string> CreateWorstCaseGraph(int numNodes)
    {
        var nodes = new List<string>();
        for (int i = 0; i < numNodes; i++)
        {
            nodes.Add($"Node{i}");
        }

        var graph = new Graph<string>(nodes);
        Random r = new Random();

        for (int i = 0; i < numNodes; i++)
        {
            for (int j = i + 1; j < numNodes; j++)
            {
                int weight = r.Next(1, 11);
                graph.AddUndirectedEdge(nodes[i], nodes[j], weight);
            }
        }
        return graph;
    }
}

