using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Solitons.Data
{
    /// <summary>
    /// A marker interface automatically adding the <see cref="ToXmlString"/> method to implementing types.
    /// </summary>
    /// <seealso cref="Parse{T}(string)"/>
    /// <seealso cref="Parse(string, Type)"/>
    /// <seealso cref="BasicXmlDataTransferObject"/>
    /// <seealso cref="BasicXmlMediaTypeSerializer"/>
    public interface IBasicXmlDataTransferObject 
    {
        /// <summary>
        /// Converts this instance to its XML- string representation.
        /// </summary>
        /// <returns>The XML- string object representation</returns>
        [DebuggerStepThrough]
        public sealed string ToXmlString()
        {
            var callback = this as ISerializationCallback;
            var serializer = new XmlSerializer(GetType());
            using var writer = new StringWriter();
            callback?.OnSerializing(null);
            serializer.Serialize(writer, this);
            writer.Flush();
            var xml = writer.ToString();
            callback?.OnSerialized(null);
            return xml;
        }

        /// <summary>
        /// Deserializes the XML to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="xmlString">The XML to deserialize.</param>
        /// <returns>The XML- deserialized object.</returns>
        [DebuggerNonUserCode]
        public static T Parse<T>(string xmlString) where T : IBasicXmlDataTransferObject, new()
        {
            return (T) Parse(xmlString, typeof(T));
        }

        /// <summary>
        /// Deserializes the XML to the specified .NET type.
        /// </summary>
        /// <param name="xmlString">The XML to deserialize.</param>
        /// <param name="returnType">The type of the object to deserialize to.</param>
        /// <returns>The XML- deserialized object.</returns>
        [DebuggerNonUserCode]
        public static object Parse(string xmlString, Type returnType) 
        {
            var serializer = new XmlSerializer(returnType);
            using var reader = new StringReader(xmlString);
            var obj = serializer.Deserialize(reader);
            if (obj is IDeserializationCallback callback)
                callback.OnDeserialization(typeof(IBasicXmlDataTransferObject));
            return obj ?? new FormatException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Converts this instance to its XML- string representation.
        /// </summary>
        /// <param name="self">The object to serialize</param>
        /// <returns>The XML- string object representation</returns>
        [DebuggerStepThrough]
        public static string ToXmlString(this IBasicXmlDataTransferObject self) => self.ToXmlString();

    }

}
