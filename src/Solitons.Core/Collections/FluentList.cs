using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Collections;

/// <summary>
/// Provides a fluent API for working with a list of elements, enabling developers
/// to build and manipulate lists in a concise and readable manner.
/// </summary>
/// <example>
/// <code>
/// <![CDATA[
/// // Create a new FluentList instance and populate it with elements.
/// var list = FluentList.Create<int>().Add(1).Add(2, 3).AddRange(new[] { 4, 5, 6 });
/// 
/// // Remove an element from the list and then clear the list.
/// list.Remove(1).Clear();
/// ]]>
/// </code>
/// </example>
public static class FluentList
{
    /// <summary>
    /// Creates a new empty FluentList instance.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <returns>A new empty FluentList instance.</returns>
    [DebuggerNonUserCode]
    public static FluentList<T> Create<T>() => new();

    /// <summary>
    /// Creates a new FluentList instance and populates it with the elements in the specified collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="collection">The collection whose elements are to be copied to the new list.</param>
    /// <returns>A new FluentList instance containing the elements in the specified collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the collection is null.</exception>
    [DebuggerNonUserCode]
    public static FluentList<T> Create<T>(IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        return new FluentList<T>(collection.ToList());
    }

    /// <summary>
    /// Creates a new FluentList instance containing a single element.
    /// </summary>
    /// <typeparam name="T">The type of element in the list.</typeparam>
    /// <param name="item">The element to include in the list.</param>
    /// <returns>A new FluentList instance containing a single element.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // Create a FluentList instance with a single element.
    /// var singleItemList = FluentList.Create<int>(42);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> Create<T>(T item) => Wrap(new List<T>(){item});

    /// <summary>
    /// Creates a new FluentList instance containing two elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="item1">The first element to include in the list.</param>
    /// <param name="item2">The second element to include in the list.</param>
    /// <returns>A new FluentList instance containing the specified elements.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // Create a FluentList instance with two elements.
    /// var doubleItemList = FluentList.Create<int>(7, 42);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> Create<T>(T item1, T item2) => Wrap(new List<T>() { item1, item2});

    /// <summary>
    /// Creates a new FluentList instance containing three elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="item1">The first element to include in the list.</param>
    /// <param name="item2">The second element to include in the list.</param>
    /// <param name="item3">The third element to include in the list.</param>
    /// <returns>A new FluentList instance containing the specified elements.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // Create a FluentList instance with three elements.
    /// var tripleItemList = FluentList.Create<int>(7, 42, 100);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> Create<T>(T item1, T item2, T item3) => Wrap(new List<T>() { item1, item2, item3 });

    /// <summary>
    /// Creates a new <see cref="FluentList{T}"/> instance with the specified <paramref name="items"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="items">The items to add to the list.</param>
    /// <returns>A new <see cref="FluentList{T}"/> instance with the specified <paramref name="items"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="items"/> array is null.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var list = FluentList.Create<int>(1, 2, 3, 4);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> Create<T>(params T[] items) => Wrap(new List<T>(items));

    /// <summary>
    /// Creates a new <see cref="FluentList{T}"/> instance with the specified initial capacity and a single element.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="capacity">The initial capacity of the list.</param>
    /// <param name="item">The element to include in the list.</param>
    /// <returns>A new <see cref="FluentList{T}"/> instance with the specified initial capacity and a single element.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="capacity"/> is less than 0.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // Create a new FluentList instance with a specified capacity and a single element
    /// var list = FluentList.CreateWithCapacity<int>(10, 1);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> CreateWithCapacity<T>(int capacity, T item) => Wrap(new List<T>(capacity) { item });

    /// <summary>
    /// Creates a new <see cref="FluentList{T}"/> instance with the specified initial capacity and two elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="capacity">The initial capacity of the list.</param>
    /// <param name="item1">The first element to include in the list.</param>
    /// <param name="item2">The second element to include in the list.</param>
    /// <returns>A new <see cref="FluentList{T}"/> instance with the specified initial capacity and two elements.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="capacity"/> is less than 0.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // Create a new FluentList instance with a specified capacity and two elements
    /// var list = FluentList.CreateWithCapacity<int>(10, 1, 2);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> CreateWithCapacity<T>(int capacity, T item1, T item2) => Wrap(new List<T>(capacity) { item1, item2 });

