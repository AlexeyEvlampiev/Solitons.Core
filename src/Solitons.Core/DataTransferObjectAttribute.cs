using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Solitons.Common;

namespace Solitons
{
    /// <summary>
    /// Annotates custom types as Data Transfer Objects, specifying a serializer type to use for object serialization and packing.
    /// </summary>
    /// <seealso cref="IDataTransferObjectSerializer"/>
    /// <seealso cref="IBasicJsonDataTransferObject"/>
    /// <seealso cref="IBasicXmlDataTransferObject"/>
    /// <seealso cref="BasicJsonDataTransferObject"/>
    /// <seealso cref="BasicXmlDataTransferObject"/>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DataTransferObjectAttribute : Attribute, IDataTransferObjectMetadata
    {
        internal DataTransferObjectAttribute(IDataTransferObjectMetadata other, Type targetType) 
            : this(other.SerializerType)
        {
            IsDefault = other.IsDefault;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializerType"></param>
        public DataTransferObjectAttribute(Type serializerType)
        {
            SerializerType = serializerType ?? throw new ArgumentNullException(nameof(serializerType));
            if (false == typeof(IDataTransferObjectSerializer).IsAssignableFrom(serializerType))
                throw new ArgumentException($"{serializerType} does not implement {typeof(IDataTransferObjectSerializer)}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        internal DataTransferObjectAttribute(IDataTransferObjectMetadata other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            SerializerType = other.SerializerType ?? throw new ArgumentException($"{nameof(other.SerializerType)} is null", nameof(other));
            if (false == typeof(IDataTransferObjectSerializer).IsAssignableFrom(SerializerType))
                throw new ArgumentException($"{SerializerType} does not implement {typeof(IDataTransferObjectSerializer)}");
        }


        /// <summary>
        /// Gets the of serializer to be used for serialization and packing of annotated Data Transfer Objects.        
        /// </summary>
        /// <remarks>
        /// The serializer type is required to implement <see cref="IDataTransferObjectSerializer"/>
        /// </remarks>
        public Type SerializerType { get; }

        /// <summary>
        /// Gets or sets the flag indicating whether this Data Transfer Object serialization should be applied by default.
        /// </summary>
        public bool IsDefault { get; set; }

    }
}
