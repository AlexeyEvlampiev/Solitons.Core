using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Solitons.Collections;

namespace Solitons.Diagnostics;

/// <summary>
/// Represents a builder for creating log messages with properties and tags.
/// </summary>
public partial interface ILogStringBuilder
{
    /// <summary>
    /// Adds a new property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    ILogStringBuilder WithProperty(string name, object value);

    /// <summary>
    /// Adds a new tag to the log message.
    /// </summary>
    /// <param name="tag">The tag to add to the log message.</param>
    /// <returns>The current instance of the log string builder.</returns>
    ILogStringBuilder WithTags(string tag);

    /// <summary>
    /// Converts the log message to a string.
    /// </summary>
    /// <returns>The log message as a string.</returns>
    string ToString();
}

public partial interface ILogStringBuilder
{
    /// <summary>
    /// Adds multiple tags to the log message.
    /// </summary>
    /// <param name="tag0">The first tag to add.</param>
    /// <param name="tag1">The second tag to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public sealed ILogStringBuilder WithTags(string tag0, string tag1)
    {
        return WithTags(tag0)
            .WithTags(tag1);
    }

    /// <summary>
    /// Adds multiple tags to the log message.
    /// </summary>
    /// <param name="tag0">The first tag to add.</param>
    /// <param name="tag1">The second tag to add.</param>
    /// <param name="tag2">The third tag to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public sealed ILogStringBuilder WithTags(string tag0, string tag1, string tag2)
    {
        return WithTags(tag0)
            .WithTags(tag1)
            .WithTags(tag2);
    }

    /// <summary>
    /// Adds multiple properties to the log message.
    /// </summary>
    /// <param name="properties">An enumerable collection of key-value pairs representing the properties to add to the log message.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperties(IEnumerable<KeyValuePair<string, object>> properties)
    {
        foreach (var property in properties)
        {
            WithProperty(property.Key, property.Value);
        }
        return this;
    }

    /// <summary>
    /// Adds multiple properties to the log message.
    /// </summary>
    /// <param name="config">An action that configures a dictionary of properties using a fluent API.</param>
    /// <returns>The current instance of the log string builder.</returns>
    public sealed ILogStringBuilder WithProperties(
        Action<FluentDictionary<string, object>> config)
    {
        var properties = new Dictionary<string, object>();
        var fluentProperties = FluentDictionary.Create(properties);
        config.Invoke(fluentProperties);
        return WithProperties(properties);
    }

    /// <summary>
    /// Adds multiple tags to the log message.
    /// </summary>
    /// <param name="tags">An array of tags to add to the log message.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public sealed ILogStringBuilder WithTags(params string[] tags)
    {
        ILogStringBuilder entry = this;
        foreach (var tag in tags)
        {
            entry = entry.WithTags(tag);
        }

        return entry;
    }

    /// <summary>
    /// Adds a new boolean property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperty(string name, bool value) => this.WithProperty(name, (object)value);

    /// <summary>
    /// Adds a new TimeSpan property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperty(string name, TimeSpan value)
    {
        WithProperty(name, (object)XmlConvert.ToString(value));
        return this;
    }

    /// <summary>
    /// Adds a new DateTime property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperty(string name, DateTime value) => this.WithProperty(name, (object)value);

    /// <summary>
    /// Adds a new DateTimeOffset property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperty(string name, DateTimeOffset value) => this.WithProperty(name, (object)value);

    /// <summary>
    /// Adds a new Guid property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperty(string name, Guid value) => this.WithProperty(name, (object)value);

    /// <summary>
    /// Adds a new int property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperty(string name, int value) => this.WithProperty(name, (object)value);

    /// <summary>
    /// Adds a new uint property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperty(string name, uint value) => this.WithProperty(name, (object)value);


    /// <summary>
    /// Adds a new short property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperty(string name, short value) => this.WithProperty(name, (object)value);

    /// <summary>
    /// Adds a new ushort property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperty(string name, ushort value) => this.WithProperty(name, (object)value);

    /// <summary>
    /// Adds a new double property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperty(string name, double value) => this.WithProperty(name, (object)value);

    /// <summary>
    /// Adds a new float property to the log message with the specified name and value.
    /// </summary>
    /// <param name="name">The name of the property to add.</param>
    /// <param name="value">The value of the property to add.</param>
    /// <returns>The current instance of the log string builder.</returns>
    [DebuggerStepThrough]
    public virtual ILogStringBuilder WithProperty(string name, float value) => this.WithProperty(name, (object)value);
}