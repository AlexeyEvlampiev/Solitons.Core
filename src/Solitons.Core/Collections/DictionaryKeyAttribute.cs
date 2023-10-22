using System;
using System.Diagnostics;
using System.Reflection;

namespace Solitons.Collections;

/// <summary>
/// The <c>DictionaryKeyAttribute</c> is used to specify a custom key name for a property 
/// when building a key-value pair collection from a class. 
/// By default, the property name is used as the key, but applying this attribute 
/// allows for a custom key name to be defined.
/// </summary>
/// <example>
/// Here is an example of how to use the <c>DictionaryKeyAttribute</c>:
/// <code><![CDATA[
/// namespace Solitons.Examples
/// {
///     using Solitons.Collections;
///     using System;
///     using System.Collections.Generic;
///     using System.Reflection;
///
///     public class Person
///     {
///         [DictionaryKey("First_Name")]
///         public string FirstName { get; set; }
/// 
///         [DictionaryKey("Last_Name")]
///         public string LastName { get; set; }
///     }
/// 
///     public class Program
///     {
///         public static void Main(string[] args)
///         {
///             var person = new Person { FirstName = "John", LastName = "Doe" };
///             var dictionary = GetDictionary(person);
/// 
///             foreach (var kvp in dictionary)
///             {
///                 Console.WriteLine($"{kvp.Key}: {kvp.Value}");
///             }
///         }
/// 
///         public static Dictionary<string, object> GetDictionary(object obj)
///         {
///             var dictionary = new Dictionary<string, object>();
///             var properties = obj.GetType().GetProperties();
/// 
///             foreach (var property in properties)
///             {
///                 var key = DictionaryKeyAttribute.ResolveKeyName(property);
///                 var value = property.GetValue(obj);
///                 dictionary.Add(key, value);
///             }
/// 
///             return dictionary;
///         }
///     }
/// }
/// ]]></code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class DictionaryKeyAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DictionaryKeyAttribute"/> class with the specified key name.
    /// </summary>
    /// <param name="name">The custom name of the key for the property.</param>
    /// <exception cref="ArgumentNullException">Thrown when the specified name is null or empty.</exception>
    public DictionaryKeyAttribute(string name)
    {
        Name = ThrowIf.ArgumentNullOrWhiteSpace(name, "Name is required", nameof(name));
    }

    /// <summary>
    /// Gets the custom name of the key for the property.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Retrieves the custom key name for the specified property, 
    /// or falls back to the property name if <see cref="DictionaryKeyAttribute"/> is not applied.
    /// </summary>
    /// <param name="property">The property for which to retrieve the key name.</param>
    /// <returns>The custom key name or the property name.</returns>
    [DebuggerNonUserCode]
    public static string ResolveKeyName(PropertyInfo property)
    {
        var attribute = property
            .GetCustomAttribute<DictionaryKeyAttribute>()?.Name;
        return attribute.DefaultIfNullOrWhiteSpace(property.Name)!;
    }
}