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
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct|AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DataTransferObjectAttribute : Attribute, IDataTransferObjectMetadata
    {
        private DataTransferObjectAttribute(IDataTransferObjectMetadata other, Type targetType) 
            : this(other.SerializerType)
        {
            TargetType = targetType;
            IsDefault = other.IsDefault;
        }
        /// <summary>
        /// Creates a new <see cref="DataTransferObjectAttribute"/> marker indicating that the target class can be serialized with <see cref="System.Text.Json.JsonSerializer"/>.
        /// </summary>
        public DataTransferObjectAttribute() : this(typeof(BasicJsonDataTransferObjectSerializer))
        {
            
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


        internal DataTransferObjectAttribute(Type type, IDataTransferObjectMetadata other)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (other == null) throw new ArgumentNullException(nameof(other));
            TargetType = type;
            SerializerType = other.SerializerType ?? throw new ArgumentException($"{nameof(other.SerializerType)} is null", nameof(other));
            if (false == typeof(IDataTransferObjectSerializer).IsAssignableFrom(SerializerType))
                throw new ArgumentException($"{SerializerType} does not implement {typeof(IDataTransferObjectSerializer)}");
        }

        internal static DataTransferObjectAttribute[] Discover(Type type, IDictionary<Type, IDataTransferObjectSerializer> serializers)
        {
            var attributes = type
                .GetCustomAttributes()
                .OfType<IDataTransferObjectMetadata>()
                .Select(metadata => new DataTransferObjectAttribute(metadata, type))
                .ToList();
            if (typeof(IBasicJsonDataTransferObject).IsAssignableFrom(type))
                attributes.Add(new DataTransferObjectAttribute(typeof(BasicJsonDataTransferObjectSerializer)));
            if (typeof(IBasicXmlDataTransferObject).IsAssignableFrom(type))
                attributes.Add(new DataTransferObjectAttribute(typeof(BasicXmlDataTransferObjectSerializer)));
            attributes = attributes
                .GroupBy(a => a.SerializerType)
                .Select(serializerGroup => serializerGroup
                    .OrderBy(a => a.IsDefault ? 0 : 1)
                    .First())
                .ToList();
            foreach (var attribute in attributes)
            {
                IDataTransferObjectSerializer CreateInstance()
                {
                    return ((IDataTransferObjectSerializer)Activator.CreateInstance(attribute.SerializerType))
                        .ThrowIfNull(() => new InvalidOperationException($"{attribute.SerializerType} could not be instantiated."))
                        .AsDataTransferObjectSerializer();
                }

                attribute.Serializer = serializers.GetOrAdd(attribute.SerializerType, CreateInstance);
            }

            attributes
                .GroupBy(a=> a.Serializer.ContentType)
                .Where(grp=> grp.Count() > 1)
                .ForEach(grp =>
                {
                    var error = new StringBuilder("Ambiguous Data Transfer Object declaration.");
                    error.Append($" Discovered conflicting '{grp.Key}' content type serialization handlers declared on {type}.");
                    throw new InvalidOperationException(error.ToString());
                });

            if (attributes.Count == 0) return Array.Empty<DataTransferObjectAttribute>();
            var defaultSerializersCount = attributes.Count(a => a.IsDefault);
            if (defaultSerializersCount == 0)
            {
                attributes[0].IsDefault = true;
            }
            else if(defaultSerializersCount > 1)
            {
                var error = new StringBuilder("Invalid Data Transfer Object declaration.");
                error.Append($" Discovered multiple default serializers declared on {type}.");
                throw new InvalidOperationException(error.ToString());
            }

            return attributes  
                .Do(a=> a.TargetType = type)
                .ToArray();
        }

        internal static Dictionary<Type, DataTransferObjectAttribute[]> Discover(IEnumerable<Type> types)
        {
            var serializers = new Dictionary<Type, IDataTransferObjectSerializer>();
            var result = types
                .ThrowIfNullArgument(nameof(types))
                .Select(t => new
                {
                    DtoType = t,
                    Attributes = Discover(t, serializers)
                })
                .Where(i => i.Attributes.Length > 0)
                .ToDictionary(i => i.DtoType, i => i.Attributes);
            return result;
        }

        internal IDataTransferObjectSerializer Serializer { get; private set; }


        internal Type TargetType { get; set; }

        public Type SerializerType { get; }

        public bool IsDefault { get; set; }



    }
}
