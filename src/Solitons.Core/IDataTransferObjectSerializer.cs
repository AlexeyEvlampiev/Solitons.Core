using System;
using System.Diagnostics;
using Solitons.Common;

namespace Solitons
{
    /// <summary>
    /// Represents a Content Type constraint serializer.
    /// </summary>
    public partial interface IDataTransferObjectSerializer
    {
        /// <summary>
        /// Gets the serializer constraint content type.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Returns a string representation of the specified object, encoded according to the declared content type specifications (see <see cref="ContentType"/>).
        /// </summary>
        /// <param name="obj">The object to be serialized</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/></exception>
        /// <exception cref="NotSupportedException"><paramref name="obj"/></exception>
        string Serialize(object obj);

        /// <summary>
        /// Returns an object deserialized from the specified content string.
        /// </summary>
        /// <param name="content">Objects string representation</param>
        /// <param name="targetType">Target object type</param>
        /// <returns>Deserialized object</returns>
        /// <exception cref="ArgumentException"><paramref name="content"/> or <paramref name="targetType"/></exception>
        object Deserialize(string content, Type targetType);
    }


    public partial interface IDataTransferObjectSerializer
    {
        /// <summary>
        /// Default basic JSON serializer instance
        /// </summary>
        public static readonly IDataTransferObjectSerializer BasicJsonSerializer = new BasicJsonDataTransferObjectSerializer();

        /// <summary>
        /// Default basic XML serializer instance
        /// </summary>
        public static readonly IDataTransferObjectSerializer BasicXmlSerializer = new BasicXmlDataTransferObjectSerializer();

        /// <summary>
        /// Extends this object behaviour with additional assertions for methods and properties. 
        /// </summary>
        /// <returns>Proxy instance</returns>
        [DebuggerNonUserCode]
        public IDataTransferObjectSerializer AsDataTransferObjectSerializer() =>
            DataTransferObjectSerializerProxy.Wrap(this);
    }
}
