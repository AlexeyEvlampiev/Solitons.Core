using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Solitons.Common;

namespace Solitons.Data;

/// <summary>
/// XML-first Data Transfer Object. When used as a base class, ensures that the overridden <see cref="object.ToString"/> method returns the object's XML representation.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="BasicXmlDataTransferObject"/> class serves as a base class for XML-first Data Transfer Objects (DTOs).
/// When derived from, it ensures that the <see cref="object.ToString"/> method returns the XML representation of the object.
/// </para>
/// <para>
/// The <see cref="ToString"/> method is overridden to return the XML string representation of the object using the <see cref="ToXmlMediaContent()"/> method.
/// </para>
/// <para>
/// The <see cref="Clone"/> method creates a deep copy of the object by performing XML deserialization on the object's XML representation.
/// </para>
/// </remarks>
public abstract class BasicXmlDataTransferObject : SerializationCallback, IBasicXmlDataTransferObject, ICloneable
{
    /// <summary>
    /// Returns the XML string representation of the object.
    /// </summary>
    /// <returns>The XML string representation of the object.</returns>
    [DebuggerStepThrough]
    public sealed override string ToString() => this.ToXmlMediaContent();

    /// <summary>
    /// Creates a deep copy of the object by performing XML deserialization on its XML representation.
    /// </summary>
    /// <returns>A deep copy of the object.</returns>
    [DebuggerNonUserCode]
    protected BasicXmlDataTransferObject Clone()
    {
        var xml = ToString();
        var serializer = new XmlSerializer(GetType());
        return ((BasicXmlDataTransferObject)serializer.Deserialize(new StringReader(xml))!);
    }


    [DebuggerStepThrough]
    object ICloneable.Clone() => Clone();


    /// <summary>
    /// Converts the object to its XML string representation.
    /// </summary>
    /// <returns>The XML string representation of the object.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToXmlMediaContent() => ToXmlMediaContent(this);

    /// <summary>
    /// Converts the object to its XML string representation with additional XML types.
    /// </summary>
    /// <param name="extraTypes">Additional XML types to include during serialization.</param>
    /// <returns>The XML string representation of the object.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToXmlMediaContent(Type[] extraTypes) => ToXmlMediaContent(this, extraTypes);

    /// <summary>
    /// Converts the object to its XML string representation with XML attribute overrides.
    /// </summary>
    /// <param name="overrides">The XML attribute overrides to apply during serialization.</param>
    /// <returns>The XML string representation of the object.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToXmlMediaContent(XmlAttributeOverrides overrides) => ToXmlMediaContent(this, overrides);

