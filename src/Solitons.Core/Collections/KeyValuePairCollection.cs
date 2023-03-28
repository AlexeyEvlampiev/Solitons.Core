﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Collections;

/// <summary>
/// Provides a set of static methods for working with instances of the KeyValuePairCollection&lt;TKey, TValue&gt; class.
/// </summary>
public static class KeyValuePairCollection
{
    /// <summary>
    /// Creates a new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class with the default constructor.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <returns>A new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class.</returns>
    public static KeyValuePairCollection<TKey, TValue> Create<TKey, TValue>() => new();

    /// <summary>
    /// Creates a new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class with the default constructor and a fixed string key type.
    /// </summary>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <returns>A new instance of the KeyValuePairCollection&lt;string, TValue&gt; class.</returns>
    public static KeyValuePairCollection<string,TValue> Create<TValue>() => new();

    /// <summary>
    /// Creates a new instance of the KeyValuePairCollection&lt;string, string&gt; class with the default constructor.
    /// </summary>
    /// <returns>A new instance of the KeyValuePairCollection&lt;string, string&gt; class.</returns>
    public static KeyValuePairCollection<string, string> Create() => new();

    /// <summary>
    /// Creates a new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class with the default constructor, adds a key-value pair to it, and returns the new collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <param name="key">The key of the key-value pair to add to the collection.</param>
    /// <param name="value">The value of the key-value pair to add to the collection.</param>
    /// <returns>A new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class with the specified key-value pair added to it.</returns>
    public static KeyValuePairCollection<TKey, TValue> Add<TKey, TValue>(TKey key, TValue value)
    {
        var result = Create<TKey,TValue>();
        result.Add(KeyValuePair.Create(key, value));
        return result;
    }

    /// <summary>
    /// Creates a new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class with the default constructor and initializes it with a collection of key-value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <param name="collection">The collection of key-value pairs to initialize the new instance with.</param>
    /// <returns>A new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class initialized with the specified collection of key-value pairs.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the specified collection is null.</exception>
    public static KeyValuePairCollection<TKey, TValue> Create<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection) => 
        new(ThrowIf.ArgumentNull(collection, nameof(collection)).ToList());

    /// <summary>
    /// Wraps an existing collection of key-value pairs in a new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class, or returns the existing collection if it is already an instance of KeyValuePairCollection&lt;TKey, TValue&gt;.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <param name="collection">The collection of key-value pairs to wrap.</param>
    /// <returns>A new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class that wraps the specified collection, or the existing collection if it is already an instance of KeyValuePairCollection&lt;TKey, TValue&gt;.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the specified collection is null.</exception>
    public static KeyValuePairCollection<TKey, TValue> Wrap<TKey, TValue>(
        ICollection<KeyValuePair<TKey, TValue>> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return collection is KeyValuePairCollection<TKey, TValue> other
            ? other
            : new KeyValuePairCollection<TKey, TValue>(collection);
    }

    /// <summary>
    /// Wraps an existing collection of key-value pairs in a new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class, or creates a new collection if the specified collection is null or not already an instance of KeyValuePairCollection&lt;TKey, TValue&gt;.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <param name="collection">The collection of key-value pairs to wrap, or null to create a new collection.</param>
    /// <returns>A new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class that wraps the specified collection, or a new empty collection if the specified collection is null or not already an instance of KeyValuePairCollection&lt;TKey, TValue&gt;.</returns>
    public static KeyValuePairCollection<TKey, TValue> WrapOrCreate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return collection is KeyValuePairCollection<TKey, TValue> other
            ? other
            : new KeyValuePairCollection<TKey, TValue>(collection);
    }
}


/// <summary>
/// Represents a strongly typed collection of key-value pairs that can be accessed by index. Provides methods to manipulate the collection.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the values in the collection.</typeparam>
public class KeyValuePairCollection<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly ICollection<KeyValuePair<TKey, TValue>> _collection;

    /// <summary>
    /// Initializes a new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class that is empty and has the default initial capacity.
    /// </summary>
    [DebuggerStepThrough]
    internal KeyValuePairCollection()
    {
        _collection = new List<KeyValuePair<TKey, TValue>>();
    }

    /// <summary>
    /// Initializes a new instance of the KeyValuePairCollection&lt;TKey, TValue&gt; class that contains elements copied from the specified enumerable.
    /// </summary>
    /// <param name="enumerable">The enumerable whose elements are copied to the new collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when the specified enumerable is null.</exception>
    [DebuggerStepThrough]
    internal KeyValuePairCollection(IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
    {
        if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
        if (enumerable is KeyValuePairCollection<TKey, TValue> other)
        {
            _collection = other._collection;
        }
        else if(enumerable is ICollection<KeyValuePair<TKey, TValue>> collection)
        {
            _collection = collection;
        }
        else
        {
            _collection = new List<KeyValuePair<TKey, TValue>>(enumerable);
        }
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _collection.GetEnumerator();

    /// <inheritdoc/>
    [DebuggerStepThrough]
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_collection).GetEnumerator();

    /// <inheritdoc/>
    [DebuggerStepThrough]
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => _collection.Add(item);

    /// <summary>
    /// Adds an item to the collection and returns the collection for method chaining.
    /// </summary>
    /// <param name="item">The item to add to the collection.</param>
    /// <returns>The collection after the item has been added.</returns>
    [DebuggerStepThrough]
    public KeyValuePairCollection<TKey, TValue> Add(KeyValuePair<TKey, TValue> item)
    {
        _collection.Add(item);
        return this;
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    void ICollection<KeyValuePair<TKey, TValue>>.Clear() => _collection.Clear();

    /// <summary>
    /// Removes all items from the collection and returns the collection for method chaining.
    /// </summary>
    /// <returns>The collection after all items have been removed.</returns>
    [DebuggerStepThrough]
    public KeyValuePairCollection<TKey, TValue> Clear()
    {
        _collection.Clear();
        return this;
    }

    /// <summary>
    /// Removes all items from the collection and returns the collection for method chaining.
    /// </summary>
    /// <returns>The collection after all items have been removed.</returns>
    [DebuggerStepThrough]
    public bool Contains(KeyValuePair<TKey, TValue> item) => _collection.Contains(item);

    /// <summary>
    /// Copies the elements of the collection to an array, starting at a particular array index.
    /// </summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from the collection.</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    [DebuggerStepThrough]
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);


    /// <summary>
    /// Removes the first occurrence of a specific item from the collection.
    /// </summary>
    /// <param name="item">The item to remove from the collection.</param>
    /// <returns>true if the item was successfully removed from the collection; otherwise, false.</returns>
    [DebuggerStepThrough]
    public bool Remove(KeyValuePair<TKey, TValue> item) => _collection.Remove(item);

    /// <summary>
    /// Removes the first occurrence of a specific item from the collection and returns the collection for method chaining.
    /// </summary>
    /// <param name="item">The item to remove from the collection.</param>
    /// <param name="removed">When this method returns, contains true if the item was successfully removed from the collection; otherwise, false.</param>
    /// <returns>The collection after the item has been removed.</returns>
    [DebuggerStepThrough]
    public KeyValuePairCollection<TKey, TValue> Remove(KeyValuePair<TKey, TValue> item, out bool removed)
    {
        removed = _collection.Remove(item);
        return this;
    }

    /// <inheritdoc />
    public int Count => _collection.Count;

    /// <inheritdoc />
    public bool IsReadOnly => _collection.IsReadOnly;
}