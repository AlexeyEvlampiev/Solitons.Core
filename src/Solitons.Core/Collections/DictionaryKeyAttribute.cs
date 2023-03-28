using System;

namespace Solitons.Collections;

/// <summary>
/// Indicates that a property is the key for a dictionary-like data structure.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class DictionaryKeyAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the DictionaryKeyAttribute class with the specified key name.
    /// </summary>
    /// <param name="name">The name of the key for the property.</param>
    /// <exception cref="ArgumentNullException">Thrown when the specified name is null or empty.</exception>
    public DictionaryKeyAttribute(string name)
    {
        Name = ThrowIf.ArgumentNullOrWhiteSpace(name, "Name is required", nameof(name));
    }

    /// <summary>
    /// Gets the name of the key for the property.
    /// </summary>
    public string Name { get; }
}