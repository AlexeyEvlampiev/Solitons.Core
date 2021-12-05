using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBasicXmlDataTransferObject : IBasicDataTransferObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToXmlString()
        {
            var serializer = new XmlSerializer(GetType());
            using var writer = new StringWriter();
            serializer.Serialize(writer, this);
            writer.Flush();
            return writer.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T FromXml<T>(string xmlString) where T : IBasicXmlDataTransferObject, new()
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xmlString);
            var obj = serializer.Deserialize(reader);
            if(obj is IDeserializationCallback callback)
                callback.OnDeserialization(typeof(IBasicXmlDataTransferObject));
            return (T) obj;
        }

        internal static object Parse(string xmlString, Type returnType) 
        {
            var serializer = new XmlSerializer(returnType);
            using var reader = new StringReader(xmlString);
            var obj = serializer.Deserialize(reader);
            if (obj is IDeserializationCallback callback)
                callback.OnDeserialization(typeof(IBasicXmlDataTransferObject));
            return obj;
        }
    }

    public static partial class Extensions
    {
        [DebuggerStepThrough]
        public static string ToXmlString(this IBasicXmlDataTransferObject self) => self.ToXmlString();

    }

}
