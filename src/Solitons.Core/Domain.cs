using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Solitons.Common;
using Solitons.Queues;
using Solitons.Web;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class Domain : IEnumerable<Assembly>
    {
        private readonly Type[] _types;
        private readonly HashSet<Assembly> _assemblies;
        private readonly Lazy<IEnumerable<RoleSetAttribute>> _roleSets;
        private readonly Lazy<IDomainSerializer> _serializer;
        private readonly Lazy<Dictionary<Type, IWebQueryConverter>> _webQueryConverterByRestApiAttributeType;
        private readonly Lazy<Dictionary<Type, DbTransactionAttribute[]>> _dbTransactionsByType;
        private readonly Lazy<Dictionary<Type, BlobSecureAccessSignatureMetadata[]>> _sasPermissions;
        private readonly Lazy<Dictionary<Type, DataTransferObjectAttribute[]>> _dataTransferObjectTypes;
        private readonly Dictionary<Type, DataTransferObjectAttribute[]> _externalDtoTypes = new();


        /// <summary>
        /// Initializes a new instance of the <see cref="Domain"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <exception cref="System.ArgumentException">Domain assembly list is required - assemblies</exception>
        protected Domain(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            _assemblies = assemblies
                .ThrowIfNullArgument(nameof(assemblies))
                .Union(typeof(Domain).Assembly.ToEnumerable())
                .ToHashSet();
            _types = _assemblies
                .SelectMany(a=> a.GetTypes())
                .ToArray();
            if (_types.Length == 0)
                throw new ArgumentException($"Domain assembly list is required", nameof(assemblies));
            _roleSets = new Lazy<IEnumerable<RoleSetAttribute>>(DiscoverRoleSets);
            _webQueryConverterByRestApiAttributeType = new Lazy<Dictionary<Type, IWebQueryConverter>>(()=> WebQueryConverter.Discover(_types));
            _dbTransactionsByType = new Lazy<Dictionary<Type, DbTransactionAttribute[]>>(()=> DbTransactionAttribute.Discover(_types));
            _sasPermissions = new Lazy<Dictionary<Type, BlobSecureAccessSignatureMetadata[]>>(()=> BlobSecureAccessSignatureMetadata.Discover(_types));
            _dataTransferObjectTypes = new Lazy<Dictionary<Type, DataTransferObjectAttribute[]>>(() => DataTransferObjectAttribute.Discover(_types, _externalDtoTypes));
            _serializer = new Lazy<IDomainSerializer>(() => DomainSerializer.Create(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        [DebuggerStepThrough]
        protected void RegisterExternalDto<T>(Type type) where T : IDataContractSerializer, new()
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            RegisterExternalDto(type, new DataTransferObjectMetadata<T>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="metadata"></param>
        [DebuggerStepThrough]
        protected void RegisterExternalDto(Type type, IDataTransferObjectMetadata metadata)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (metadata == null) throw new ArgumentNullException(nameof(metadata));
            RegisterExternalDto(type, new []{metadata});
        }

        protected void RegisterExternalDto(Type type, params IDataTransferObjectMetadata[] metadata)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if ((metadata?.Length ?? 0) == 0) throw new ArgumentException($"Metadata collection is required.", nameof(metadata));
            if(metadata.Length < 1) throw new ArgumentNullException($"{nameof(metadata.Length)} ", nameof(metadata));
            if (_externalDtoTypes.ContainsKey(type))
                throw new InvalidOperationException($"{type} is already registered as an external Data Transfer Object");
            if(_assemblies.Contains(type.Assembly))
                throw new InvalidOperationException($"{type} cannot be an external Data Transfer Object as it is a member of {type.Assembly} - domain assembly.");

            var attributes = metadata
                .Select(m=> new DataTransferObjectAttribute(type,m))
                .ToArray();
            if (attributes.All(a => a.IsDefault == false))
                attributes[0].IsDefault = true;
            attributes
                .Where(a => a.IsDefault)
                .ThrowIfCountExceeds(1, () => new ArgumentException("Multiple default metadata items.", nameof(metadata)));
            _externalDtoTypes.Add(type, attributes);
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
        public IEnumerable<RoleSetAttribute> GetRoleSets() => _roleSets.Value;

        public IEnumerable<T> GetSecureAccessSignatureDeclarations<T>() where T : ISecureAccessSignatureMetadata
        {
            if (_sasPermissions.Value.TryGetValue(typeof(T), out var array))
            {
                return array.OfType<T>();
            }

            return Enumerable.Empty<T>();
        }

        public IDomainSerializer GetSerializer() => _serializer.Value;

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
        /// <returns></returns>
        internal IEnumerable<Type> GetDataTransferObjectTypes() => _dataTransferObjectTypes.Value.Keys;

        internal DataTransferObjectAttribute[] GetDataTransferAttributes(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return _dataTransferObjectTypes.Value.TryGetValue(type, out var attributes)
                ? attributes
                : Array.Empty<DataTransferObjectAttribute>();
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




    }

    public abstract partial class Domain
    {
        sealed record DataTransferObjectMetadata<T> : IDataTransferObjectMetadata
            where T : IDataContractSerializer, new()
        {
            public Type SerializerType => typeof(T);
            public bool IsDefault => false;
        }
    }
}
