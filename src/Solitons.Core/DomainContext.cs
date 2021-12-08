using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Solitons.Common;
using Solitons.Queues;
using Solitons.Web;

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
    /// <description>Url query parameter: <see cref="Solitons.Web.QueryParameterAttribute"/></description>
    /// </item>
    /// </list>
    /// </summary>
    public abstract partial class DomainContext : IEnumerable<Assembly>
    {
        #region Private Fields

        private readonly Type[] _types;
        private readonly HashSet<Assembly> _assemblies;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IEnumerable<RoleSetAttribute>> _roleSets;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<IDomainSerializer> _serializer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Dictionary<Type, IWebQueryConverter>> _webQueryConverterByRestApiAttributeType;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Dictionary<Type, DbTransactionAttribute[]>> _dbTransactionsByType;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Dictionary<Type, BlobSecureAccessSignatureMetadata[]>> _sasPermissions;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Dictionary<Type, DataTransferObjectAttribute[]>> _dataTransferObjectTypes;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<Type, IDataTransferObjectSerializer> _specializedSerializersBySerializerType = new Dictionary<Type, IDataTransferObjectSerializer>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Regex _jsonLikeRegex = new Regex(@"(?i)\bjson\b");

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
            
            _roleSets = new Lazy<IEnumerable<RoleSetAttribute>>(DiscoverRoleSets);
            _webQueryConverterByRestApiAttributeType = new Lazy<Dictionary<Type, IWebQueryConverter>>(() => WebQueryConverter.Discover(_types));
            _dbTransactionsByType = new Lazy<Dictionary<Type, DbTransactionAttribute[]>>(() => DbTransactionAttribute.Discover(_types));
            _sasPermissions = new Lazy<Dictionary<Type, BlobSecureAccessSignatureMetadata[]>>(() => BlobSecureAccessSignatureMetadata.Discover(_types));
            _dataTransferObjectTypes = new Lazy<Dictionary<Type, DataTransferObjectAttribute[]>>(() => _types
                .Select(type=> KeyValuePair.Create(type, DiscoverDataTransferObjectAttributes(type)))
                .Where(pair=>pair.Value.Any())
                .ToDictionary());
            _serializer = new Lazy<IDomainSerializer>(() => DomainSerializer.Create(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DomainContext CreateGenericContext(params Type[] types) =>
            new GenericDomainContext(types
                .ThrowIfNullArgument(nameof(types)));

        /// <summary>
        /// Creates a generic instance of <see cref="DomainContext"/> built from the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DomainContext CreateGenericContext(Assembly assembly) =>
            new GenericDomainContext(assembly
                .ThrowIfNullArgument(nameof(assembly))
                .ToEnumerable());

        /// <summary>
        /// Creates a generic instance of <see cref="DomainContext"/> built from the specified assemblies.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DomainContext CreateGenericContext(params Assembly[] assemblies) =>
            new GenericDomainContext(assemblies
                .ThrowIfNullArgument(nameof(assemblies)));

        /// <summary>
        /// Creates a generic instance of <see cref="DomainContext"/> built from the specified assemblies.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DomainContext CreateGenericContext(IEnumerable<Assembly> assemblies) =>
            new GenericDomainContext(assemblies
                .ThrowIfNullArgument(nameof(assemblies)));
        
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
        /// Gets the role sets.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleSetAttribute> GetRoles() => _roleSets.Value;

        public IEnumerable<T> GetSecureAccessSignatureDeclarations<T>() where T : ISecureAccessSignatureMetadata
        {
            if (_sasPermissions.Value.TryGetValue(typeof(T), out var array))
            {
                return array.OfType<T>();
            }

            return Enumerable.Empty<T>();
        }

        /// <summary>
        /// Gets an instance of the <see cref="IDomainSerializer"/> class assembled by this <see cref="DomainContext"/> object.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IDomainSerializer GetSerializer() => _serializer.Value;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IWebQueryConverter GetWebQueryConverter<T>() where T : IHttpTriggerMetadata
        {
            if (_webQueryConverterByRestApiAttributeType.Value.TryGetValue(typeof(T), out var converter))
            {
                return converter;
            }

            throw new InvalidOperationException();
        }

        public IEnumerable<T> GetDbCommands<T>() where T : DbTransactionAttribute
        {
            if (_dbTransactionsByType.Value.TryGetValue(typeof(T), out var transactions))
            {
                return transactions.OfType<T>();
            }

            return Enumerable.Empty<T>();
        }

        private IEnumerable<RoleSetAttribute> DiscoverRoleSets()
        {
            var roleSets =
                from e in _types
                where e.IsEnum
                let att = RoleSetAttribute.Get(e)
                where att != null
                select att;
            return roleSets.Distinct().ToArray().AsEnumerable();
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

        IEnumerator<Assembly> IEnumerable<Assembly>.GetEnumerator() => _assemblies.AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _assemblies.GetEnumerator();


        public IEnumerable<Type> GetTypes() => _types.AsEnumerable();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IEnumerable<T> GetHttpTriggers<T>() where T : IHttpTriggerMetadata => HttpTriggerAttribute.Discover(_types).OfType<T>();


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THttpTrigger">API scope</typeparam>
        /// <typeparam name="TDbTransaction">Database scope</typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public Dictionary<THttpTrigger, TDbTransaction> GetHttpTriggers<THttpTrigger, TDbTransaction>() 
            where THttpTrigger : IHttpTriggerMetadata
            where TDbTransaction : IDbTransactionMetadata
        {
            var pairs =
                from t in _types
                let webAction = HttpTriggerAttribute.Get(t).OfType<THttpTrigger>().SingleOrDefault()
                let dbTransaction = DbTransactionAttribute.Get(t).OfType<TDbTransaction>().SingleOrDefault()
                where webAction is not null && dbTransaction is not null
                select KeyValuePair.Create(webAction, dbTransaction);
            return pairs.ToDictionary();
        }


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
    }

    public abstract partial class DomainContext
    {
        sealed record DataTransferObjectMetadata<T> : IDataTransferObjectMetadata
            where T : IDataTransferObjectSerializer, new()
        {
            public Type SerializerType => typeof(T);
            public bool IsDefault => false;
        }
    }
}