    /// <summary>
    /// Creates a new <see cref="FluentList{T}"/> instance with the specified initial capacity and three elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="capacity">The initial capacity of the list.</param>
    /// <param name="item1">The first element to include in the list.</param>
    /// <param name="item2">The second element to include in the list.</param>
    /// <param name="item3">The third element to include in the list.</param>
    /// <returns>A new <see cref="FluentList{T}"/> instance with the specified initial capacity and three elements.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="capacity"/> is less than 0.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // Create a new FluentList instance with a specified capacity and three elements
    /// var list = FluentList.CreateWithCapacity<string>(10, "Alice", "Bob", "Charlie");
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> CreateWithCapacity<T>(int capacity, T item1, T item2, T item3) => Wrap(new List<T>(capacity) { item1, item2, item3 });

    /// <summary>
    /// Creates a new <see cref="FluentList{T}"/> instance with the specified initial capacity and elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    /// <param name="capacity">The initial capacity of the list.</param>
    /// <param name="items">The elements to include in the list.</param>
    /// <returns>A new <see cref="FluentList{T}"/> instance with the specified initial capacity and elements.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="capacity"/> is less than 0.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // Create a new FluentList instance with a specified capacity and elements
    /// var list = FluentList.CreateWithCapacity<string>(10, "Alice", "Bob", "Charlie");
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> CreateWithCapacity<T>(int capacity, params T[] items)
    {
        var list = new List<T>(capacity);
        list.AddRange(items);
        return Wrap(list);
    }

    /// <summary>
    /// Creates a new <see cref="FluentList{T}"/> instance with the specified initial capacity.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    /// <param name="capacity">The initial capacity of the list.</param>
    /// <returns>A new <see cref="FluentList{T}"/> instance with the specified initial capacity.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="capacity"/> is less than 0.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // Create a new FluentList instance with a specified capacity
    /// var list = FluentList.CreateWithCapacity<string>(10);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> CreateWithCapacity<T>(int capacity) => new(new List<T>(capacity));

    /// <summary>
    /// Creates a new <see cref="FluentList{T}"/> instance with the specified initial capacity and copies the elements from the specified collection to the new list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="capacity">The initial capacity of the list.</param>
    /// <param name="collection">The collection whose elements are copied to the new list.</param>
    /// <returns>A new <see cref="FluentList{T}"/> with the specified initial capacity and the elements copied from the specified collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="capacity"/> is less than 0.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var sourceCollection = new List<int> { 1, 2, 3, 4, 5 };
    /// // Create a new FluentList instance with a specified capacity and copy elements from a collection
    /// var list = FluentList.CreateWithCapacity(10, sourceCollection);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> CreateWithCapacity<T>(int capacity, IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        var list = new List<T>(capacity);
        list.AddRange(collection);
        return new FluentList<T>(list);
    }

    /// <summary>
    /// Wraps the given <paramref name="list"/> in a new instance of <see cref="FluentList{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    /// <param name="list">The list to wrap.</param>
    /// <returns>A new instance of <see cref="FluentList{T}"/> that wraps the given <paramref name="list"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="list"/> parameter is null.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var existingList = new List<int> { 1, 2, 3, 4, 5 };
    /// // Wrap the existing list in a new FluentList instance
    /// var fluentList = FluentList.Wrap(existingList);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> Wrap<T>(IList<T> list)
    {
        ThrowIf.ArgumentNull(list);
        return list is FluentList<T> fluentList
            ? fluentList
            : new FluentList<T>(list);
    }


    /// <summary>
    /// Creates a new instance of <see cref="FluentList{T}"/> by wrapping an existing <see cref="IList{T}"/> or creating a new one from the specified <see cref="IEnumerable{T}"/> collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="collection">The collection to wrap or create the list from.</param>
    /// <returns>A new instance of <see cref="FluentList{T}"/> containing the elements from the specified collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection"/> is null.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var existingList = new List<int> { 1, 2, 3, 4, 5 };
    /// // Wrap an existing list
    /// var wrappedList = FluentList.WrapOrCreate(existingList);
    ///
    /// var newCollection = new [] { 6, 7, 8, 9, 10 };
    /// // Create a new FluentList from a collection
    /// var newList = FluentList.WrapOrCreate(newCollection);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static FluentList<T> WrapOrCreate<T>(IEnumerable<T> collection)
    {
        return collection is FluentList<T> fluentList
            ? fluentList
            : new FluentList<T>(collection);
    }
}

