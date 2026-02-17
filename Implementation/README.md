# CS-470, Project 1 - Algorithms Analysis

**Brock Kitterman - bfkitterman@crimson.ua.edu**

**Jackson Burke - jmburke4@crimson.ua.edu**

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
The project is broken into three areas. A heaps folder, an algorithms folder, and a testing folder.

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

The Graph class is a generic weighted directed/undirected graph implementation that provides core functionality for algorithms like Dijkstra's and Prim's. It manages a collection of typed nodes and weighted edges efficiently using an adjacency list representation internally. The Graph class supports adding directed edges (`AddEdge`), undirected edges (`AddUndirectedEdge`), retrieving neighbors from a node, looking up edge weights, and accessing nodes by index or value.

The GraphTest class verifies the Graph implementation through comprehensive testing. Basic construction tests ensure that graphs are properly initialized with the correct node count, and that node indexing and lookups work correctly. Edge operation tests validate that directed and undirected edges are correctly stored and retrieve-able, confirming that GetNeighbors returns accurate edge weights and that edge weight lookups work for both existing and non-existing edges.

*Note:* To see test output, see `GraphTestOutput.md` file.

## Algorithm Testing

### Dijkstra's Algorithm Testing

Dijkstra's algorithm is tested across multiple graph scenarios to validate correct computation of single-source shortest paths. The DijkstraTest class exercises the algorithm with basic scenario tests (simple 4-node graphs), worst-case scenarios (linear chains requiring all edge relaxations), and best-case scenarios (direct edges from source). More complex tests include multi-path graphs where the algorithm must choose optimal routes, dense graphs with cycles and redundant paths, and large layered graphs with 25 nodes to stress the algorithm at scale. Each test validates both the computed shortest distances and the ability to reconstruct the actual shortest paths. All tests are executed with both Fibonacci and Pairing heap implementations to measure performance differences.

### Prim's Algorithm Testing

Prim's algorithm is tested for correct computation of minimum spanning trees across diverse graph topologies. The PrimsTest class covers basic graph scenarios with simple 4-node configurations, randomly generated graphs to test general case performance and correctness, grid-based graph structures to validate behavior on regular patterns, and worst-case scenarios designed to challenge the algorithm. Tests verify that the algorithm correctly identifies and includes all required edges for a minimum spanning tree while excluding unnecessary edges. Similar to Dijkstra testing, all Prim tests are executed with both Fibonacci and Pairing heap implementations to evaluate their relative performance impact on the algorithm.

**Note:** To see test output, see `PrimsTestOutput.md` or `DijkstraTestOutput.md` files.

# Discussion

## Where does each Algorithm shine?

