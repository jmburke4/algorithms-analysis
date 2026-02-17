**Note**: This file was generated via the command pane, and is a direct copy of the test results as produced.

=== Fibonacci Heap Tests ===                                                                 
Test 1 - FindMin on empty heap returns null - Passed
        ===Test Metrics===
        Total Runtime: 0.1500 ms
        Heap Operations: 0
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 8.03 KB
Test 2 - FindMin after single insert returns 5 - Passed
        ===Test Metrics===
        Total Runtime: 0.6397 ms
        Heap Operations: 1
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 11.97 KB
Test 3 - FindMin after insert 5,3,7 returns 3 - Passed
        ===Test Metrics===
        Total Runtime: 0.0371 ms
        Heap Operations: 2
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 11.97 KB
Test 4 - DeleteMin returns 1,5,8,10,15 in order - Passed
        ===Test Metrics===
        Total Runtime: 1.0104 ms
        Heap Operations: 10
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.2708 ms
        Memory Delta: 16.06 KB
Test 5 - FindMin is null after all elements removed - Passed
        ===Test Metrics===
        Total Runtime: 0.0002 ms
        Heap Operations: 0
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 0.00 KB
Test 6 - FindMin after DecreaseKey(20->5) is 5 - Passed
        ===Test Metrics===
        Total Runtime: 0.1293 ms
        Heap Operations: 4
        Decrease Key Time: 0.0002 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 16.06 KB
Test 7 - DeleteMin returns decreased node (5) - Passed
        ===Test Metrics===
        Total Runtime: 0.0030 ms
        Heap Operations: 1
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0026 ms
        Memory Delta: 3.94 KB
Test 8 - Combined: insert then DecreaseKey(100->10); first DeleteMin gives 10 - Passed       
        ===Test Metrics===
        Total Runtime: 0.0034 ms
        Heap Operations: 7
        Decrease Key Time: 0.0001 ms
        Extract Min Time: 0.0017 ms
        Memory Delta: 8.03 KB
Test 9 - Combined: remaining DeleteMin order 25,50,60,75 - Passed
        ===Test Metrics===
        Total Runtime: 0.0138 ms
        Heap Operations: 4
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0028 ms
        Memory Delta: 16.06 KB
Test 10 - DeleteMin on empty heap throws InvalidOperationException - Passed
        ===Test Metrics===
        Total Runtime: 0.1571 ms
        Heap Operations: 0
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 16.06 KB
Test 11 - DecreaseKey to larger value throws InvalidOperationException - Passed
        ===Test Metrics===
        Total Runtime: 0.0110 ms
        Heap Operations: 1
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 16.06 KB
Test 12 - FindMin with string type returns "a" - Passed
        ===Test Metrics===
        Total Runtime: 0.6656 ms
        Heap Operations: 3
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 16.06 KB
Test 13 - DeleteMin with string type returns "a","m","z" in order - Passed
        ===Test Metrics===
        Total Runtime: 0.6702 ms
        Heap Operations: 3
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.2857 ms
        Memory Delta: 16.06 KB

Results: 13 passed, 0 failed.
All tests passed.

=== Pair Heap Tests ===
Test 1 - FindMin on empty heap returns null - Passed
        ===Test Metrics===
        Total Runtime: 0.1532 ms
        Heap Operations: 0
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 16.06 KB
Test 2 - FindMin after single insert returns 5 - Passed
        ===Test Metrics===
        Total Runtime: 0.1958 ms
        Heap Operations: 1
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 11.97 KB
Test 3 - FindMin after insert 5,3,7 returns 3 - Passed
        ===Test Metrics===
        Total Runtime: 0.0024 ms
        Heap Operations: 2
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 3.94 KB
Test 4 - DeleteMin returns 1,5,8,10,15 in order - Passed
        ===Test Metrics===
        Total Runtime: 0.6730 ms
        Heap Operations: 10
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.1048 ms
        Memory Delta: 16.06 KB
Test 5 - FindMin is null after all elements removed - Passed
        ===Test Metrics===
        Total Runtime: 0.0003 ms
        Heap Operations: 0
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 0.00 KB
Test 6 - FindMin after DecreaseKey(20->5) is 5 - Passed
        ===Test Metrics===
        Total Runtime: 0.1679 ms
        Heap Operations: 4
        Decrease Key Time: 0.0004 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 16.06 KB
Test 7 - DeleteMin returns decreased node (5) - Passed
        ===Test Metrics===
        Total Runtime: 0.0025 ms
        Heap Operations: 1
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0020 ms
        Memory Delta: 8.03 KB
Test 8 - Combined: insert then DecreaseKey(100->10); first DeleteMin gives 10 - Passed       
        ===Test Metrics===
        Total Runtime: 0.0030 ms
        Heap Operations: 7
        Decrease Key Time: 0.0002 ms
        Extract Min Time: 0.0011 ms
        Memory Delta: 8.03 KB
Test 9 - Combined: remaining DeleteMin order 25,50,60,75 - Passed
        ===Test Metrics===
        Total Runtime: 0.0048 ms
        Heap Operations: 4
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0021 ms
        Memory Delta: 8.03 KB
Test 10 - DeleteMin on empty heap throws InvalidOperationException - Passed
        ===Test Metrics===
        Total Runtime: 0.0096 ms
        Heap Operations: 0
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 16.06 KB
Test 11 - DecreaseKey to larger value throws InvalidOperationException - Passed
        ===Test Metrics===
        Total Runtime: 0.0198 ms
        Heap Operations: 1
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 16.06 KB
Test 12 - FindMin with string type returns "a" - Passed
        ===Test Metrics===
        Total Runtime: 0.2509 ms
        Heap Operations: 3
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0000 ms
        Memory Delta: 16.06 KB
Test 13 - DeleteMin with string type returns "a","m","z" in order - Passed
        ===Test Metrics===
        Total Runtime: 0.2186 ms
        Heap Operations: 3
        Decrease Key Time: 0.0000 ms
        Extract Min Time: 0.0221 ms
        Memory Delta: 16.06 KB

Results: 13 passed, 0 failed.
All tests passed.