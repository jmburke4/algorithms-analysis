using System.Collections.Generic;
using System.Linq;
using System;

namespace Implementation.Heaps;

/// <summary>
/// A pairing heap implementation.
/// <para>
/// Has functions for insert, extract-min, decrease-key, and find-min.
/// </para>
/// 
/// Author: Brock Kitterman - bfkitterman@crimson.ua.edu
/// </summary>
public class PairingHeap<T> : IHeap<T> where T : IComparable<T>
{
    /// <summary>
    /// The current heap's root node.
    /// </summary>
    public Node<T>? Root {get; private set;}

    /// <summary>
    /// Create a new pairing heap with root node = null
    /// </summary>
    public PairingHeap()
    {
        Root = null;
    }

    /// <summary>
    /// Create a new pairing heap with root node
    /// </summary>
    /// <param name="initialValue">Initial Value for the heap</param>
    public PairingHeap(T initialValue)
    {
        Root = new Node<T>(initialValue, false);
    }

    /// <summary>
    /// Merge two heap roots into one.
    /// </summary>
    /// <param name="a">Node A</param>
    /// <param name="b">Node B</param>
    /// <returns>The updated merge</returns>
    private Node<T> merge(Node<T> a, Node<T> b)
    {
        if(a == null) return b;
        if(b == null) return a;   

        if(a.Value.CompareTo(b.Value) < 0)
        {
            // A becomes parent of B
            b.Sibling = a.Child;
            if(a.Child != null) a.Child.Prev = b;
            a.Child = b;
            b.Parent = a;
            b.Prev = null;
            return a;
        } else
        {
            // B becomes parent of A
            a.Sibling = b.Child;
            if(b.Child != null) b.Child.Prev = a;
            b.Child = a;
            a.Parent = b;
            a.Prev = null;
            return b;
        }
    }

    /// <summary>
    /// Insert a new value into the heap and rebuild as needed
    /// </summary>
    /// <param name="newValue">Value to add</param>
    /// <returns>The updated heap</returns>
    public Node<T> Insert(T newValue)
    {
        MetricsHandler.StartHeapOperation(MetricsHandler.OpType.Insert);
        Node<T> newNode = new Node<T>(newValue, false);
        Root = merge(Root, newNode);
        MetricsHandler.EndHeapOperation();
        return Root;
    }

    /// <summary>
    /// Remove the minimum value from the heap and return it
    /// </summary>
    /// <returns>The heap's minimum value</returns>
    /// <exception cref="Exception">If root is null, throw exception</exception>
    public T ExtractMin()
    {
        if(Root == null) throw new InvalidOperationException("Root node is null and heap is empty");
        MetricsHandler.StartHeapOperation(MetricsHandler.OpType.ExtractMin);

        Node<T> oldRoot = Root;
        Node<T> currentNode = Root.Child;

        // Only root in tree
        if(currentNode == null) 
        {
            Root = null;
            MetricsHandler.EndHeapOperation();
            return oldRoot.Value; // Empty heap
        }

        // First pass: Pair up siblings
        List<Node<T>> pairs = [];
        while(currentNode != null)
        {
            Node<T> a = currentNode;
            Node<T> b = currentNode.Sibling;

            if(b != null)
            {
                Node<T> nextSibling = b.Sibling;
                a.Sibling = null;
                a.Prev = null;

                b.Sibling = null;
                b.Prev = null;

                pairs.Add(merge(a,b));

                currentNode = nextSibling;
            } else
            {
                // Odd number of siblings
                pairs.Add(a);
                currentNode = null;
            }
        }

        // Second Pass: Accumulate from right to left
        Node<T> result = pairs.Last();

        for(int i = pairs.Count - 2; i >= 0; i--)
            result = merge(pairs[i], result);
        
        Root = result;
        Root.Parent = null;
        MetricsHandler.EndHeapOperation();
        return oldRoot.Value;
    }

    public T? FindMin()
    {
        if (Root == null)
            return default;
        
        return Root.Value;
    }

    /// <summary>
    /// Update the value of the node passed and rebuild the heap if needed.
    /// </summary>
    /// <param name="updateNode">Node to update</param>
    /// <param name="newValue">Value to update to</param>
    /// <returns>The updated heap</returns>
    public void DecreaseKey(Node<T> updateNode, T newValue)
    {
        if(updateNode.Value.CompareTo(newValue) < 0) throw new InvalidOperationException(
            "Error: Cannot decrease key of pairing heap if new value is larger than previous value.\nPassed: " + newValue + 
            "\nPrevious Value: " + 
            updateNode.Value);
        MetricsHandler.StartHeapOperation(MetricsHandler.OpType.DecreaseKey);

        updateNode.Value = newValue;

        // Ensure update isn't root
        if(updateNode == Root)
        {
            MetricsHandler.EndHeapOperation();
            return;
        }

        // Double check to ensure no further issues
        if (updateNode.Parent == null)
        {
            MetricsHandler.EndHeapOperation();
            return; 
        }

        // Check if heap is violated
        if(updateNode.Value.CompareTo(updateNode.Parent.Value) < 0)
        {
            // If not first child, link siblings
            // Else, link next sibling to parent
            if(updateNode.Prev != null) updateNode.Prev.Sibling = updateNode.Sibling;
            else updateNode.Parent.Child = updateNode.Sibling;

            // Updated prev if needed
            if(updateNode.Sibling != null) updateNode.Sibling.Prev = updateNode.Prev;

            // Cut node out
            updateNode.Sibling = null;
            updateNode.Prev = null;
            updateNode.Parent = null;

            // Merge and return as root
            Root = merge(Root, updateNode);
        }
        MetricsHandler.EndHeapOperation();
    }
}
