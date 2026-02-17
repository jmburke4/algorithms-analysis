using System.Diagnostics; 

namespace Implementation {

/// <summary>
/// Record metrics used in testing. 
/// 
/// Author: Brock Kitterman - bfkitterman@crimson.ua.edu
/// </summary>
public static class MetricsHandler
{
    // Data Tracking
    private static int heapOperations = 0;

    // Time related
    private static Stopwatch totalRuntimeWatch = new Stopwatch();
    private static Stopwatch heapTimeWatch = new Stopwatch();
    private static long heapExtractRuntime;
    private static long heapKeyRuntime;

    // Memory
    private static long memoryStart;
    private static long memoryEnd;

    public enum OpType { Insert, ExtractMin, DecreaseKey }
    private static OpType currentOp;

    /// <summary>
    /// Start a test
    /// 
    /// Reset all value and record initial values
    /// </summary>
    public static void StartTest() 
    {
        // Reset all values to 0, record start time
        heapOperations = 0;
        heapExtractRuntime = 0;
        heapKeyRuntime = 0;

        GC.Collect();
        GC.WaitForPendingFinalizers();
        memoryStart = GC.GetTotalMemory(true);

        totalRuntimeWatch.Restart();
    }

    /// <summary>
    /// Finalize test
    /// </summary>
    public static void EndTest() 
    {
        // Calculate total time and memory usage
        // Record memory change
        totalRuntimeWatch.Stop();
        memoryEnd = GC.GetTotalMemory(false);
    }

    /// <summary>
    /// Call on a heap operation (Insert, Decrease, Extract).
    /// </summary>
    public static void StartHeapOperation(OpType op) 
    {
        heapOperations++;
        currentOp = op;

        if(op != OpType.Insert)
            heapTimeWatch.Restart();
    }

    /// <summary>
    /// End heap timer and add to total time.
    /// </summary>
    public static void EndHeapOperation() {
        if(currentOp == OpType.Insert) return;

        heapTimeWatch.Stop();
        if(currentOp == OpType.DecreaseKey)
            heapKeyRuntime += heapTimeWatch.ElapsedTicks;
        else
            heapExtractRuntime += heapTimeWatch.ElapsedTicks;
    }

    /// <summary>
    /// Print out metrics recorded for the test
    /// </summary>
    public static void PrintMetrics()
    {
        Console.WriteLine("\t===Test Metrics===");
        Console.WriteLine($"\tTotal Runtime: {totalRuntimeWatch.Elapsed.TotalMilliseconds:F4} ms");
        Console.WriteLine($"\tHeap Operations: {heapOperations}");
        Console.WriteLine($"\tDecrease Key Time: {TimeSpan.FromTicks(heapKeyRuntime).TotalMilliseconds:F4} ms");
        Console.WriteLine($"\tExtract Min Time: {TimeSpan.FromTicks(heapExtractRuntime).TotalMilliseconds:F4} ms");
        Console.WriteLine($"\tMemory Delta: {(memoryEnd - memoryStart) / 1024.0:F2} KB");
    }
}
}