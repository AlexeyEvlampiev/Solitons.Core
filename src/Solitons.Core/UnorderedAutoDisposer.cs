using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons;

/// <summary>
/// Manages a collection of IDisposable resources.
/// Disposes the items in no particular order when this instance is disposed.
/// </summary>
public class UnorderedAutoDisposer : IDisposable
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly HashSet<IDisposable> _set = new HashSet<IDisposable>();

    /// <summary>
    /// Adds an IDisposable item to the collection.
    /// </summary>
    /// <param name="item">The IDisposable item to be added to the collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when the item is null.</exception>
    public void Add(IDisposable item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _set.Add(item);
    }

    /// <summary>
    /// Disposes all items in the collection in no particular order.
    /// </summary>
    /// <remarks>
    /// Call Dispose when you are finished using the UnorderedAutoDisposer. The Dispose method leaves the UnorderedAutoDisposer in an unusable state. 
    /// After calling Dispose, you must release all references to the UnorderedAutoDisposer so the garbage collector can reclaim the memory that 
    /// the UnorderedAutoDisposer was occupying.
    /// </remarks>
    public void Dispose()
    {
        foreach (var item in _set)
        {
            item.Dispose();
        }
        _set.Clear();
    }
}