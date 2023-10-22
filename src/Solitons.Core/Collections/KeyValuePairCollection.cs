using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solitons.Collections;

/// <summary>
/// Provides a set of static (Shared in Visual Basic) methods for working with instances of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class.
/// </summary>
/// <example>
/// <code>
/// <![CDATA[
/// var collection = KeyValuePairCollection.Create<string, int>();
/// collection = KeyValuePairCollection.Add("key1", 1);
/// foreach (var kvp in collection)
/// {
///     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
/// }
/// ]]>
/// </code>
/// </example>
public static class KeyValuePairCollection
{
    /// <summary>
    /// Creates a new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class using the default constructor.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <returns>A new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var collection = KeyValuePairCollection.Create<int, string>();
    /// ]]>
    /// </code>
    /// </example>
    public static KeyValuePairCollection<TKey, TValue> Create<TKey, TValue>() => new();

    /// <summary>
    /// Creates a new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class with the default constructor and a fixed string key type.
    /// </summary>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <returns>A new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class where TKey is <see cref="string"/>.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var stringIntCollection = KeyValuePairCollection.Create<int>();
    /// var stringStringCollection = KeyValuePairCollection.Create<string>();
    /// ]]>
    /// </code>
    /// </example>
    public static KeyValuePairCollection<string,TValue> Create<TValue>() => new();

    /// <summary>
    /// Creates a new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class with the default constructor, specifically for a collection of string key-value pairs.
    /// </summary>
    /// <returns>A new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class where TKey and TValue are of type <see cref="string"/>.</returns>
    /// <example>
    /// This example demonstrates how to use the <see cref="Create"/> method to create a new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class where TKey and TValue are of type &lt;see cref="string"/&gt;.
    /// <code>
    /// <![CDATA[
    /// var stringPairCollection = KeyValuePairCollection.Create();
    /// ]]>
    /// </code>
    /// </example>
    public static KeyValuePairCollection<string, string> Create() => new();

    /// <summary>
    /// Creates a new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class, adds a key-value pair to it, and returns the new collection.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <param name="key">The key of the key-value pair to add to the collection.</param>
    /// <param name="value">The value of the key-value pair to add to the collection.</param>
    /// <returns>A new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class with the specified key-value pair added to it.</returns>
    /// <example>
    /// The following example demonstrates how to use the <see cref="Add{TKey,TValue}"/> method to create a new <see cref="KeyValuePairCollection{TKey,TValue}"/> instance and add a key-value pair to it.
    /// <code>
    /// <![CDATA[
    /// var collection = KeyValuePairCollection.Add("Name", "John Doe");
    /// ]]>
    /// </code>
    /// </example>
    public static KeyValuePairCollection<TKey, TValue> Add<TKey, TValue>(TKey key, TValue value)
    {
        var result = Create<TKey,TValue>();
        result.Add(KeyValuePair.Create(key, value));
        return result;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class initialized with a collection of key-value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <param name="collection">The collection of key-value pairs to initialize the new instance with.</param>
    /// <returns>
    /// A new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class initialized with the specified collection of key-value pairs.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the specified collection is null.</exception>
    /// <example>
    /// This example demonstrates how to use the <see cref="Create{TKey,TValue}(IEnumerable{KeyValuePair{TKey,TValue}})"/> method to create a new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class initialized with a collection of key-value pairs.
    /// <code>
    /// <![CDATA[
    /// var existingPairs = new List<KeyValuePair<string, int>>
    /// {
    ///     new KeyValuePair<string, int>("one", 1),
    ///     new KeyValuePair<string, int>("two", 2),
    ///     new KeyValuePair<string, int>("three", 3)
    /// };
    ///
    /// var pairCollection = KeyValuePairCollection.Create(existingPairs);
    /// ]]>
    /// </code>
    /// </example>
    public static KeyValuePairCollection<TKey, TValue> Create<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection) => 
        new(ThrowIf.ArgumentNull(collection, nameof(collection)).ToList());

    /// <summary>
    /// Wraps an existing collection of key-value pairs in a new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class, 
    /// or returns the existing collection if it is already an instance of <see cref="KeyValuePairCollection{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <param name="collection">The collection of key-value pairs to wrap.</param>
    /// <returns>A new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class that wraps the specified collection,
    /// or the existing collection if it is already an instance of <see cref="KeyValuePairCollection{TKey,TValue}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the specified collection is null.</exception>
    /// <example>
    /// This example demonstrates how to use the <see cref="Wrap{TKey,TValue}"/> method to wrap an existing collection of key-value pairs.
    /// <code>
    /// <![CDATA[
    /// var existingCollection = new List<KeyValuePair<string, int>>
    /// {
    ///     new KeyValuePair<string, int>("key1", 1),
    ///     new KeyValuePair<string, int>("key2", 2)
    /// };
    ///
    /// var wrappedCollection = KeyValuePairCollection.Wrap(existingCollection);
    /// ]]>
    /// </code>
    /// </example>
    public static KeyValuePairCollection<TKey, TValue> Wrap<TKey, TValue>(
        ICollection<KeyValuePair<TKey, TValue>> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return collection is KeyValuePairCollection<TKey, TValue> other
            ? other
            : new KeyValuePairCollection<TKey, TValue>(collection);
    }

