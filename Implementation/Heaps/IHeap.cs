namespace Implementation.Heaps;

public interface IHeap<T> where T : IComparable<T>
{
    /// <summary>
    /// Insert a new value into the heap and return the node containing it.
    /// </summary>
    /// <param name="value">Value to insert</param>
    /// <returns>Node containing the inserted value</returns>
    public Node<T> Insert(T value);

    /// <summary>
    /// Return the minimum value in the heap without modifying the heap.
    /// </summary>
    /// <returns>The minimum value in the heap</returns>
    public T? FindMin();

    /// <summary>
    /// Remove and return the minimum value in the heap, modifying the heap.
    /// </summary>
    /// <returns>The minimum value in the heap</returns>
    public T ExtractMin();

    /// <summary>
    /// Decrease the value of a node to a new value that is less than or equal to its current value.
    /// </summary>
    /// <param name="node">Node to decrease</param>
    /// <param name="newValue">New value for the node; must be <= current value</param>
    public void DecreaseKey(Node<T> node, T newValue);
}
