using System;
using System.Collections.Generic;

namespace Implementation.Heaps;

/// <summary>
/// A Fibonacci heap implementation.
/// <para>
/// Has functions for insert, extract-min, decrease-key, and find-min.
/// </para>
/// Author: Christian Lindner, calindner@crimson.ua.edu
/// </summary>
/// <typeparam name="T">Element type; must implement IComparable of T</typeparam>
public class FibonacciHeap<T> : IHeap<T> where T : IComparable<T>
{
    /// <summary>
    /// The current heap's minimum root node; null if heap is empty.
    /// </summary>
    private Node<T>? min;

    /// <summary>
    /// Create a new Fibonacci heap with no roots.
    /// </summary>
    public FibonacciHeap()
    {
        min = null;
    }

    /// <summary>
    /// Add a node to the root list and update min if needed.
    /// </summary>
    /// <param name="node">Node to add as a root</param>
    private void AddRoot(Node<T> node)
    {
        if (min == null)
        {
            min = node;
            node.Prev = node;
            node.Sibling = node;
            return;
        }
        // Splice node into root list between min and min.Sibling
        node.Prev = min;
        node.Sibling = min.Sibling;
        min.Sibling!.Prev = node;
        min.Sibling = node;
        if (node.Value.CompareTo(min.Value) < 0)
            min = node;
    }

    /// <summary>
    /// Remove a node from its circular doubly-linked list (root or child list).
    /// </summary>
    /// <param name="node">Node to remove from the list</param>
    private static void RemoveFromList(Node<T> node)
    {
        node.Prev!.Sibling = node.Sibling;
        node.Sibling!.Prev = node.Prev;
    }

    /// <summary>
    /// Make child a child of parent in the Fibonacci heap (insert into parent's child list).
    /// </summary>
    /// <param name="parent">Parent node</param>
    /// <param name="child">Child node to attach</param>
    private static void AddChild(Node<T> parent, Node<T> child)
    {
        child.Parent = parent;
        child.Mark = false;
        parent.Degree++;
        if (parent.Child == null)
        {
            parent.Child = child;
            child.Prev = child;
            child.Sibling = child;
            return;
        }
        // Splice child into circular list at parent.Child
        child.Prev = parent.Child;
        child.Sibling = parent.Child.Sibling;
        parent.Child.Sibling!.Prev = child;
        parent.Child.Sibling = child;
    }

    /// <summary>
    /// Insert a new value into the heap and rebuild as needed.
    /// </summary>
    /// <param name="value">Value to add</param>
    /// <returns>The new node (for use with DecreaseKey)</returns>
    public Node<T> Insert(T value)
    {
        MetricsHandler.StartHeapOperation(MetricsHandler.OpType.Insert);
        var node = new Node<T>(value, isFibonacci: true);
        AddRoot(node);
        MetricsHandler.EndHeapOperation();
        return node;
    }

    /// <summary>
    /// Return the node with minimum key, or null if the heap is empty.
    /// </summary>
    /// <returns>The heap's minimum node, or null if empty</returns>
    public T? FindMin()
    {
        if (min == null)
            return default;
        
        return min.Value;
    }

    /// <summary>
    /// Link two roots: the one with larger key becomes a child of the one with smaller key.
    /// </summary>
    /// <param name="a">First root node</param>
    /// <param name="b">Second root node</param>
    /// <returns>The root of the merged tree (the smaller of the two)</returns>
    private static Node<T> Link(Node<T> a, Node<T> b)
    {
        if (a.Value.CompareTo(b.Value) > 0)
            (a, b) = (b, a);
        AddChild(a, b);
        return a;
    }

