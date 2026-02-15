using System;
using Implementation.Heaps;

namespace Implementation;

/// <summary>
/// Tests for FibonacciHeap to verify insert, find-min, extract-min, and decrease-key behavior.
/// Author: Christian Lindner, calindner@crimson.ua.edu
/// </summary>
public static class FibHeapTest
{
    /// <summary>
    /// Run all Fibonacci heap unit tests and print results.
    /// </summary>
    public static void Run()
    {
        Console.WriteLine("=== Fibonacci Heap Tests ===");
        var testNumber = 0;
        var passed = 0;
        var failed = 0;

        RunInsertAndFindMin(ref testNumber, ref passed, ref failed);
        RunExtractMin(ref testNumber, ref passed, ref failed);
        RunDecreaseKey(ref testNumber, ref passed, ref failed);
        RunCombined(ref testNumber, ref passed, ref failed);
        RunEmptyDeleteMinThrows(ref testNumber, ref passed, ref failed);
        RunDecreaseKeyInvalidThrows(ref testNumber, ref passed, ref failed);
        RunDataTypeIndependence(ref testNumber, ref passed, ref failed);

        Console.WriteLine();
        Console.WriteLine($"Results: {passed} passed, {failed} failed.");
        Console.WriteLine(failed == 0 ? "All tests passed." : "Some tests failed.");
    }

    /// <summary>
    /// Record a passing test and print "Test # - &lt;description&gt; - Passed".
    /// </summary>
    /// <param name="testNumber">Current test index (incremented)</param>
    /// <param name="description">What the test did</param>
    /// <param name="passed">Count of passed tests (incremented)</param>
    private static void Pass(ref int testNumber, string description, ref int passed)
    {
        testNumber++;
        Console.WriteLine($"Test {testNumber} - {description} - Passed");
        passed++;
    }

    /// <summary>
    /// Record a failing test and print "Test # - &lt;description&gt; - FAILED" with reason.
    /// </summary>
    /// <param name="testNumber">Current test index (incremented)</param>
    /// <param name="description">What the test did</param>
    /// <param name="reason">Why or how the result differs from expected</param>
    /// <param name="failed">Count of failed tests (incremented)</param>
    private static void Fail(ref int testNumber, string description, string reason, ref int failed)
    {
        testNumber++;
        Console.WriteLine($"Test {testNumber} - {description} - FAILED: {reason}");
        failed++;
    }

    /// <summary>
    /// Run unit tests for Insert and FindMin on an integer heap.
    /// </summary>
    /// <param name="testNumber">Current test index</param>
    /// <param name="passed">Count of passed tests</param>
    /// <param name="failed">Count of failed tests</param>
    private static void RunInsertAndFindMin(ref int testNumber, ref int passed, ref int failed)
    {
        var heap = new FibonacciHeap<int>();

        // FindMin on empty heap should return null
        if (heap.FindMin() != default(int))
        {
            Fail(ref testNumber, "FindMin on empty heap returns null", "expected null, got non-null", ref failed);
            return;
        }
        Pass(ref testNumber, "FindMin on empty heap returns null", ref passed);

        var n5 = heap.Insert(5);
        var min = heap.FindMin();
        if (min == default(int) || min != 5)
        {
            Fail(ref testNumber, "FindMin after single insert returns 5", $"expected 5, got {(min == null ? "null" : min.ToString())}", ref failed);
            return;
        }
        Pass(ref testNumber, "FindMin after single insert returns 5", ref passed);

        heap.Insert(3);
        heap.Insert(7);
        min = heap.FindMin();
        if (min == default(int) || min!= 3)
        {
            Fail(ref testNumber, "FindMin after insert 5,3,7 returns 3", $"expected 3, got {(min == null ? "null" : min.ToString())}", ref failed);
            return;
        }
        Pass(ref testNumber, "FindMin after insert 5,3,7 returns 3", ref passed);
    }

    /// <summary>
    /// Run unit tests for DeleteMin: extract-min order and empty heap.
    /// </summary>
    /// <param name="testNumber">Current test index</param>
    /// <param name="passed">Count of passed tests</param>
    /// <param name="failed">Count of failed tests</param>
    private static void RunExtractMin(ref int testNumber, ref int passed, ref int failed)
    {
        var heap = new FibonacciHeap<int>();
        heap.Insert(10);
        heap.Insert(5);
        heap.Insert(15);
        heap.Insert(1);
        heap.Insert(8);

        var order = new[] { 1, 5, 8, 10, 15 };
        for (var i = 0; i < order.Length; i++)
        {
            var node = heap.ExtractMin();
            if (node != order[i])
            {
                Fail(ref testNumber, "DeleteMin returns 1,5,8,10,15 in order", $"DeleteMin #{i + 1}: expected {order[i]}, got {node}", ref failed);
                return;
            }
        }
        Pass(ref testNumber, "DeleteMin returns 1,5,8,10,15 in order", ref passed);

        if (heap.FindMin() != default(int))
        {
            Fail(ref testNumber, "FindMin is null after all elements removed", "expected null, got non-null", ref failed);
            return;
        }
        Pass(ref testNumber, "FindMin is null after all elements removed", ref passed);
    }

