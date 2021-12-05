using System;
using System.IO;
using System.Xml.Serialization;

namespace Solitons.Common
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BasicXmlDataContractSerializer : DataContractSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        public BasicXmlDataContractSerializer() : base("application/xml")
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

        protected override object Deserialize(string content, Type targetType)
        {
            var serializer = new XmlSerializer(targetType);
            using var reader = new StringReader(content);
            return serializer.Deserialize(reader);
        }
    }
}
