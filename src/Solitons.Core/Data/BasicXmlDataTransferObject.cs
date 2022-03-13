using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
            IBasicXmlDataTransferObject.Parse<T>(xmlString.ThrowIfNullOrWhiteSpaceArgument(nameof(xmlString)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DataContractSerializer BuildSerializer(DataContractSerializerBehaviour behaviour, IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            return new BasicXmlDataContractSerializer(behaviour, assemblies);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static partial class XmlDataTransferObjectExtensions
    {
        /// <summary>
        /// Parses this <see cref="string"/> as a xml object representation. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T ConvertTo<T>(this string self) where T : BasicXmlDataTransferObject, new() =>
            IBasicXmlDataTransferObject.Parse<T>(self);
    }
}
