# CS-470, Project 1 - Algorithms Analysis
# Lox Interpreter (C#)
**Brock Kitterman - 12322954 - bfkitterman@crimson.ua.edu**

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
|- Heaps
|   |- PairingHeaps.cs
|   ⌞- FibonacciHeaps.cs
|- Algorithms
|   |- TestFiles
|       |- 
|       ⌞- complexFunctions.
|   ⌞- LoxTester.cs
|- FibHeapTest.cs
|- PairHeapTest.cs
|- Program.cs
|- Implementation.csproj
⌞-README.md

# Testing
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