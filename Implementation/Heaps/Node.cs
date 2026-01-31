using System;

/// <summary>
/// A node implementation for either pairing or fibonacci heaps.
/// 
/// <para>
/// Author: Brock Kitterman
/// </para>
/// </summary>
/// <typeparam name="T"></typeparam>
public class Node<T> where T: IComparable<T>
{
    /// <summary>
    /// Node Value, implements IComparable
    /// </summary>
    public T Value {get; set;}

    // Structural pointers

    /// <summary>
    /// First Child node
    /// </summary>
    public Node<T>? Child {get; set;}
    /// <summary>
    /// Next sibling in level
    /// </summary>
    public Node<T>? Sibling {get; set;}
    /// <summary>
    /// Behaves differently depending on heap structure:
    /// <para> 
    /// Pairing Heap: Previous is either previous sibling or null if first child
    /// </para>
    /// Fibonacci Heap: Previous refers to previous sibling always (Circular list)
    /// </summary>
    public Node<T>? Prev {get; set;}
    /// <summary>
    /// Parent Node
    /// </summary>
    public Node<T>? Parent {get; set;}

    // Fibonacci Required fields

    public int Degree {get; set;}
    public bool Mark {get; set;}

    // Meta fields
    public bool IsFibonacci {get;}
    public bool IsPairing => !IsFibonacci;

    /// <summary>
    /// Create a new node and assign value
    /// </summary>
    /// <param name="value"></param>
    public Node(T value, bool isFibonacci = true) 
    {
        Value = value;
        IsFibonacci = isFibonacci;

        if(IsFibonacci)
        {
            // Create circular list of children
            Prev = this;
            Sibling = this;
        }else
        {
            // Prev/Sibling default to null
        }
    }
}