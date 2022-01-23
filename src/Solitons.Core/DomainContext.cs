using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Solitons.Collections;
using Solitons.Common;
using Solitons.Data;
using Solitons.Queues;

namespace Solitons
{
    /// <summary>
    /// Provides domain services based on types discovered in the scoped assemblies.
    /// <para>See domain type annotations:</para>
    /// <list type="bullet">
    /// <item>
    /// <description>Data contracts: <see cref="DataTransferObjectAttribute"/>, <see cref="IDataTransferObjectMetadata"/></description>
    /// </item>
    /// <item>
    /// </item>
    /// </list>
    /// </summary>
    public abstract partial class DomainContext : IDomainContext
    {
        #region Private Fields

        private readonly Type[] _types;
        private readonly HashSet<Assembly> _assemblies;
        private readonly Lazy<Dictionary<Attribute, Type>> _typesByAttribute;

        private static readonly ConcurrentDictionary<Type, WeakReference> WeakReferences = new();


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDomainContractSerializer> _serializer;


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Dictionary<Type, DataTransferObjectAttribute[]>> _dataTransferObjectTypes;



        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<Type, IDataTransferObjectSerializer> _specializedSerializersBySerializerType = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Regex _jsonLikeRegex = new(@"(?i)\bjson\b");

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainContext"/> class.
        /// </summary>
        /// <param name="assemblies">Domain type assemblies.</param>
        /// <exception cref="System.ArgumentException">Domain assembly list is required - assemblies</exception>
        [DebuggerStepThrough]
        protected DomainContext(IEnumerable<Assembly> assemblies) : this(assemblies
            .ThrowIfNullArgument(nameof(assemblies))
            .SelectMany(a=> a.GetTypes()))
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        [DebuggerStepThrough]
        protected DomainContext(Assembly assembly) : this(FluentEnumerable.Yield(assembly
           .ThrowIfNullArgument(nameof(assembly)))
           .SelectMany(a => a.GetTypes()))
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DomainContext"/> class.
        /// </summary>
        /// <param name="types">Domain types</param>
        protected DomainContext(IEnumerable<Type> types)
        {
            var typesArray = types
                .ThrowIfNullArgument(nameof(types))
                .Where(MatchDomainTypeCriteria)
                .ToArray();
            if (typesArray.Length == 0)
                throw new ArgumentException($"There are no types in the given collection that match the domain type criteria.", nameof(types));

            _types = typesArray;
            _assemblies = typesArray
                .Select(t=> t.Assembly)
                .ToHashSet();

            _typesByAttribute = new Lazy<Dictionary<Attribute, Type>>(() =>
            {
                var result = new Dictionary<Attribute, Type>();
                foreach(var type in typesArray)
                {
                    foreach(var att in type.GetCustomAttributes())
                    {
                        result.Add(att, type);
                    }
                }
                return result;
            });

            _dataTransferObjectTypes = new Lazy<Dictionary<Type, DataTransferObjectAttribute[]>>(() => _types
                .Select(type=> KeyValuePair.Create(type, DiscoverDataTransferObjectAttributes(type)))
                .Where(pair=>pair.Value.Any())
                .ToDictionary());
            _serializer = new Lazy<IDomainContractSerializer>(() => DomainContractSerializer.Create(this));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IDataTransferObjectMetadata> GetDataTransferObjectMetadata(Type type) => type
            .ThrowIfNullArgument(nameof(type))
            .GetCustomAttributes()
            .OfType<IDataTransferObjectMetadata>();


        internal IReadOnlyDictionary<Type, DataTransferObjectAttribute[]> GetDataTransferObjectTypes() => 
            new ReadOnlyDictionary<Type, DataTransferObjectAttribute[]>(_dataTransferObjectTypes.Value);


        private DataTransferObjectAttribute[] DiscoverDataTransferObjectAttributes(Type type)
        {
            var attributes = GetDataTransferObjectMetadata(type)
                .ThrowIfNull(()=> new NullReferenceException($"{GetType()}.{nameof(GetDataTransferObjectMetadata)} returned null."))
                .Select(metadata => new DataTransferObjectAttribute(metadata, type))
                .ToList();

            if (typeof(IBasicJsonDataTransferObject).IsAssignableFrom(type) && 
                false == attributes.Any(a=> a.SerializerType == typeof(BasicJsonDataTransferObjectSerializer)))
            {
                // Implicit basic JSON serialization declaration
                attributes.Add(new DataTransferObjectAttribute(typeof(BasicJsonDataTransferObjectSerializer)));
            }

            if (typeof(IBasicXmlDataTransferObject).IsAssignableFrom(type) &&
                false == attributes.Any(a => a.SerializerType == typeof(BasicXmlDataTransferObjectSerializer)))
            {
                // Implicit basic XML serialization declaration
                attributes.Add(new DataTransferObjectAttribute(typeof(BasicXmlDataTransferObjectSerializer)));
            }

            if (attributes.Count == 0)
                return Array.Empty<DataTransferObjectAttribute>();

            if(Attribute.IsDefined(type, typeof(GuidAttribute)) == false)
            {
                throw new InvalidOperationException(new StringBuilder("Missing type ID declaration.")
                    .Append($" The {type} type is declared as Data Transfer Object contract, but is missing the required {typeof(GuidAttribute)} marker.")
                    .Append($" Make sure that the type is annotated with its unique GUID identifier.")
                    .ToString());
            }

            if(attributes.Any(a => a.SerializerType == typeof(BasicXmlDataTransferObjectSerializer)))
            {
                var ctor = type
                    .GetConstructor(Array.Empty<Type>())
                    .ThrowIfNull(()=> new InvalidOperationException(new StringBuilder("Missing the default constructor.")
                        .Append($" The {type} type is declared as Basic XML Data Transfer Object contract, however is missing the required default constructor.")
                        .ToString()));
            }


            // Skip duplicate serializer declarations, prioritazing declarations appointed as default
            attributes = attributes
                .GroupBy(a => a.SerializerType)
                .Select(serializerGroup => serializerGroup
                    .OrderBy(a => a.IsDefault ? 0 : 1)   
                    .First())
                .ToList();

            attributes
                .GroupBy(a =>
                {
                    var serializer = GetDataTransferObjectSerializer(a.SerializerType);
                    return serializer.ContentType;
                })
                .Where(grp => grp.Count() > 1)
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
                attributes
                    .OrderBy(a =>
                    {
                        if (typeof(BasicJsonDataTransferObject).IsAssignableFrom(type) ||
                            typeof(BasicJsonDataTransferRecord).IsAssignableFrom(type))
                        {
                            return a.SerializerType == typeof(BasicJsonDataTransferObjectSerializer) ? 0 : 1;
                        }
                        else if (typeof(BasicXmlDataTransferObject).IsAssignableFrom(type))
                        {
                            return a.SerializerType == typeof(BasicXmlDataTransferObjectSerializer) ? 0 : 1;
                        }
                        return 1;
                    })
                    .ThenBy(a =>
                    {
                        var serializer = GetDataTransferObjectSerializer(a.SerializerType);
                        var value =  StringComparer.OrdinalIgnoreCase.Equals(serializer.ContentType, "application/json")
                            ? 0 : _jsonLikeRegex.IsMatch(serializer.ContentType)
                            ? 1 
                            : 2;
                        return value;
                    })
                    .Take(1)
                    .ForEach(a=> a.IsDefault = true);
            }
            else if (defaultSerializersCount > 1)
            {
                var error = new StringBuilder("Invalid Data Transfer Object declaration.");
                error.Append($" Discovered multiple default serializers declared on {type}.");
                throw new InvalidOperationException(error.ToString());
            }

            Debug.Assert(attributes.Count(a => a.IsDefault) == 1);
            return attributes
                .ToArray();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="transientStorage"></param>
        /// <returns></returns>
        public IDomainQueue BuildQueue(IQueueServiceProvider provider, ITransientStorage transientStorage)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (transientStorage == null) throw new ArgumentNullException(nameof(transientStorage));
            return new DomainQueue(this, provider, transientStorage);
        }