    /// <summary>
    /// Run unit tests for DecreaseKey and FindMin after decrease.
    /// </summary>
    /// <param name="testNumber">Current test index</param>
    /// <param name="passed">Count of passed tests</param>
    /// <param name="failed">Count of failed tests</param>
    private static void RunDecreaseKey(ref int testNumber, ref int passed, ref int failed)
    {
        var heap = new FibonacciHeap<int>();
        var n20 = heap.Insert(20);
        heap.Insert(10);
        heap.Insert(30);

        if (heap.FindMin() != 10)
        {
            Fail(ref testNumber, "FindMin before DecreaseKey is 10", "expected 10", ref failed);
            return;
        }

        heap.DecreaseKey(n20, 5);
        var min = heap.FindMin();
        if (min == default(int) || min != 5)
        {
            Fail(ref testNumber, "FindMin after DecreaseKey(20->5) is 5", $"expected 5, got {(min == null ? "null" : min.ToString())}", ref failed);
            return;
        }
        Pass(ref testNumber, "FindMin after DecreaseKey(20->5) is 5", ref passed);

        var first = heap.ExtractMin();
        if (first != 5)
        {
            Fail(ref testNumber, "DeleteMin returns decreased node (5)", $"expected 5, got {first}", ref failed);
            return;
        }
        Pass(ref testNumber, "DeleteMin returns decreased node (5)", ref passed);
    }

    /// <summary>
    /// Run combined scenario: insert, DecreaseKey, then DeleteMin order.
    /// </summary>
    /// <param name="testNumber">Current test index</param>
    /// <param name="passed">Count of passed tests</param>
    /// <param name="failed">Count of failed tests</param>
    private static void RunCombined(ref int testNumber, ref int passed, ref int failed)
    {
        var heap = new FibonacciHeap<int>();
        var n100 = heap.Insert(100);
        heap.Insert(50);
        heap.Insert(75);
        heap.Insert(25);
        heap.Insert(60);

        heap.DecreaseKey(n100, 10);
        var min = heap.ExtractMin();
        if (min != 10)
        {
            Fail(ref testNumber, "Combined: insert then DecreaseKey(100->10); first DeleteMin gives 10", $"expected 10, got {min}", ref failed);
            return;
        }
        Pass(ref testNumber, "Combined: insert then DecreaseKey(100->10); first DeleteMin gives 10", ref passed);

        var expectedOrder = new[] { 25, 50, 60, 75 };
        foreach (var expected in expectedOrder)
        {
            var node = heap.ExtractMin();
            if (node != expected)
            {
                Fail(ref testNumber, "Combined: remaining DeleteMin order 25,50,60,75", $"expected {expected}, got {node}", ref failed);
                return;
            }
        }
        Pass(ref testNumber, "Combined: remaining DeleteMin order 25,50,60,75", ref passed);
    }

    /// <summary>
    /// Run unit test: DeleteMin on empty heap must throw InvalidOperationException.
    /// </summary>
    /// <param name="testNumber">Current test index</param>
    /// <param name="passed">Count of passed tests</param>
    /// <param name="failed">Count of failed tests</param>
    private static void RunEmptyDeleteMinThrows(ref int testNumber, ref int passed, ref int failed)
    {
        var heap = new FibonacciHeap<int>();
        try
        {
            heap.ExtractMin();
            Fail(ref testNumber, "DeleteMin on empty heap throws InvalidOperationException", "no exception thrown", ref failed);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("empty", StringComparison.OrdinalIgnoreCase))
        {
            Pass(ref testNumber, "DeleteMin on empty heap throws InvalidOperationException", ref passed);
        }
        catch (Exception ex)
        {
            Fail(ref testNumber, "DeleteMin on empty heap throws InvalidOperationException", $"got {ex.GetType().Name}", ref failed);
        }
    }

    /// <summary>
    /// Run unit test: DecreaseKey to larger value must throw InvalidOperationException.
    /// </summary>
    /// <param name="testNumber">Current test index</param>
    /// <param name="passed">Count of passed tests</param>
    /// <param name="failed">Count of failed tests</param>
    private static void RunDecreaseKeyInvalidThrows(ref int testNumber, ref int passed, ref int failed)
    {
        var heap = new FibonacciHeap<int>();
        var n = heap.Insert(5);
        try
        {
            heap.DecreaseKey(n, 10);
            Fail(ref testNumber, "DecreaseKey to larger value throws InvalidOperationException", "no exception thrown", ref failed);
        }
        catch (InvalidOperationException)
        {
            Pass(ref testNumber, "DecreaseKey to larger value throws InvalidOperationException", ref passed);
        }
        catch (Exception ex)
        {
            Fail(ref testNumber, "DecreaseKey to larger value throws InvalidOperationException", $"got {ex.GetType().Name}", ref failed);
        }
    }

    /// <summary>
    /// Demonstrates data type independence: heap works with string (IComparable) as well as int.
    /// </summary>
    /// <param name="testNumber">Current test index</param>
    /// <param name="passed">Count of passed tests</param>
    /// <param name="failed">Count of failed tests</param>
    private static void RunDataTypeIndependence(ref int testNumber, ref int passed, ref int failed)
    {
        var heap = new FibonacciHeap<string>();
        heap.Insert("z");
        heap.Insert("a");
        heap.Insert("m");

        var min = heap.FindMin();
        if (min == default(string) || min != "a")
        {
            Fail(ref testNumber, "FindMin with string type returns \"a\"", $"expected \"a\", got {(min == null ? "null" : $"\"{min}\"")}", ref failed);
            return;
        }
        Pass(ref testNumber, "FindMin with string type returns \"a\"", ref passed);

        var order = new[] { "a", "m", "z" };
        for (var i = 0; i < order.Length; i++)
        {
            var node = heap.ExtractMin();
            if (node != order[i])
            {
                Fail(ref testNumber, "DeleteMin with string type returns \"a\",\"m\",\"z\" in order", $"#{i + 1}: expected \"{order[i]}\", got \"{node}\"", ref failed);
                return;
            }
        }
        Pass(ref testNumber, "DeleteMin with string type returns \"a\",\"m\",\"z\" in order", ref passed);
    }
}
