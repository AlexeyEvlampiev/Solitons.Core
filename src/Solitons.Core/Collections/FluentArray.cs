using System.Diagnostics;

namespace Solitons.Collections;

/// <summary>
/// Provides a set of fluent static methods to create arrays.
/// </summary>
/// <remarks>
/// The FluentArray class is a utility class that provides a set of methods to create arrays in a fluent manner.
/// This class is designed to improve code readability and reduce the amount of code required to create arrays.
/// </remarks>
/// <example>
/// Here is an example of how to use the FluentArray class:
/// <code>
/// <![CDATA[
/// var singleItemArray = FluentArray.Create(42);
/// var doubleItemArray = FluentArray.Create(42, 43);
/// var tripleItemArray = FluentArray.Create(42, 43, 44);
/// var multipleItemArray = FluentArray.Create(42, 43, 44, 45);
/// ]]>
/// </code>
/// </example>
public static class FluentArray
{
    /// <summary>
    /// Creates a new array of type <typeparamref name="T"/> with a single element.
    /// </summary>
    /// <typeparam name="T">The type of the array element.</typeparam>
    /// <param name="item">The element to include in the array.</param>
    /// <returns>A new array of type <typeparamref name="T"/> with a single element.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var array = FluentArray.Create(42);
    /// ]]>
    /// </code>
    /// </example>
    public static T[] Create<T>(T item) => new T[] { item };

    /// <summary>
    /// Creates a new array of type <typeparamref name="T"/> with two elements.
    /// </summary>
    /// <typeparam name="T">The type of the array elements.</typeparam>
    /// <param name="item1">The first element to include in the array.</param>
    /// <param name="item2">The second element to include in the array.</param>
    /// <returns>A new array of type <typeparamref name="T"/> with two elements.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var array = FluentArray.Create(42, 43);
    /// ]]>
    /// </code>
    /// </example>
    public static T[] Create<T>(T item1, T item2) => new T[] { item1, item2 };

    /// <summary>
    /// Creates a new array of type <typeparamref name="T"/> with three elements.
    /// </summary>
    /// <typeparam name="T">The type of the array elements.</typeparam>
    /// <param name="item1">The first element to include in the array.</param>
    /// <param name="item2">The second element to include in the array.</param>
    /// <param name="item3">The third element to include in the array.</param>
    /// <returns>A new array of type <typeparamref name="T"/> with three elements.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var array = FluentArray.Create(42, 43, 44);
    /// ]]>
    /// </code>
    /// </example>
    public static T[] Create<T>(T item1, T item2, T item3) => new T[] { item1, item2, item3 };

    /// <summary>
    /// Creates a new array of type <typeparamref name="T"/> with any number of elements.
    /// </summary>
    /// <typeparam name="T">The type of the array elements.</typeparam>
    /// <param name="items">The elements to include in the array.</param>
    /// <returns>A new array of type <typeparamref name="T"/> with the provided elements.</returns>
    /// <remarks>
    /// This method provides the most flexibility for creating arrays of varying lengths.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var array = FluentArray.Create(42, 43, 44, 45);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerNonUserCode]
    public static T[] Create<T>(params T[] items) => items;
}