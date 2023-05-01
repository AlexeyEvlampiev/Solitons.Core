using System;
using System.IO;
using System.Xml.Serialization;
using Solitons.Data.Common;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
sealed class BasicXmlMediaTypeSerializer : MediaTypeSerializer
{
    /// <summary>
    /// 
    /// </summary>
    public BasicXmlMediaTypeSerializer() : base("application/xml")
    {
    }

    protected override string Serialize(object obj)
    {
        var serializer = new XmlSerializer(obj.GetType());
        using var writer = new StringWriter();
        serializer.Serialize(writer, obj);
        writer.Flush();
        return writer.ToString();
    }


    protected override object? Deserialize(string content, Type targetType)
    {
        var serializer = new XmlSerializer(targetType);
        using var reader = new StringReader(content);
        return serializer.Deserialize(reader);
    }
}