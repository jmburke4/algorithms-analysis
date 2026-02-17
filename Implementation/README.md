# CS-470, Project 1 - Algorithms Analysis
**Brock Kitterman - bfkitterman@crimson.ua.edu**

This file overviews the Algorithms Analysis project for CS-470.

This project was made with C#. In order to run this program, please ensure that `.NET 8.0 SDK` is installed on your machine. To verify installation, in cmd, enter:
```bash
dotnet --version
```
As long as a version is returned, dotnet is installed. 

This project was developed on `.NET 9.0.36`, but should be backwards compatible with previous versions. Whilst this is possible, it is recommended to run on the latest version. 

# Compilation and Execution
Using the base command 'dotnet', both compilation and running will be executed at the same time. It is not entirely impossible that, on first build, multiple warnings may be printed to the CMD, most of which are about nullable references. These are of no concern and are a result of the C# project having null return and reference protection enabled. They will not interfere with the execution of the code.

In order to run and test files, first navigate to their expected folder. From here, running the following command will produce results of the tests of these algorithms and heaps.

To run the system, use the following commands:

```bash
dotnet run # this will run the test engine, and will run all tests
```

# Organization
The project is broken into three areas. A heaps folder, a algorithms folder, and a testing folder.

#### Project Directory
**Note:** Only important files are recorded here.
```
|- Heaps
|   |- PairingHeaps.cs
|   |- Node.cs
|   |- IHeap.CS
|   ⌞- FibonacciHeaps.cs
|- Algorithms
|   |- Dijkstra.cs
|   |- Graph.cs
|   ⌞- Prims.cs
|- Tests
|   |- DijkstraTest.cs
|   |- FibHeapTest.cs
|   |- GraphTest.cs
|   |- PairHeapTest.cs
|   |- PrimsTest.cs
|   ⌞- TestOutputs
|       ⌞- All Output Files (Markdowns)
|- FibHeapTest.cs
|- PairHeapTest.cs
|- Program.cs
|- Implementation.csproj
⌞-README.md
```

# Testing
## Metric Collection
Metrics are collected for the following for each individual test case:
- Total Runtime (ms)
- Heap Operations (Insert, ExtractMin, DecreaseKey)
- Decrease Key Time (ms)
- Extract Min Time (ms)
- Memory  Delta (kb, memory change from start to end of test pre-garbage collection)

Note: If Decrease/Extract time is 0, it means those functions were never called. 

All time is collected with the same metrics system with the same calls to avoid mismatches, and interior methods (Cut, Merge, etc.) do not count towards operations as they are not public API.

Extract/Decrease time checks take place after a initial root != null check. However, since both heaps do the same checks, this is trivial and will not affect overall results. 

**NOTE:** Whilst functionality will be the same, performance and metrics vary greatly across machines, and output may be different on the metrics end. All provided output was produced on the same machine with the same states to ensure continuity across tests.

## Heap Testing
For both fibonacci and pair heap implementations, the same basic tests were performed in order to ensure they were functional, with multiple data types as well.

These tests are:
1. FindMin on empty
2. FindMin after single insert
3. FindMin after insert of many
4. DeleteMin until empty heap
5. FindMin after emptied heap
6. FindMin after decrease key
7. DeleteMin returns removed node
8,9,10. Combined insert and then decrease key
11. Decrease Key on larger value
12. Find Min with strings
13. Delete min with many strings 

*Note:* To see test output, see `HeapTestOutput.md` file.

## Graph Testing


*Note:* To see test output, see `GraphTestOutput.md` file.

## Algorithm Testing


**Note:** To see test output, see `PrimsTestOutput.md` or `DijkstraTestOutput.md` files.

# Theory Vs. Practice
There are a few key reasons why the theory of these heaps do not necessarily align with the reality.

Firstly, computational theory and complexity are considered language agnostic and consider a perfect scenario, disregards hardware limitations and or implementations, and does not consider further overhead. 

Likewise, C# does a lot of low level lifting and memory abstraction, meaning it adds extra steps under the hood to handle memory and other situations. As such, complexities can change based on implementation. This is mostly due to garbage collection and automatic memory allocation. This issue posses much less of an issue in languages such as C and C++ where its handled by the programmer rather than the compiler/runtime environment. This can also be much worse if done in a language such as Lisp or Python with dynamic typing, or that run on a single-thread event loop like JavaScript.

It is also worth noting implementation of these heaps and their tests may not inherently be perfect and may have unnecessary steps not considered in the theory. Furthermore, it is worth noting that theory assumes a perfect environment. If, for example, a system were to be out of memory upon execution and require paging, times and operations can drastically vary based on paging system and implementation utilized by the operating system and hardware interfaces. However, this is secondary issue compared to cache locality. Since both heaps have heavy reliance on pointers, their operation time is drastically slowed by pointer chasing, which is slow in practice due to CPU misses (When a pointer/variable is not in the L1/2/3 cache and must be retrieved from memory, which pauses execution for hundreds of cycles).

There is also the idea of Big O and its shortcoming. Consider Fibonacci and Pair heaps time for decrease key, which are both O(1) (Amortized for Fibonacci, conjectured for Pair) with outlier worst cases as O(logn) when the next smallest node is the at the bottom of the tree.

There is one specific trap with Big O visible here: constants. Big O notation disregards constants. However, this is erroneous in the grand scheme, especially with Fibonacci heaps which have a great number of overhead. That O(1) does not consider that fibonacci heaps have a much greater total operation count. Fibonacci Decrease Key requires three pointers and two variables, whilst Pair heap only requires three pointers for this operation. As such, the code is shorter and can be executed faster.

With all that said, theory is a good outline but not inherently accurate to the reality of the situation.