using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Solitons.Data;

/// <summary>
/// Represents a marker interface that automatically adds the ToXmlString method to implementing types.
/// </summary>
/// <remarks>
/// The <see cref="IBasicXmlDataTransferObject"/> interface allows objects to be converted to their XML string representation
/// using the ToXmlString method. Implementing types can utilize this interface to enable XML serialization
/// and deserialization capabilities.
/// </remarks>
public interface IBasicXmlDataTransferObject
{
    /// <summary>
    /// Converts the object to its XML string representation.
    /// </summary>
    /// <returns>The XML string representation of the object.</returns>
    [DebuggerStepThrough]
    public sealed TextMediaContent ToXmlMediaContent() => BasicXmlDataTransferObject.ToXmlMediaContent(this);

    /// <summary>
    /// Converts the object to its XML string representation with additional XML types.
    /// </summary>
    /// <param name="extraTypes">Additional XML types to include during serialization.</param>
    /// <returns>The XML string representation of the object.</returns>
    [DebuggerStepThrough]
    public sealed TextMediaContent ToXmlMediaContent(Type[] extraTypes) => BasicXmlDataTransferObject.ToXmlMediaContent(this, extraTypes);

    /// <summary>
    /// Converts the object to its XML string representation with XML attribute overrides.
    /// </summary>
    /// <param name="overrides">The XML attribute overrides to apply during serialization.</param>
    /// <returns>The XML string representation of the object.</returns>
    [DebuggerStepThrough]
    public sealed TextMediaContent ToXmlMediaContent(XmlAttributeOverrides overrides) => BasicXmlDataTransferObject.ToXmlMediaContent(this, overrides);

    /// <summary>
    /// Converts the object to its XML string representation with additional options.
    /// </summary>
    /// <param name="overrides">The XML attribute overrides to apply during serialization.</param>
    /// <param name="extraTypes">Additional XML types to include during serialization.</param>
    /// <param name="root">The XML root attribute to apply during serialization.</param>
    /// <param name="defaultNamespace">The default namespace to use during serialization.</param>
    /// <returns>The XML string representation of the object.</returns>
    [DebuggerStepThrough]
    public sealed TextMediaContent ToXmlMediaContent(
        XmlAttributeOverrides overrides,
        Type[] extraTypes,
        XmlRootAttribute? root,
        string? defaultNamespace) => BasicXmlDataTransferObject.ToXmlMediaContent(this, overrides, extraTypes, root, defaultNamespace);

    /// <summary>
    /// Converts the object to its XML string representation with additional options.
    /// </summary>
    /// <param name="overrides">The XML attribute overrides to apply during serialization.</param>
    /// <param name="extraTypes">Additional XML types to include during serialization.</param>
    /// <param name="root">The XML root attribute to apply during serialization.</param>
    /// <param name="defaultNamespace">The default namespace to use during serialization.</param>
    /// <param name="location">The location of the XML schema to use during serialization.</param>
    /// <returns>The XML string representation of the object.</returns>
    [DebuggerStepThrough]
    public sealed TextMediaContent ToXmlMediaContent(
        XmlAttributeOverrides overrides,
        Type[] extraTypes,
        XmlRootAttribute? root,
        string? defaultNamespace,
        string? location) => BasicXmlDataTransferObject.ToXmlMediaContent(this, overrides, extraTypes, root, defaultNamespace, location);

    /// <summary>
    /// Deserializes the specified XML string into an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="xmlString">The XML string to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    public static T Parse<T>(string xmlString) where T : IBasicXmlDataTransferObject, new()
    {
        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(xmlString);
        var dto = ThrowIf.NullReference(serializer.Deserialize(reader) as IBasicXmlDataTransferObject);
        var callback = dto as IDeserializationCallback;
        callback?.OnDeserialization(null);
        return (T)dto;
    }

    /// <summary>
    /// Deserializes the specified XML string into an object of type <typeparamref name="T"/> with additional XML types.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="xmlString">The XML string to deserialize.</param>
    /// <param name="extraTypes">Additional XML types to include during deserialization.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    public static T Parse<T>(string xmlString, Type[] extraTypes) where T : IBasicXmlDataTransferObject, new()
    {
        var serializer = new XmlSerializer(typeof(T), extraTypes);
        using var reader = new StringReader(xmlString);
        var dto = ThrowIf.NullReference(serializer.Deserialize(reader) as IBasicXmlDataTransferObject);
        var callback = dto as IDeserializationCallback;
        callback?.OnDeserialization(null);
        return (T)dto;
    }

    /// <summary>
    /// Deserializes the specified XML string into an object of type <typeparamref name="T"/> with XML attribute overrides.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="xmlString">The XML string to deserialize.</param>
    /// <param name="overrides">The XML attribute overrides to apply during deserialization.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    public static T Parse<T>(string xmlString, XmlAttributeOverrides overrides) where T : IBasicXmlDataTransferObject, new()
    {
        var serializer = new XmlSerializer(typeof(T), overrides);
        using var reader = new StringReader(xmlString);
        var dto = ThrowIf.NullReference(serializer.Deserialize(reader) as IBasicXmlDataTransferObject);
        var callback = dto as IDeserializationCallback;
        callback?.OnDeserialization(null);
        return (T)dto;
    }

    /// <summary>
    /// Deserializes the specified XML string into an object of type <typeparamref name="T"/> with additional options.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="xmlString">The XML string to deserialize.</param>
    /// <param name="overrides">The XML attribute overrides to apply during deserialization.</param>
    /// <param name="extraTypes">Additional XML types to include during deserialization.</param>
    /// <param name="root">The XML root attribute to apply during deserialization.</param>
    /// <param name="defaultNamespace">The default namespace to use during deserialization.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    public static T Parse<T>(
        string xmlString,
        XmlAttributeOverrides overrides,
        Type[] extraTypes,
        XmlRootAttribute? root,
        string? defaultNamespace) where T : IBasicXmlDataTransferObject, new()
    {
        var serializer = new XmlSerializer(typeof(T), overrides, extraTypes, root, defaultNamespace);
        using var reader = new StringReader(xmlString);
        var dto = ThrowIf.NullReference(serializer.Deserialize(reader) as IBasicXmlDataTransferObject);
        var callback = dto as IDeserializationCallback;
        callback?.OnDeserialization(null);
        return (T)dto;
    }

    /// <summary>
    /// Deserializes the specified XML string into an object of type <typeparamref name="T"/> with additional options.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="xmlString">The XML string to deserialize.</param>
    /// <param name="overrides">The XML attribute overrides to apply during deserialization.</param>
    /// <param name="extraTypes">Additional XML types to include during deserialization.</param>
    /// <param name="root">The XML root attribute to apply during deserialization.</param>
    /// <param name="defaultNamespace">The default namespace to use during deserialization.</param>
    /// <param name="location">The location of the XML schema to use during deserialization.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    public static T Parse<T>(
        string xmlString,
        XmlAttributeOverrides overrides,
        Type[] extraTypes,
        XmlRootAttribute? root,
        string? defaultNamespace,
        string? location) where T : IBasicXmlDataTransferObject, new()
    {
        var serializer = new XmlSerializer(typeof(T), overrides, extraTypes, root, defaultNamespace, location);
        using var reader = new StringReader(xmlString);
        var dto = ThrowIf.NullReference(serializer.Deserialize(reader) as IBasicXmlDataTransferObject);
        var callback = dto as IDeserializationCallback;
        callback?.OnDeserialization(null);
        return (T)dto;
    }
}