    /// <summary>
    /// Converts the object to its XML string representation with additional options.
    /// </summary>
    /// <param name="overrides">The XML attribute overrides to apply during serialization.</param>
    /// <param name="extraTypes">Additional XML types to include during serialization.</param>
    /// <param name="root">The XML root attribute to apply during serialization.</param>
    /// <param name="defaultNamespace">The default namespace to use during serialization.</param>
    /// <returns>The XML string representation of the object.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToXmlMediaContent(
        XmlAttributeOverrides overrides,
        Type[] extraTypes,
        XmlRootAttribute? root,
        string? defaultNamespace) => ToXmlMediaContent(this, overrides, extraTypes, root, defaultNamespace);

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
    public TextMediaContent ToXmlMediaContent(
        XmlAttributeOverrides overrides,
        Type[] extraTypes,
        XmlRootAttribute? root,
        string? defaultNamespace,
        string? location) => ToXmlMediaContent(this, overrides, extraTypes, root, defaultNamespace, location);


    [DebuggerStepThrough]
    internal static TextMediaContent ToXmlMediaContent(IBasicXmlDataTransferObject dto)
    {
        var callback = dto as ISerializationCallback;
        var serializer = new XmlSerializer(dto.GetType());
        using var writer = new StringWriter();
        callback?.OnSerializing(null);
        serializer.Serialize(writer, dto);
        writer.Flush();
        var xml = writer.ToString();
        callback?.OnSerialized(null);
        return TextMediaContent.CreateXml(xml);
    }

    [DebuggerStepThrough]
    internal static TextMediaContent ToXmlMediaContent(IBasicXmlDataTransferObject dto, Type[] extraTypes)
    {
        var callback = dto as ISerializationCallback;
        var serializer = new XmlSerializer(dto.GetType(), extraTypes);
        using var writer = new StringWriter();
        callback?.OnSerializing(null);
        serializer.Serialize(writer, dto);
        writer.Flush();
        var xml = writer.ToString();
        callback?.OnSerialized(null);
        return TextMediaContent.CreateXml(xml);
    }

    [DebuggerStepThrough]
    internal static TextMediaContent ToXmlMediaContent(IBasicXmlDataTransferObject dto, XmlAttributeOverrides overrides)
    {
        var callback = dto as ISerializationCallback;
        var serializer = new XmlSerializer(dto.GetType(), overrides);
        using var writer = new StringWriter();
        callback?.OnSerializing(null);
        serializer.Serialize(writer, dto);
        writer.Flush();
        var xml = writer.ToString();
        callback?.OnSerialized(null);
        return TextMediaContent.CreateXml(xml);
    }

    [DebuggerStepThrough]
    internal static TextMediaContent ToXmlMediaContent(
        IBasicXmlDataTransferObject dto, 
        XmlAttributeOverrides overrides,
        Type[] extraTypes,
        XmlRootAttribute? root,
        string? defaultNamespace)
    {
        var callback = dto as ISerializationCallback;
        var serializer = new XmlSerializer(dto.GetType(), overrides, extraTypes, root, defaultNamespace);
        using var writer = new StringWriter();
        callback?.OnSerializing(null);
        serializer.Serialize(writer, dto);
        writer.Flush();
        var xml = writer.ToString();
        callback?.OnSerialized(null);

        return TextMediaContent.CreateXml(xml);
    }

    [DebuggerStepThrough]
    internal static TextMediaContent ToXmlMediaContent(
        IBasicXmlDataTransferObject dto,
        XmlAttributeOverrides overrides,
        Type[] extraTypes,
        XmlRootAttribute? root,
        string? defaultNamespace,
        string? location)
    {
        var callback = dto as ISerializationCallback;
        var serializer = new XmlSerializer(dto.GetType(), overrides, extraTypes, root, defaultNamespace, location);
        using var writer = new StringWriter();
        callback?.OnSerializing(null);
        serializer.Serialize(writer, dto);
        writer.Flush();
        var xml = writer.ToString();
        callback?.OnSerialized(null);

        return TextMediaContent.CreateXml(xml);
    }


    /// <summary>
    /// Deserializes the specified XML string into an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="xmlString">The XML string to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    protected static T Parse<T>(string xmlString) where T : IBasicXmlDataTransferObject, new()
    {
        return IBasicXmlDataTransferObject.Parse<T>(xmlString);
    }

    /// <summary>
    /// Deserializes the specified XML string into an object of type <typeparamref name="T"/> with additional XML types.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="xmlString">The XML string to deserialize.</param>
    /// <param name="extraTypes">Additional XML types to include during deserialization.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    protected static T Parse<T>(string xmlString, Type[] extraTypes) where T : IBasicXmlDataTransferObject, new()
    {
        return IBasicXmlDataTransferObject.Parse<T>(xmlString, extraTypes);
    }

    /// <summary>
    /// Deserializes the specified XML string into an object of type <typeparamref name="T"/> with XML attribute overrides.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="xmlString">The XML string to deserialize.</param>
    /// <param name="overrides">The XML attribute overrides to apply during deserialization.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    protected static T Parse<T>(string xmlString, XmlAttributeOverrides overrides) where T : IBasicXmlDataTransferObject, new()
    {
        return IBasicXmlDataTransferObject.Parse<T>(xmlString, overrides);
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
    protected static T Parse<T>(
        string xmlString,
        XmlAttributeOverrides overrides,
        Type[] extraTypes,
        XmlRootAttribute? root,
        string? defaultNamespace) where T : IBasicXmlDataTransferObject, new()
    {
        return IBasicXmlDataTransferObject.Parse<T>(xmlString, overrides, extraTypes, root, defaultNamespace);
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
    protected static T Parse<T>(
        string xmlString,
        XmlAttributeOverrides overrides,
        Type[] extraTypes,
        XmlRootAttribute? root,
        string? defaultNamespace,
        string? location) where T : IBasicXmlDataTransferObject, new()
    {
        return IBasicXmlDataTransferObject.Parse<T>(xmlString, overrides, extraTypes, root, defaultNamespace, location);
    }
}