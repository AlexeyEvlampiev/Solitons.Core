using System;

namespace Solitons.Text;

/// <summary>
/// Contains regular expression patterns for common use cases.
/// </summary>
public static partial class RegexPatterns
{
    /// <summary>
    /// Regular expression pattern for matching email addresses.
    /// </summary>
    public const string Email = @"^[a-zA-Z0-9.!#$%&''*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        
    /// <summary>
    /// Generates a regular expression pattern that matches one or more values from the specified enumeration type.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="values">The values to match.</param>
    /// <param name="ignoreCase">True to ignore case when matching.</param>
    /// <returns>A regular expression pattern that matches the specified values.</returns>
    public static string GenerateRegexExpression<T>(T values, bool ignoreCase = true) where T : Enum
    {
        var expression = values
            .ToString()
            .Replace(@"\s*,\s*", m => "|");
        var settings = ignoreCase ? "(?si)" : "(?s)";
        return $"{settings}^(?:{expression})$";
    }
}