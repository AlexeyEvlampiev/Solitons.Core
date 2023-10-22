using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Collections;

/// <summary>
/// Provides a set of static methods to create <see cref="FluentCollection{T}"/> instances.
/// </summary>
public static class FluentCollection
{
    /// <summary>
    /// Creates a new <see cref="FluentCollection{T}"/> from the specified collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection from which to create a FluentCollection.</param>
    /// <returns>A <see cref="FluentCollection{T}"/> containing the elements of the specified collection.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var list = new List<int> { 1, 2, 3 };
    /// var fluentCollection = FluentCollection.Create(list);
    /// ]]>
    /// </code>
    /// </example>
    public static FluentCollection<T> Create<T>(IEnumerable<T> collection)
    {
        if (collection is FluentCollection<T> other)
        {
            return other;
        }

        return new FluentCollection<T>(collection);
    }

    /// <summary>
    /// Creates a new <see cref="FluentCollection{T}"/> containing a single item.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="item">The item to include in the collection.</param>
    /// <returns>A <see cref="FluentCollection{T}"/> containing the specified item.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create(42);
    /// ]]>
    /// </code>
    /// </example>
    public static FluentCollection<T> Create<T>(T item) => new(FluentArray.Create(item));

    /// <summary>
    /// Creates a new <see cref="FluentCollection{T}"/> containing two items.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <param name="item1">The first item to include in the collection.</param>
    /// <param name="item2">The second item to include in the collection.</param>
    /// <returns>A <see cref="FluentCollection{T}"/> containing the specified items.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create(42, 43);
    /// ]]>
    /// </code>
    /// </example>
    public static FluentCollection<T> Create<T>(T item1, T item2) => new(FluentArray.Create(item1, item2));

    /// <summary>
    /// Creates a new <see cref="FluentCollection{T}"/> containing three items.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <param name="item1">The first item to include in the collection.</param>
    /// <param name="item2">The second item to include in the collection.</param>
    /// <param name="item3">The third item to include in the collection.</param>
    /// <returns>A <see cref="FluentCollection{T}"/> containing the specified items.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create(42, 43, 44);
    /// ]]>
    /// </code>
    /// </example>
    public static FluentCollection<T> Create<T>(T item1, T item2, T item3) => new(FluentArray.Create(item1, item2, item3));

    /// <summary>
    /// Creates a new <see cref="FluentCollection{T}"/> containing the specified items.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <param name="items">The items to include in the collection.</param>
    /// <returns>A <see cref="FluentCollection{T}"/> containing the specified items.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create(42, 43, 44, 45);
    /// ]]>
    /// </code>
    /// </example>
    public static FluentCollection<T> Create<T>(params T[] items) => new(FluentArray.Create(items));
}

/// <summary>
/// A generic collection class that provides a fluent interface for manipulating collections of items of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of item stored in the collection.</typeparam>
/// <remarks>
/// The FluentCollection class is a wrapper around a collection that provides a fluent interface for performing operations on the collection.
/// Each method returns the FluentCollection object, allowing for method chaining.
/// </remarks>
public sealed class FluentCollection<T> : ICollection<T>
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly ICollection<T> _collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCollection{T}"/> class that contains elements copied from the specified enumerable.
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
    /// Adds an element to the <see cref="FluentCollection{T}"/>.
    /// </summary>
    /// <param name="item">The item to add to the collection.</param>
    /// <returns>The <see cref="FluentCollection{T}"/> object, allowing for method chaining.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection
    ///     .Create<int>()
    ///     .Add(42);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerStepThrough]
    public FluentCollection<T> Add(T item)
    {
        _collection.Add(item);
        return this;
    }

    /// <summary>
    /// Adds the elements of the specified collection to the <see cref="FluentCollection{T}"/>.
    /// </summary>
    /// <param name="collection">The collection to add elements from.</param>
    /// <returns>The <see cref="FluentCollection{T}"/> object, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException">If the collection is null.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create<int>();
    /// fluentCollection.AddRange(new List<int> { 1, 2, 3 });
    /// ]]>
    /// </code>
    /// </example>
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
    /// Removes all elements from the <see cref="FluentCollection{T}"/>.
    /// </summary>
    /// <returns>The <see cref="FluentCollection{T}"/> object, allowing for method chaining.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create<int>(1, 2, 3);
    /// fluentCollection.Clear();
    /// ]]>
    /// </code>
    /// </example>
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
    /// Copies the elements of the <see cref="FluentCollection{T}"/> to an array, starting at a particular array index.
    /// </summary>
    /// <param name="array">The array to copy elements to.</param>
    /// <param name="arrayIndex">The index in the array to start copying elements.</param>
    /// <returns>The <see cref="FluentCollection{T}"/> object, allowing for method chaining.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create<int>(1, 2, 3);
    /// var array = new int[3];
    /// fluentCollection.CopyTo(array, 0);
    /// ]]>
    /// </code>
    /// </example>
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
    /// Removes the first occurrence of a specific object from the <see cref="FluentCollection{T}"/>.
    /// </summary>
    /// <param name="item">The item to remove from the collection.</param>
    /// <param name="removed">True if the item was removed; otherwise, false.</param>
    /// <returns>The <see cref="FluentCollection{T}"/> object, allowing for method chaining.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create<int>(1, 2, 3);
    /// bool removed;
    /// fluentCollection.Remove(2, out removed);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerStepThrough]
    public FluentCollection<T> Remove(T item, out bool removed)
    {
        removed = Remove(item);
        return this;
    }

    /// <summary>
    /// Removes all elements that match the conditions defined by the specified predicate from the <see cref="FluentCollection{T}"/>.
    /// </summary>
    /// <param name="selector">The predicate that defines the conditions of the elements to remove.</param>
    /// <returns>The <see cref="FluentCollection{T}"/> object, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException">If the selector is null.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create<int>(1, 2, 3, 4, 5);
    /// fluentCollection.Remove(item => item % 2 == 0);
    /// ]]>
    /// </code>
    /// </example>
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
    /// Removes all elements that match the conditions defined by the specified predicate from the <see cref="FluentCollection{T}"/>, and returns the number of elements removed.
    /// </summary>
    /// <param name="selector">The predicate that defines the conditions of the elements to remove.</param>
    /// <param name="removed">The number of elements removed.</param>
    /// <returns>The <see cref="FluentCollection{T}"/> object, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException">If the selector is null.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create<int>(1, 2, 3, 4, 5);
    /// int removed;
    /// fluentCollection.Remove(item => item % 2 == 0, out removed);
    /// ]]>
    /// </code>
    /// </example>
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
    /// Performs the specified action on each element of the <see cref="FluentCollection{T}"/>.
    /// </summary>
    /// <param name="action">The action to perform on each element of the collection.</param>
    /// <returns>The <see cref="FluentCollection{T}"/> object, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException">If the action is null.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create<int>(1, 2, 3);
    /// fluentCollection.ForEach(item => Console
    /// WriteLine(item));
    /// ]]>
    /// </code>
    /// </example>
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
    /// Performs the specified action on each element of the <see cref="FluentCollection{T}"/>, with an index parameter.
    /// </summary>
    /// <param name="action">The action to perform on each element of the collection.</param>
    /// <returns>The <see cref="FluentCollection{T}"/> object, allowing for method chaining.</returns>
    /// <exception cref="ArgumentNullException">If the action is null.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentCollection = FluentCollection.Create<int>(1, 2, 3);
    /// fluentCollection.ForEach((item, index) => Console.WriteLine($"{index}: {item}"));
    /// ]]>
    /// </code>
    /// </example>
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