Dijkstra's and Prim's algorithms serve fundamentally different purposes and excel in different contexts. Dijkstra's algorithm is specifically designed for single-source shortest path problems—finding the shortest routes from one node to all others—making it ideal for navigation systems, network routing protocols, and social network analysis where path optimization is critical. Prim's algorithm solves the minimum spanning tree problem, useful for network design, infrastructure planning, and approximation algorithms, where the goal is connecting all nodes with minimum total edge weight rather than optimizing individual paths. In terms of implementation complexity, Prim's is generally simpler: it greedily expands from a starting node without needing to track predecessors for path reconstruction, whereas Dijkstra's requires maintaining parent pointers for path recovery. Memory-wise, both are O(V) for distance/weight tracking, but Dijkstra's additional predecessor map adds modest overhead. Speed-efficient implementations strongly favor Dijkstra's in dense graphs thanks to its greater sensitivity to heap efficiency improvements—the O(E log V) complexity with binary heaps becomes much more favorable with Fibonacci heaps. However, for sparse graphs or small networks, this advantage diminishes significantly. Prim's offers more consistent performance across graph types since its O(E log V) complexity doesn't scale as dramatically with edge count. Choosing which algorithm to use depends on the problem (shortest paths vs spanning trees), graph density (dense favors optimized Dijkstra's, sparse is less critical), and implementation requirements (simpler Prim's vs optimizable Dijkstra's).

## Which Algorithm benefits more from an advanced heap?
As briefly mentioned above, Dijkstra's algorithm stands to benefit significantly more from advanced heap implementations than Prim's algorithm. This is because Dijkstra's performs DecreaseKey operations proportional to the number of edges in the graph—every time an edge is relaxed, a DecreaseKey call may occur. In dense graphs with many edges relative to nodes, this results in a large number of DecreaseKey calls, where the difference between O(log n) (binary heap) and O(1) amortized (Fibonacci heap) compounds substantially. Prim's algorithm, by contrast, only processes each edge once and maintains a more bounded set of DecreaseKey operations. While both algorithms benefit from faster decrease-key operations, Dijkstra's gain is more pronounced, especially in graphs with high edge density. Therefore, the choice of heap implementation has a more dramatic performance impact on Dijkstra's algorithm than on Prim's, making advanced heap structures like Fibonacci or Pairing heaps particularly valuable for shortest-path computations on large, dense graphs.

## How Graph Structure Affects Performance

The structure and topology of a graph significantly impact the runtime performance of both Dijkstra's and Prim's algorithms. Dense graphs with high edge-to-node ratios generate substantially more heap operations, particularly DecreaseKey calls, amplifying the benefits of efficient heap implementations. Conversely, sparse graphs with few edges relative to nodes result in fewer operations overall, reducing the performance differential between heap types. Graph connectivity also matters: well-connected graphs may cause algorithms to explore many paths before settling on optimal solutions, increasing computational work. Linear chain graphs represent a worst-case scenario where algorithms must process and relax edges sequentially, while fully connected graphs represent density extremes. The presence of cycles affects exploration patterns but not algorithmic correctness (since both algorithms naturally handle cycles). Layered or grid-structured graphs demonstrate more predictable access patterns that may interact favorably or unfavorably with CPU cache locality. Additionally, edge weight distribution influences convergence speed—uniform weights may lead to different exploration patterns than highly varied weights. These structural characteristics make testing across diverse graph topologies essential for understanding algorithm behavior and determining appropriate heap implementations for different real-world scenarios.

## Theory Vs. Practice
There are a few key reasons why the theory of these heaps do not necessarily align with the reality.

Firstly, computational theory and complexity are considered language agnostic and consider a perfect scenario, disregards hardware limitations and or implementations, and does not consider further overhead. 

Likewise, C# does a lot of low level lifting and memory abstraction, meaning it adds extra steps under the hood to handle memory and other situations. As such, complexities can change based on implementation. This is mostly due to garbage collection and automatic memory allocation. This issue posses much less of an issue in languages such as C and C++ where its handled by the programmer rather than the compiler/runtime environment. This can also be much worse if done in a language such as Lisp or Python with dynamic typing, or that run on a single-thread event loop like JavaScript.

It is also worth noting implementation of these heaps and their tests may not inherently be perfect and may have unnecessary steps not considered in the theory. Furthermore, it is worth noting that theory assumes a perfect environment. If, for example, a system were to be out of memory upon execution and require paging, times and operations can drastically vary based on paging system and implementation utilized by the operating system and hardware interfaces. However, this is secondary issue compared to cache locality. Since both heaps have heavy reliance on pointers, their operation time is drastically slowed by pointer chasing, which is slow in practice due to CPU misses (When a pointer/variable is not in the L1/2/3 cache and must be retrieved from memory, which pauses execution for hundreds of cycles).

There is also the idea of Big O and its shortcoming. Consider Fibonacci and Pair heaps time for decrease key, which are both O(1) (Amortized for Fibonacci, conjectured for Pair) with outlier worst cases as O(logn) when the next smallest node is the at the bottom of the tree.

There is one specific trap with Big O visible here: constants. Big O notation disregards constants. However, this is erroneous in the grand scheme, especially with Fibonacci heaps which have a great number of overhead. That O(1) does not consider that fibonacci heaps have a much greater total operation count. Fibonacci Decrease Key requires three pointers and two variables, whilst Pair heap only requires three pointers for this operation. As such, the code is shorter and can be executed faster.

With all that said, theory is a good outline but not inherently accurate to the reality of the situation.