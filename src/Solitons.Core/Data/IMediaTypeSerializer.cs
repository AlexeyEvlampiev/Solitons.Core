using System;
using System.Diagnostics;

namespace Solitons.Data
{
    /// <summary>
    /// Represents a Content Type constraint serializer.
    /// </summary>
    public partial interface IMediaTypeSerializer
    {
        /// <summary>
        /// Gets the serializer constraint content type.
        /// </summary>
        string TargetContentType { get; }

        /// <summary>
        /// Returns a string representation of the specified object, encoded according to the declared content type specifications (see <see cref="TargetContentType"/>).
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


    public partial interface IMediaTypeSerializer
    {
        /// <summary>
        /// Default basic JSON serializer instance
        /// </summary>
        public static readonly IMediaTypeSerializer BasicJsonSerializer = new BasicJsonMediaTypeSerializer();

        /// <summary>
        /// Default basic XML serializer instance
        /// </summary>
        public static readonly IMediaTypeSerializer BasicXmlSerializer = new BasicXmlMediaTypeSerializer();

        /// <summary>
        /// Extends this object behaviour with additional assertions for methods and properties. 
        /// </summary>
        /// <returns>Proxy instance</returns>
        [DebuggerNonUserCode]
        public IMediaTypeSerializer AsDataTransferObjectSerializer() =>
            MediaTypeSerializerProxy.Wrap(this);
    }
}