/// <summary>
/// Provides a list implementation that enables a more fluent and concise syntax for working with lists.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
/// <example>
/// This example demonstrates how to create and use a FluentList instance.
/// <code>
/// <![CDATA[
/// var list = new FluentList<string>()
///     .Add("Alice")
///     .Add("Bob", "Charlie")
///     .AddRange(new [] { "David", "Eve" });
/// foreach (var name in list)
/// {
///     Console.WriteLine(name);
/// }
/// ]]>
/// </code>
/// </example>
public sealed class FluentList<T> : IList<T>
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly IList<T> _list;

    /// <summary>
    /// Initializes a new instance of the FluentList class that is empty and has the default initial capacity.
    /// </summary>
    /// <remarks>
    /// This constructor initializes a new FluentList instance with default capacity. Use this constructor when you know
    /// the list will grow to require the default capacity, to minimize reallocations.
    /// </remarks>
    [DebuggerNonUserCode]
    internal FluentList()
    {
        _list = new List<T>();
    }

    /// <summary>
    /// Initializes a new instance of the FluentList class that is empty and has the specified initial capacity.
    /// </summary>
    /// <param name="capacity">The number of elements that the new list can initially store.</param>
    [DebuggerNonUserCode]
    internal FluentList(int capacity)
    {
        _list = new List<T>(capacity);
    }

    /// <summary>
    /// Initializes a new instance of the FluentList class that contains elements copied from the specified collection.
    /// </summary>
    /// <param name="collection">The collection whose elements are copied to the new list.</param>
    /// <exception cref="ArgumentNullException">Thrown when the collection is null.</exception>
    [DebuggerNonUserCode]
    internal FluentList(IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        if (collection is FluentList<T> other)
        {
            _list = other._list;
        }
        else
        {
            _list = collection is IList<T> list
                ? list
                : new List<T>(collection);
        }
    }

    /// <summary>
    /// Explicitly converts a List to a FluentList.
    /// </summary>
    /// <param name="list">The list to convert.</param>
    /// <returns>A new FluentList instance containing the elements of the specified list.</returns>
    /// <remarks>
    /// This operator creates a new FluentList instance and populates it with the elements of the specified list.
    /// </remarks>
    [DebuggerNonUserCode]
    public static explicit operator FluentList<T>(List<T> list) => new();



    /// <inheritdoc/>
    [DebuggerNonUserCode]
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    /// <inheritdoc/>
    [DebuggerNonUserCode]
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();

    /// <inheritdoc/>
    [DebuggerNonUserCode]
    void ICollection<T>.Add(T item) => _list.Add(item);

    /// <summary>
    /// Adds an item to the end of the list and returns the list itself, enabling a fluent syntax.
    /// </summary>
    /// <param name="item">The item to add to the list.</param>
    /// <returns>The current FluentList instance.</returns>
    /// <remarks>
    /// Use this method to add a single item to the list and continue method chaining.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to use the <see cref="Add(T)"/> method to add an item to the list.
    /// <code>
    /// <![CDATA[
    /// var list = new FluentList<string>();
    /// list.Add("Alice")
    ///     .Add("Bob")
    ///     .Add("Charlie");
    /// foreach(var name in list)
    /// {
    ///     Console.WriteLine(name);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public FluentList<T> Add(T item)
    {
        _list.Add(item);
        return this;
    }

    /// <summary>
    /// Adds two items to the end of the list.
    /// </summary>
    /// <param name="item1">The first item to add.</param>
    /// <param name="item2">The second item to add.</param>
    /// <returns>The current instance of the <see cref="FluentList{T}"/> class, allowing for method chaining.</returns>
    /// <example>
    /// This example demonstrates how to use the <see cref="Add(T, T)"/> method to add two items to the list.
    /// <code>
    /// <![CDATA[
    /// var list = new FluentList<string>();
    /// list.Add("Alice", "Bob");
    /// foreach(var name in list)
    /// {
    ///     Console.WriteLine(name);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public FluentList<T> Add(T item1, T item2)
    {
        _list.Add(item1);
        _list.Add(item2);
        return this;
    }

    /// <summary>
    /// Adds three items to the list.
    /// </summary>
    /// <param name="item1">The first item to add.</param>
    /// <param name="item2">The second item to add.</param>
    /// <param name="item3">The third item to add.</param>
    /// <returns>The current instance of the <see cref="FluentList{T}"/> class.</returns>
    /// <remarks>
    /// This method provides a fluent way to add multiple items to the list at once.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to use the <see cref="Add(T,T,T)"/> method to add multiple items to the list.
    /// <code>
    /// <![CDATA[
    /// var list = new FluentList<string>();
    /// list.Add("Alice", "Bob", "Charlie");
    /// foreach(var name in list)
    /// {
    ///     Console.WriteLine(name);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public FluentList<T> Add(T item1, T item2, T item3)
    {
        _list.Add(item1);
        _list.Add(item2);
        _list.Add(item3);
        return this;
    }

    /// <summary>
    /// Adds the specified items to the end of the list, allowing for a fluent chaining of operations.
    /// </summary>
    /// <param name="items">The items to add to the list.</param>
    /// <returns>The current instance of <see cref="FluentList{T}"/> for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="items"/> is null.</exception>
    /// <remarks>
    /// This method accepts a parameter array, allowing you to add multiple items to the list in a single call.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to use the <see cref="Add(T[])"/> method to add multiple items to the list.
    /// <code>
    /// <![CDATA[
    /// var list = new FluentList<string>();
    /// list.Add("Alice", "Bob", "Charlie", "Tom");
    /// foreach(var name in list)
    /// {
    ///     Console.WriteLine(name);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public FluentList<T> Add(params T[] items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        foreach (var item in items)
        {
            _list.Add(item);
        }
        return this;
    }

    /// <summary>
    /// Adds the elements of the specified collection to the end of the <see cref="FluentList{T}"/>.
    /// </summary>
    /// <param name="items">The collection whose elements should be added to the end of the <see cref="FluentList{T}"/>. 
    /// The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
    /// <returns>The <see cref="FluentList{T}"/> after the elements are added.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="items"/> parameter is null.</exception>
    /// <example>
    /// This example demonstrates how to use the <see cref="AddRange"/> method to add multiple items to the list.
    /// <code>
    /// <![CDATA[
    /// var list = new FluentList<string>();
    /// var names = new[] { "Alice", "Bob", "Charlie" };
    /// list.AddRange(names);
    /// foreach(var name in list)
    /// {
    ///     Console.WriteLine(name);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public FluentList<T> AddRange(IEnumerable<T> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        foreach (var item in items)
        {
            _list.Add(item);
        }
        return this;
    }

    /// <inheritdoc/>
    [DebuggerNonUserCode]
    void ICollection<T>.Clear() => _list.Clear();

    /// <summary>
    /// Removes all elements from the <see cref="FluentList{T}"/>, and returns the list itself to enable method chaining.
    /// </summary>
    /// <returns>The current <see cref="FluentList{T}"/> instance with all elements removed.</returns>
    /// <remarks>
    /// Use this method to clear the list and continue method chaining.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to use the <see cref="Clear"/> method to remove all elements from the list.
    /// <code>
    /// <![CDATA[
    /// var list = new FluentList<string>();
    /// list.Add("Alice")
    ///     .Add("Bob")
    ///     .Add("Charlie");
    /// // List contains: Alice, Bob, Charlie
    /// list.Clear();
    /// // List is now empty
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public FluentList<T> Clear()
    {
        _list.Clear();
        return this;
    }

    /// <summary>
    /// Determines whether the list contains a specific element.
    /// </summary>
    /// <param name="item">The element to locate in the list.</param>
    /// <returns>
    /// True if the element is found in the list; otherwise, false.
    /// </returns>
    /// <example>
    /// This example demonstrates how to use the <see cref="Contains(T)"/> method to check if an item exists in the list.
    /// <code>
    /// <![CDATA[
    /// var list = new FluentList<string> { "Alice", "Bob", "Charlie" };
    /// var containsBob = list.Contains("Bob");  // returns true
    /// var containsDave = list.Contains("Dave");  // returns false
    /// Console.WriteLine(containsBob);  // Outputs: True
    /// Console.WriteLine(containsDave);  // Outputs: False
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public bool Contains(T item) => _list.Contains(item);

    /// <summary>
    /// Determines whether this list contains a specific element and returns the index of the first occurrence.
    /// </summary>
    /// <param name="item">The object to locate in the list.</param>
    /// <param name="index">When this method returns, contains the index of the first occurrence of the specified element, if found; otherwise, -1.</param>
    /// <returns>true if the list contains the specified element; otherwise, false.</returns>
    /// <remarks>
    /// Use this method to not only check the presence of an item, but also to retrieve its index in a single operation.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to use the <see cref="Contains(T, out int)"/> method to check the presence of an item and obtain its index.
    /// <code>
    /// <![CDATA[
    /// var list = new FluentList<string> { "Apple", "Banana", "Cherry" };
    /// if (list.Contains("Banana", out int index))
    /// {
    ///     Console.WriteLine($"Item found at index {index}");
    /// }
    /// else
    /// {
    ///     Console.WriteLine("Item not found");
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public bool Contains(T item, out int index)
    {
        index = IndexOf(item);
        return index >= 0;
    }

    /// <summary>
    /// Copies the elements of the FluentList to an array, starting at a particular array index.
    /// </summary>
    /// <param name="array">The one-dimensional array that is the destination of the elements copied from FluentList. The array must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    /// <exception cref="ArgumentNullException">Thrown if the destination array is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if arrayIndex is negative.</exception>
    /// <exception cref="ArgumentException">Thrown if there is not enough available space from arrayIndex to the end of the destination array to accommodate all elements.</exception>
    /// <example>
    /// This example demonstrates how to copy elements from a FluentList to an array.
    /// <code>
    /// <![CDATA[
    /// var list = new FluentList<int> { 1, 2, 3, 4, 5 };
    /// var array = new int[5];
    /// list.CopyTo(array, 0);
    /// // array now contains: 1, 2, 3, 4, 5
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    [DebuggerNonUserCode]
    public bool Remove(T item) => _list.Remove(item);


    /// <summary>
    /// Removes the first occurrence of a specific object from the FluentList and returns the FluentList instance to allow chaining.
    /// </summary>
    /// <param name="item">The object to remove from the FluentList.</param>
    /// <param name="removed">When this method returns, contains true if item is successfully removed; otherwise, false. This parameter is passed uninitialized.</param>
    /// <returns>The FluentList instance.</returns>
    /// <remarks>
    /// This method is useful when you want to remove an item and continue method chaining, while also checking whether the removal was successful.
    /// </remarks>
    /// <example>
    /// This example demonstrates how to use the Remove method to remove an item and check the result.
    /// <code>
    /// <![CDATA[
    /// var list = new FluentList<string> { "Alice", "Bob", "Charlie" };
    /// bool removed;
    /// list.Remove("Bob", out removed);
    /// Console.WriteLine(removed);  // Outputs: True
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public FluentList<T> Remove(T item, out bool removed)
    {
        removed = _list.Remove(item);
        return this;
    }

    /// <inheritdoc/>
    [DebuggerNonUserCode]
    public int Count => _list.Count;

    /// <inheritdoc/>
    [DebuggerNonUserCode]
    bool ICollection<T>.IsReadOnly => _list.IsReadOnly;

    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire FluentList.
    /// </summary>
    /// <param name="item">The object to locate in the FluentList.</param>
    /// <returns>The zero-based index of the first occurrence of item within the entire FluentList, if found; otherwise, –1.</returns>
    [DebuggerNonUserCode]
    public int IndexOf(T item) => _list.IndexOf(item);

    /// <inheritdoc/>
    [DebuggerNonUserCode]
    void IList<T>.Insert(int index, T item) => _list.Insert(index, item);

    /// <summary>
    /// Inserts an element into the FluentList at the specified index and returns FluentList instance to allow chaining.
    /// </summary>
    /// <param name="index">The zero-based index at which item should be inserted.</param>
    /// <param name="item">The object to insert into the FluentList.</param>
    /// <returns>The FluentList instance.</returns>
    [DebuggerNonUserCode]
    public FluentList<T> Insert(int index, T item)
    {
        _list.Insert(index, item);
        return this;
    }

    /// <inheritdoc/>
    [DebuggerNonUserCode]
    void IList<T>.RemoveAt(int index) => _list.RemoveAt(index);

    /// <summary>
    /// Removes the element at the specified index from the <see cref="FluentList{T}"/>.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <returns>The current instance of the <see cref="FluentList{T}"/> after the element has been removed.</returns>
    [DebuggerNonUserCode]
    public FluentList<T> RemoveAt(int index)
    {
        _list.RemoveAt(index);
        return this;
    }

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">index is less than 0 or index is equal to or greater than Count.</exception>
    public T this[int index]
    {
        [DebuggerNonUserCode]
        get => _list[index];
        [DebuggerNonUserCode]
        set => _list[index] = value;
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override int GetHashCode() => _list.GetHashCode();

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (ReferenceEquals(_list, obj)) return true;
        if (ReferenceEquals(null, obj)) return false;
        return _list.Equals(obj);
    }
}