using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Solitons.Common;

namespace Solitons.Data
{
    /// <summary>
    /// XML- first Data Transfer Object. When used as a base class, ensures that the overriden <see cref="object.ToString"/> returns objects xml representation.
    /// 
    /// </summary>
    public abstract class BasicXmlDataTransferObject : SerializationCallback, IBasicXmlDataTransferObject, ICloneable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public sealed override string ToString() => this.ToXmlString();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        protected static T Parse<T>(string xmlString) where T : BasicXmlDataTransferObject, new() =>
            ThrowIf
                .ArgumentNullOrWhiteSpace(xmlString, nameof(xmlString))
                .Convert(IBasicXmlDataTransferObject.Parse<T>);
    }

}
