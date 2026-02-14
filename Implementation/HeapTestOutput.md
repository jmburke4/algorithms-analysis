**Note**: This file was generated via the command pane, and is a direct copy of the test results as produced.

=== Fibonacci Heap Tests ===
Test 1 - FindMin on empty heap returns null - Passed
Test 2 - FindMin after single insert returns 5 - Passed
Test 3 - FindMin after insert 5,3,7 returns 3 - Passed
Test 4 - DeleteMin returns 1,5,8,10,15 in order - Passed
Test 5 - FindMin is null after all elements removed - Passed
Test 6 - FindMin after DecreaseKey(20->5) is 5 - Passed
Test 7 - DeleteMin returns decreased node (5) - Passed
Test 8 - Combined: insert then DecreaseKey(100->10); first DeleteMin gives 10 - Passed
Test 9 - Combined: remaining DeleteMin order 25,50,60,75 - Passed
Test 10 - DeleteMin on empty heap throws InvalidOperationException - Passed
Test 11 - DecreaseKey to larger value throws InvalidOperationException - Passed      
Test 12 - FindMin with string type returns "a" - Passed
Test 13 - DeleteMin with string type returns "a","m","z" in order - Passed

Results: 13 passed, 0 failed.
All tests passed.

=== Pair Heap Tests ===
Test 1 - FindMin on empty heap returns null - Passed
Test 2 - FindMin after single insert returns 5 - Passed
Test 3 - FindMin after insert 5,3,7 returns 3 - Passed
Test 4 - DeleteMin returns 1,5,8,10,15 in order - Passed
Test 5 - FindMin is null after all elements removed - Passed
Test 6 - FindMin after DecreaseKey(20->5) is 5 - Passed
Test 7 - DeleteMin returns decreased node (5) - Passed
Test 8 - Combined: insert then DecreaseKey(100->10); first DeleteMin gives 10 - Passed
Test 9 - Combined: remaining DeleteMin order 25,50,60,75 - Passed
Test 10 - DeleteMin on empty heap throws InvalidOperationException - Passed
Test 11 - DecreaseKey to larger value throws InvalidOperationException - Passed      
Test 12 - FindMin with string type returns "a" - Passed
Test 13 - DeleteMin with string type returns "a","m","z" in order - Passed

Results: 13 passed, 0 failed.
All tests passed.