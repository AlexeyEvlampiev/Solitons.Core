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

        internal static Dictionary<Type, DataTransferObjectAttribute[]> Discover(
            IEnumerable<Type> domainTypes, 
            Dictionary<Type, DataTransferObjectAttribute[]> externalTypes)
        {
            if (domainTypes == null) throw new ArgumentNullException(nameof(domainTypes));
            externalTypes ??= new Dictionary<Type, DataTransferObjectAttribute[]>();

            var serializers = new Dictionary<Type, IDataTransferObjectSerializer>();

            var allAttributes = externalTypes
                .SelectMany(pair => pair.Value)
                .Select(external =>
                {
                    if (serializers.TryGetValue(external.SerializerType, out var instance))
                    {
                        external.Serializer = instance;
                    }
                    else
                    {
                        instance = ((IDataTransferObjectSerializer) Activator.CreateInstance(external.SerializerType))
                            .ThrowIfNull(()=> new InvalidOperationException($"{external.SerializerType} could not be instantiated."))
                            .AsDataTransferObjectSerializer();
                        external.Serializer = instance;
                        serializers.Add(external.SerializerType, external.Serializer);
                    }

                    return external;
                })
                .ToList();

            var list = new List<DataTransferObjectAttribute>();
            foreach (var domainType in domainTypes)
            {
                if(domainType.IsAbstract)continue;
                list.Clear();
                list.AddRange(domainType
                    .GetCustomAttributes()
                    .OfType<IDataTransferObjectMetadata>()
                    .Select(metadata=> new DataTransferObjectAttribute(metadata, domainType)));
                if(list.Count == 0)continue;

                if (IsDefined(domainType, typeof(GuidAttribute)) == false)
                {
                    throw new InvalidOperationException(
                        new StringBuilder($"Missing {typeof(GuidAttribute)}.")
                            .Append($" See {domainType} type declaration.")
                            .ToString());
                }
                if (list.Count  == 1)
                {
                    list[0].IsDefault = true;
                }

                list.Count(a => a.IsDefault)
                    .ThrowIfOutOfRange(1, 1, 
                        () => new InvalidOperationException(
                            new StringBuilder($"Exactly one of the applied {typeof(DataTransferObjectAttribute)} attributes is required to have {nameof(IDataTransferObjectMetadata.IsDefault)} set to true.")
                                .Append($" See {domainType} declaration.")
                                .ToString()));

                foreach (var attribute in list)
                {
                    var serializer = serializers
                        .GetOrAdd(attribute.SerializerType, ()=> (IDataTransferObjectSerializer)Activator
                            .CreateInstance(attribute.SerializerType));
                    attribute.Serializer = serializer;
                }

                allAttributes.AddRange(list);
            }

            return allAttributes
                .GroupBy(att => att.TargetType)
                .ToDictionary(group => group.Key, group => group.ToArray());
        }

        internal IDataTransferObjectSerializer Serializer { get; private set; }


        internal Type TargetType { get; }

        public Type SerializerType { get; }

        public bool IsDefault { get; set; }



    }
}
