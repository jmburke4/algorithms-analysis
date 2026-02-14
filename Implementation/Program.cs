using Implementation.Algorithms;

namespace Implementation;

class Program
{
    static void Main(string[] args)
    {
        Graph<string> strings = new(6, ["A", "B", "C", "D", "E", "F"]);
        strings.AddEdge("A", "B", 2);
        strings.AddEdge("A", "D", 8);
        strings.AddEdge("B", "D", 5);
        strings.AddEdge("B", "E", 6);
        strings.AddEdge("D", "E", 3);
        strings.AddEdge("D", "F", 2);
        strings.AddEdge("E", "C", 9);
        strings.AddEdge("E", "F", 1);
        strings.AddEdge("F", "C", 3);

        Console.WriteLine("Breakpoint...");
    }
}
