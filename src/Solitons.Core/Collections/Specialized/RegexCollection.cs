using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solitons.Collections.Specialized;

/// <summary>
/// Represents a read-only collection of regular expressions.
/// </summary>
public sealed class RegexCollection : IReadOnlyList<Regex>
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly Regex[] _expressions;


    /// <summary>
    /// Initializes a new instance of the RegexCollection class 
    /// that contains regular expressions created from an input sequence of patterns.
    /// </summary>
    /// <param name="patterns">The sequence of patterns to create regular expressions from.</param>
    [DebuggerNonUserCode]
    private RegexCollection(IEnumerable<string> patterns)
        : this(patterns.Select(pattern => new Regex(pattern)))
    {
    }

    /// <summary>
    /// Initializes a new instance of the RegexCollection class 
    /// that contains regular expressions copied from the specified sequence.
    /// </summary>
    /// <param name="expressions">The sequence of regular expressions to copy.</param>
    [DebuggerNonUserCode]
    public RegexCollection(IEnumerable<Regex> expressions)
    {
        _expressions = expressions.ToArray();
    }

    /// <summary>
    /// Creates a new RegexCollection from an enumerable of string patterns.
    /// </summary>
    /// <param name="patterns">The enumerable of patterns to create regular expressions from.</param>
    /// <returns>A new RegexCollection containing the regular expressions created from the patterns.</returns>
    [DebuggerNonUserCode]
    public static RegexCollection FromPatterns(IEnumerable<string> patterns)
    {
        return new RegexCollection(patterns.Select(pattern => new Regex(pattern)));
    }

    /// <summary>
    /// Creates a new RegexCollection from an array of string patterns.
    /// </summary>
    /// <param name="patterns">An array of patterns to create regular expressions from.</param>
    /// <returns>A new RegexCollection containing the regular expressions created from the patterns.</returns>
    [DebuggerNonUserCode]
    public static RegexCollection FromPatterns(params string[] patterns)
    {
        return new RegexCollection(patterns.Select(pattern => new Regex(pattern)));
    }

    /// <summary>
    /// Returns an enumerator that iterates through the RegexCollection.
    /// </summary>
    /// <returns>An enumerator for the RegexCollection.</returns>
    [DebuggerNonUserCode]
    public IEnumerator<Regex> GetEnumerator()
    {
        foreach (var expression in _expressions)
        {
            yield return expression;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _expressions.GetEnumerator();
    }

    /// <summary>
    /// Gets the number of regular expressions contained in the RegexCollection.
    /// </summary>
    public int Count => _expressions.Length;

    /// <summary>
    /// Gets the regular expression at the specified index in the RegexCollection.
    /// </summary>
    /// <param name="index">The index of the regular expression to get.</param>
    public Regex this[int index] => _expressions[index];

    /// <summary>
    /// Gets the index of the first regular expression that matches the input string.
    /// </summary>
    /// <param name="input">The string to match.</param>
    public int this[string input] => IndexOfFirstMatch(input, -1);

    /// <summary>
    /// Finds the index of the first regular expression that matches the input string.
    /// </summary>
    /// <param name="input">The string to match against the regular expressions.</param>
    /// <param name="defaultValue">The default value to return if no matches are found. Defaults to -1.</param>
    /// <returns>The index of the first matched regular expression, or the defaultValue if none matched.</returns>
    public int IndexOfFirstMatch(string input, int defaultValue = -1)
    {
        for (int i = 0; i < _expressions.Length; i++)
        {
            if (_expressions[i].IsMatch(input))
            {
                return i;
            }
        }

        return -1;
    }

}