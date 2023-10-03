using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Solitons.Collections.Common;

/// <summary>
/// Represents a base class for a proxy to a dictionary that provides an explicit interface to the wrapped dictionary.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
public abstract class DictionaryExplicitProxy<TKey, TValue> : IDictionary<TKey, TValue>
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly IDictionary<TKey, TValue> _innerDictionary;

    /// <summary>
    /// Initializes a new instance of the <see cref="DictionaryExplicitProxy{TKey, TValue}"/> class that wraps the specified dictionary.
    /// </summary>
    /// <param name="innerDictionary">The dictionary to wrap.</param>
    [DebuggerStepThrough]
    protected DictionaryExplicitProxy(IDictionary<TKey, TValue> innerDictionary)
    {
        _innerDictionary = innerDictionary;
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
        return _innerDictionary.GetEnumerator();
    }

    [DebuggerStepThrough]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_innerDictionary).GetEnumerator();
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        _innerDictionary.Add(item);
    }

    [DebuggerStepThrough]
    void ICollection<KeyValuePair<TKey, TValue>>.Clear()
    {
        _innerDictionary.Clear();
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        return _innerDictionary.Contains(item);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        _innerDictionary.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        return _innerDictionary.Remove(item);
    }

    /// <inheritdoc/>
    int ICollection<KeyValuePair<TKey, TValue>>.Count => _innerDictionary.Count;

    /// <inheritdoc/>
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => _innerDictionary.IsReadOnly;

    [DebuggerStepThrough]
    void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
    {
        _innerDictionary.Add(key, value);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    bool IDictionary<TKey, TValue>.ContainsKey(TKey key)
    {
        return _innerDictionary.ContainsKey(key);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    bool IDictionary<TKey, TValue>.Remove(TKey key)
    {
        return _innerDictionary.Remove(key);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    bool IDictionary<TKey, TValue>.TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _innerDictionary.TryGetValue(key, out value);
    }

    /// <inheritdoc/>
    TValue IDictionary<TKey, TValue>.this[TKey key]
    {
        [DebuggerStepThrough]
        get => _innerDictionary[key];
        [DebuggerStepThrough]
        set => _innerDictionary[key] = value;
    }

    /// <inheritdoc/>
    ICollection<TKey> IDictionary<TKey, TValue>.Keys => _innerDictionary.Keys;

    /// <inheritdoc/>
    ICollection<TValue> IDictionary<TKey, TValue>.Values => _innerDictionary.Values;

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override int GetHashCode() => _innerDictionary.GetHashCode();

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(obj, null)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (_innerDictionary.Equals(obj)) return true;
        if (obj is DictionaryExplicitProxy<TKey, TValue> other)
        {
            return _innerDictionary.Equals(other._innerDictionary);
        }
        return false;
    }
}