    /// <summary>
    /// Remove the minimum value from the heap and return it.
    /// </summary>
    /// <returns>The heap's minimum node</returns>
    /// <exception cref="InvalidOperationException">If the heap is empty</exception>
    public T ExtractMin()
    {
        if (min == null)
            throw new InvalidOperationException("Heap is empty.");

        MetricsHandler.StartHeapOperation(MetricsHandler.OpType.ExtractMin);
        var oldMin = min;
        RemoveFromList(min);

        // If min was the only root, list is now empty; otherwise remaining roots start at oldMin.Sibling
        if (min.Sibling == min)
            min = null;
        else
            min = min.Sibling;

        // Add all children of oldMin to the root list
        var child = oldMin.Child;
        if (child != null)
        {
            var cur = child;
            do
            {
                var next = cur.Sibling;
                cur.Parent = null;
                cur.Prev = cur;
                cur.Sibling = cur;
                AddRoot(cur);
                cur = next;
            } while (cur != child);
            oldMin.Child = null;
            oldMin.Degree = 0;
        }

        if (min == null) {
            MetricsHandler.EndHeapOperation();
            return oldMin.Value;
        }

        // First pass: collect all roots
        var roots = new List<Node<T>>();
        var start = min;
        var current = min;
        do
        {
            roots.Add(current);
            current = current.Sibling!;
        } while (current != start);

        // Second pass: consolidate roots of the same degree
        var byDegree = new List<Node<T>?>();
        foreach (var r in roots)
        {
            // Skip nodes that were linked as children during this consolidation
            if (r.Parent != null)
                continue;
            var x = r;
            while (byDegree.Count <= x.Degree)
                byDegree.Add(null);
            while (byDegree[x.Degree] != null)
            {
                var other = byDegree[x.Degree];
                byDegree[x.Degree] = null;
                x = Link(x, other!);
                while (byDegree.Count <= x.Degree)
                    byDegree.Add(null);
            }
            byDegree[x.Degree] = x;
        }

        // Rebuild root list and find new min
        min = null;
        foreach (var r in byDegree)
        {
            if (r != null)
                AddRoot(r);
        }

        MetricsHandler.EndHeapOperation();
        return oldMin.Value;
    }

    /// <summary>
    /// Update the value of the node passed and rebuild the heap if needed.
    /// </summary>
    /// <param name="updateNode">Node to update</param>
    /// <param name="newValue">Value to update to</param>
    /// <exception cref="InvalidOperationException">If newValue is greater than the current value</exception>
    public void DecreaseKey(Node<T> updateNode, T newValue)
    {
        if (newValue.CompareTo(updateNode.Value) > 0)
            throw new InvalidOperationException("DecreaseKey requires new value to be less than or equal to current value.");

        MetricsHandler.StartHeapOperation(MetricsHandler.OpType.DecreaseKey);

        updateNode.Value = newValue;

        if (updateNode.Parent == null)
        {
            // Already root, update min if root is smaller
            if (min != null && updateNode.Value.CompareTo(min.Value) < 0)
                min = updateNode;
            MetricsHandler.EndHeapOperation();
            return;
        }

        if (updateNode.Value.CompareTo(updateNode.Parent.Value) >= 0) {
            MetricsHandler.EndHeapOperation();
            return;
        }

        // Heap violated: cut node from parent and add to root list; cascading cut if parent marked
        Cut(updateNode);
        MetricsHandler.EndHeapOperation();
    }

    /// <summary>
    /// Cut node from its parent and add to root list; perform cascading cut on parent if marked.
    /// </summary>
    /// <param name="node">Node to cut from its parent</param>
    private void Cut(Node<T> node)
    {
        var parent = node.Parent!;
        RemoveFromList(node);

        if (parent.Child == node)
            parent.Child = node.Sibling != node ? node.Sibling : null;
        parent.Degree--;
        node.Parent = null;
        node.Mark = false;

        AddRoot(node);
        if (min != null && node.Value.CompareTo(min.Value) < 0)
            min = node;

        if (parent.Parent == null)
            return; // parent is root, done

        if (parent.Mark)
            Cut(parent);
        else
            parent.Mark = true;
    }
}