        /// <summary>
        /// Gets an instance of the <see cref="IDomainContractSerializer"/> class assembled by this <see cref="DomainContext"/> object.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IDomainContractSerializer GetSerializer() => _serializer.Value;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T Create<T>(ITransactionScriptProvider provider) where T : class
        {
            provider = provider.ThrowIfNullArgument(nameof(provider));
            //return TransactionScriptApi.Create<T>(provider, _serializer.Value);
            throw new NotImplementedException();
        }




        internal IDataTransferObjectSerializer GetDataTransferObjectSerializer(Type serializerType)
        {
            if (_specializedSerializersBySerializerType.TryGetValue(serializerType, out var serializer))
                return serializer;
            serializer = ((IDataTransferObjectSerializer)Activator.CreateInstance(serializerType))
                        .ThrowIfNull(() => new InvalidOperationException($"{serializerType} could not be instantiated."))
                        .AsDataTransferObjectSerializer();
            _specializedSerializersBySerializerType.Add(serializerType, serializer);
            return serializer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetTypes() => _types.AsEnumerable();




        /// <summary>
        /// 
        /// </summary>
        /// <param name="transientStorage"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IDomainTransientStorage CreateDomainTransientStorage(ITransientStorage transientStorage) => 
            new DomainTransientStorage(
                transientStorage.ThrowIfNullArgument(nameof(transientStorage)), 
                GetSerializer());
        


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Assemblies: {_assemblies.Select(a => a.GetName().Name).Join()}.";

        private static bool MatchDomainTypeCriteria(Type type) => type.IsAbstract == false && type.IsCOMObject == false;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        protected static T GetOrCreate<T>(Func<T> factory) where T : DomainContext
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            [DebuggerStepThrough]
            T Create() => factory
                .Invoke()
                .ThrowIfNull(() => new NullReferenceException($"{typeof(T)} factory returned null."));

            var reference = WeakReferences.GetOrAdd(typeof(T), ()=> new WeakReference(Create()));

            var instance = reference.Target;
            if (instance is null)
                reference.Target = instance = Create();
            return (T)instance;
        }

    }
}
