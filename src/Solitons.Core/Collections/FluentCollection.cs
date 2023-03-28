using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Collections;

/// <summary>
/// A generic collection class that provides a fluent interface for manipulating collections of items of type T.
/// </summary>
/// <typeparam name="T">The type of item stored in the collection.</typeparam>
public sealed class FluentCollection<T> : ICollection<T>
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly ICollection<T> _collection;

    /// <summary>
    /// Initializes a new instance of the FluentCollection class that contains elements copied from the specified enumerable.
    /// </summary>
    /// <param name="enumerable">The enumerable to copy elements from.</param>
    /// <exception cref="ArgumentNullException">If the enumerable is null.</exception>
    [DebuggerNonUserCode]
    internal FluentCollection(IEnumerable<T> enumerable)
    {
        if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
        _collection = enumerable is ICollection<T> collection
            ? collection
            : enumerable.ToList();
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

    /// <inheritdoc/>
    [DebuggerStepThrough]
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_collection).GetEnumerator();

    /// <inheritdoc/>
    [DebuggerStepThrough]
    void ICollection<T>.Add(T item) => _collection.Add(item);

    /// <summary>
    /// Adds an element to the FluentCollection.
    /// </summary>
    /// <param name="item">The item to add to the collection.</param>
    /// <returns>The FluentCollection object, allowing for method chaining.</returns>
    [DebuggerStepThrough]
    public FluentCollection<T> Add(T item)
    {
        _collection.Add(item);
        return this;
    }

    /// <summary>
    /// Adds the elements of the specified collection to the FluentCollection.
    /// </summary>
    /// <param name="collection">The collection to add elements from.</param>
    /// <returns>The FluentCollection object, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException">If the collection is null.</exception>
    [DebuggerStepThrough]
    public FluentCollection<T> AddRange(IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        foreach (var item in collection)
        {
            _collection.Add(item);
        }

        return this;
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    void ICollection<T>.Clear() => _collection.Clear();


    /// <summary>
    /// Removes all elements from the FluentCollection.
    /// </summary>
    /// <returns>The FluentCollection object, allowing for method chaining.</returns>
    [DebuggerStepThrough]
    public FluentCollection<T> Clear()
    {
        _collection.Clear();
        return this;
    }


    /// <inheritdoc/>
    [DebuggerStepThrough]
    public bool Contains(T item) => _collection.Contains(item);

    /// <inheritdoc/>
    [DebuggerStepThrough]
    void ICollection<T>.CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

    /// <summary>
    /// Copies the elements of the FluentCollection to an array, starting at a particular array index.
    /// </summary>
    /// <param name="array">The array to copy elements to.</param>
    /// <param name="arrayIndex">The index in the array to start copying elements.</param>
    /// <returns>The FluentCollection object, allowing for method chaining.</returns>
    [DebuggerStepThrough]
    public FluentCollection<T> CopyTo(T[] array, int arrayIndex)
    {
        _collection.CopyTo(array, arrayIndex);
        return this;
    }


    /// <inheritdoc/>
    [DebuggerStepThrough]
    public bool Remove(T item) => _collection.Remove(item);

    /// <summary>
    /// Removes the first occurrence of a specific object from the FluentCollection.
    /// </summary>
    /// <param name="item">The item to remove from the collection.</param>
    /// <param name="removed">True if the item was removed; otherwise, false.</param>
    /// <returns>The FluentCollection object, allowing for method chaining.</returns>
    [DebuggerStepThrough]
    public FluentCollection<T> Remove(T item, out bool removed)
    {
        removed = Remove(item);
        return this;
    }

    /// <summary>
    /// Removes all elements that match the conditions defined by the specified predicate from the FluentCollection.
    /// </summary>
    /// <param name="selector">The predicate that defines the conditions of the elements to remove.</param>
    /// <returns>The FluentCollection object, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException">If the selector is null.</exception>
    [DebuggerStepThrough]
    public FluentCollection<T> Remove(Func<T, bool> selector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        foreach (var item in _collection.Where(selector).ToList())
        {
            _collection.Remove(item);
        }
        return this;
    }

    /// <summary>
    /// Removes all elements that match the conditions defined by the specified predicate from the FluentCollection, and returns the number of elements removed.
    /// </summary>
    /// <param name="selector">The predicate that defines the conditions of the elements to remove.</param>
    /// <param name="removed">The number of elements removed.</param>
    /// <returns>The FluentCollection object, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException">If the selector is null.</exception>
    [DebuggerStepThrough]
    public FluentCollection<T> Remove(Func<T, bool> selector, out int removed)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        removed = 0;
        foreach (var item in _collection.Where(selector).ToList())
        {
            removed++;
            _collection.Remove(item);
        }
        return this;
    }

    /// <inheritdoc/>
    [DebuggerNonUserCode]
    public int Count => _collection.Count;

    /// <inheritdoc/>
    [DebuggerNonUserCode]
    public bool IsReadOnly => _collection.IsReadOnly;

    /// <summary>
    /// Performs the specified action on each element of the FluentCollection.
    /// </summary>
    /// <param name="action">The action to perform on each element of the collection.</param>
    /// <returns>The FluentCollection object, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException">If the action is null.</exception>
    [DebuggerStepThrough]
    public FluentCollection<T> ForEach(Action<T> action)
    {
        ThrowIf.ArgumentNull(action);
        foreach (var item in _collection)
        {
            action.Invoke(item);
        }

        return this;
    }

    /// <summary>
    /// Performs the specified action on each element of the FluentCollection, with an index parameter.
    /// </summary>
    /// <param name="action">The action to perform on each element of the collection.</param>
    /// <returns>The FluentCollection object, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException">If the action is null.</exception>
    [DebuggerStepThrough]
    public FluentCollection<T> ForEach(Action<T, int> action)
    {
        ThrowIf.ArgumentNull(action);
        int index = 0;
        foreach (var item in _collection)
        {
            action.Invoke(item, index++);
        }

        return this;
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override int GetHashCode() => _collection.GetHashCode();

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (ReferenceEquals(_collection, obj)) return true;
        if (ReferenceEquals(null, obj)) return false;
        return _collection.Equals(obj);
    }
}