using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Solitons.Collections;

/// <summary>
/// Provides a collection of extension methods that return an <see cref="IEnumerable{T}"/>, 
/// enabling developers to create sequences with fluent syntax.
/// </summary>
/// <example>
/// <code>
/// <![CDATA[
/// // Creating an IEnumerable with a single item
/// var singleItem = FluentEnumerable.Yield(42);
/// 
/// // Creating an IEnumerable with two items
/// var twoItems = FluentEnumerable.Yield(42, 43);
/// 
/// // Creating an IEnumerable with multiple items
/// var multipleItems = FluentEnumerable.Yield(42, 43, 44, 45);
/// ]]>
/// </code>
/// </example>
public static class FluentEnumerable
{
    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that contains a single item.
    /// </summary>
    /// <typeparam name="T">The type of the item to return.</typeparam>
    /// <param name="item">The item to return.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains only the specified item.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var singleItem = FluentEnumerable.Yield(42);
    /// ]]>
    /// </code>
    /// </example>
    public static IEnumerable<T> Yield<T>(T item)
    {
        yield return item;
    }

    /// <summary>
    /// Conditionally encapsulates a single item within an <see cref="IEnumerable{T}"/> based on a specified condition.
    /// If the condition is met, an <see cref="IEnumerable{T}"/> containing the item is returned; otherwise, an empty <see cref="IEnumerable{T}"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the item to be encapsulated within the returned <see cref="IEnumerable{T}"/>.</typeparam>
    /// <param name="item">The item to potentially encapsulate within an <see cref="IEnumerable{T}"/>.</param>
    /// <param name="condition">A Boolean expression representing the condition to be met for the item to be encapsulated within the returned <see cref="IEnumerable{T}"/>.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> that either contains the specified item if the condition is true, or is empty if the condition is false.
    /// </returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var item = 42;
    /// // If the item is even, encapsulate it within an IEnumerable; otherwise, return an empty IEnumerable.
    /// var collection = FluentEnumerable.Yield(item, item % 2 == 0);
    /// ]]>
    /// </code>
    /// </example>
    /// <remarks>
    /// This method provides a concise way to conditionally turn a single item into a collection, which can be useful in scenarios 
    /// where an algorithm expects a collection but you only have a single item whose inclusion is contingent on certain criteria.
    /// </remarks>
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> Yield<T>(T item, bool condition)
    {
        if(condition)yield return item;
    }

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that contains two items.
    /// </summary>
    /// <typeparam name="T">The type of the items to return.</typeparam>
    /// <param name="item1">The first item to return.</param>
    /// <param name="item2">The second item to return.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the two specified items.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var twoItems = FluentEnumerable.Yield(42, 43);
    /// ]]>
    /// </code>
    /// </example>
    public static IEnumerable<T> Yield<T>(T item1, T item2)
    {
        yield return item1;
        yield return item2;
    }

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that contains three items.
    /// </summary>
    /// <typeparam name="T">The type of the items to return.</typeparam>
    /// <param name="item1">The first item to return.</param>
    /// <param name="item2">The second item to return.</param>
    /// <param name="item3">The third item to return.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the three specified items.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var threeItems = FluentEnumerable.Yield(42, 43, 44);
    /// ]]>
    /// </code>
    /// </example>
    public static IEnumerable<T> Yield<T>(T item1, T item2, T item3)
    {
        yield return item1;
        yield return item2;
        yield return item3;
    }

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that contains four items.
    /// </summary>
    /// <typeparam name="T">The type of the items to return.</typeparam>
    /// <param name="item1">The first item to return.</param>
    /// <param name="item2">The second item to return.</param>
    /// <param name="item3">The third item to return.</param>
    /// <param name="item4">The fourth item to return.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the four specified items.</returns>
    public static IEnumerable<T> Yield<T>(T item1, T item2, T item3, T item4)
    {
        yield return item1;
        yield return item2;
        yield return item3;
        yield return item4;
    }

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that contains the specified items.
    /// </summary>
    /// <typeparam name="T">The type of the items to return.</typeparam>
    /// <param name="items">The items to return.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the specified items.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var multipleItems = FluentEnumerable.Yield(42, 43, 44, 45);
    /// ]]>
    /// </code>
    /// </example>
    public static IEnumerable<T> Yield<T>(params T[] items) => items ?? Array.Empty<T>();

}