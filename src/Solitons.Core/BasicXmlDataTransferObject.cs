using Solitons.Common;
using System.Diagnostics;

namespace Solitons
{
    /// <summary>
    /// XML- first Data Transfer Object. When used as a base class, ensures that the overriden <see cref="object.ToString"/> returns objects xml representation.
    /// 
    /// </summary>
    public abstract class BasicXmlDataTransferObject : SerializationCallback, IBasicXmlDataTransferObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public sealed override string ToString() => this.ToXmlString();

        protected static T Parse<T>(string xmlString) where T : BasicXmlDataTransferObject, new() => IBasicXmlDataTransferObject.Parse<T>(xmlString);
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