    /// <summary>
    /// Wraps an existing collection of key-value pairs in a new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class, 
    /// or creates a new collection if the specified collection is null or not already an instance of <see cref="KeyValuePairCollection{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TValue">The type of the values in the collection.</typeparam>
    /// <param name="collection">The collection of key-value pairs to wrap, or null to create a new collection.</param>
    /// <returns>
    /// A new instance of the <see cref="KeyValuePairCollection{TKey,TValue}"/> class that wraps the specified collection, 
    /// or a new empty collection if the specified collection is null or not already an instance of <see cref="KeyValuePairCollection{TKey,TValue}"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the specified collection is null.</exception>
    /// <example>
    /// This example demonstrates how to use the <see cref="WrapOrCreate{TKey,TValue}"/> method to either wrap an existing collection 
    /// or create a new one if the existing collection is null.
    /// <code>
    /// <![CDATA[
    /// // Existing collection
    /// var existingCollection = new List<KeyValuePair<int, string>>() 
    /// { 
    ///     new KeyValuePair<int, string>(1, "One"), 
    ///     new KeyValuePair<int, string>(2, "Two") 
    /// };
    /// 
    /// // Wrap existing collection or create new if null
    /// var kvpCollection = KeyValuePairCollection.WrapOrCreate(existingCollection);
    /// 
    /// // Now kvpCollection is a KeyValuePairCollection<int, string> wrapping the existing collection
    /// 
    /// // Create a new collection when the existing collection is null
    /// IEnumerable<KeyValuePair<int, string>> nullCollection = null;
    /// var newKvpCollection = KeyValuePairCollection.WrapOrCreate(nullCollection);
    /// 
    /// // Now newKvpCollection is a new empty KeyValuePairCollection<int, string>
    /// ]]>
    /// </code>
    /// </example>
    public static KeyValuePairCollection<TKey, TValue> WrapOrCreate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return collection is KeyValuePairCollection<TKey, TValue> other
            ? other
            : new KeyValuePairCollection<TKey, TValue>(collection);
    }

    /// <summary>
    /// Parses a string in CSV format into a collection of key-value pairs.
    /// </summary>
    /// <param name="csvLine">The input CSV string.</param>
    /// <param name="delimiter">The delimiter character used to separate fields in the CSV string (default is ',').</param>
    /// <returns>A <see cref="KeyValuePairCollection{TKey,TValue}"/> containing the parsed key-value pairs.</returns>
    /// <exception cref="FormatException">Thrown when an individual key-value pair in the CSV string is not in the format 'key=value'.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var csvLine = "key1=value1,key2=value2";
    /// var collection = KeyValuePairCollection.ParseCsvLine(csvLine);
    /// foreach(var kvp in collection)
    /// {
    ///     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    /// }
    /// // Output:
    /// // key1: value1
    /// // key2: value2
    /// ]]>
    /// </code>
    /// </example>
    public static KeyValuePairCollection<string, string> ParseCsvLine(string csvLine, char delimiter = ',')
    {
        var equationRegex = new Regex(@"^(?<key>[^=]+)=(?<value>.+)$");
        var pairs = Regex
            .Split(csvLine, $@"\s*[{delimiter}]\s*(?=[^=\s]+\s*[=]\s*[^=\s{delimiter}]+)")
            .Where(equation => equation.IsPrintable())
            .Select(equation =>
            {
                var match = equationRegex.Match(equation.Trim());
                if (match.Success)
                {
                    var (key, value) = (
                        match.Groups["key"].Value.Trim(),
                        match.Groups["value"].Value.Trim());
                    return KeyValuePair.Create(key, value);
                }

                throw new FormatException($"Input string '{equation}' is not in the format 'key=value'");
            });
        return KeyValuePairCollection.Create(pairs);
    }
}


/// <summary>
/// Represents a strongly typed collection of key-value pairs that can be accessed by index. Provides methods to manipulate the collection.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
/// <typeparam name="TValue">The type of the values in the collection.</typeparam>
/// <example>
/// <code>
/// <![CDATA[
/// var kvpCollection = new KeyValuePairCollection<string, int>();
/// kvpCollection.Add(new KeyValuePair<string, int>("One", 1))
///               .Add(new KeyValuePair<string, int>("Two", 2));
/// foreach(var kvp in kvpCollection)
/// {
///     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
/// }
/// ]]>
/// </code>
/// </example>
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
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var kvpCollection = new KeyValuePairCollection<string, int>();
    /// kvpCollection.Add(new KeyValuePair<string, int>("Three", 3));
    /// ]]>
    /// </code>
    /// </example>
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
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var kvpCollection = new KeyValuePairCollection<string, int>();
    /// kvpCollection.Add(new KeyValuePair<string, int>("Four", 4));
    /// kvpCollection.Remove(new KeyValuePair<string, int>("Four", 4), out bool removed);
    /// Console.WriteLine(removed);  // Outputs: true
    /// ]]>
    /// </code>
    /// </example>
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