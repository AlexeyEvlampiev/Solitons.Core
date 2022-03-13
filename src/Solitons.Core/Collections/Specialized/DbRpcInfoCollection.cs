using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Solitons.Collections.Specialized
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DbRpcInfoCollection : IEnumerable<DbRpcInfo>
    {
        private readonly Dictionary<Guid, DbRpcInfo> _infoByOid = new();
        private readonly Dictionary<string, DbRpcInfo> _infoByProcedure = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Type, HashSet<string>> _requiredContentTypes = new();
        private readonly HashSet<DbRpcInfo> _infos = new();



        protected DbRpcInfo Add<TRequest, TResponse>(string guid, string procedure, Action<Options>? config = null)
        {
            guid = guid.ThrowIfNullOrWhiteSpaceArgument(nameof(guid));
            if (false == Guid.TryParse(guid, out var oid))
                throw new ArgumentOutOfRangeException();
            oid.ThrowIfEmptyArgument(nameof(guid));

            if (_infoByOid.TryGetValue(oid, out var duplicate))
            {
                throw new InvalidOperationException(new StringBuilder("Duplicate command oid")
                    //TODO: extend
                    .ToString());
            }

            procedure = procedure
                .ThrowIfNullOrWhiteSpaceArgument(nameof(procedure))
                .Trim();

            if (_infoByProcedure.TryGetValue(procedure, out duplicate))
            {
                throw new InvalidOperationException(new StringBuilder("Duplicate command name")
                    //TODO: extend
                    .ToString());
            }

            if (Attribute.IsDefined(typeof(TRequest), typeof(GuidAttribute)) == false)
            {
                throw new InvalidOperationException(new StringBuilder($"Missing {typeof(GuidAttribute)} annotation.")
                    //TODO: extend
                    .ToString());
            }

            if (Attribute.IsDefined(typeof(TResponse), typeof(GuidAttribute)) == false)
            {
                throw new InvalidOperationException(new StringBuilder($"Missing {typeof(GuidAttribute)} annotation.")
                    //TODO: extend
                    .ToString());
            }

            var info = new DbRpcInfo(oid, procedure, typeof(TRequest), typeof(TResponse));
            _infos.Add(info);
            _infoByOid.Add(oid, info);
            _infoByProcedure.Add(procedure, info);

            var options = new Options(oid, procedure, typeof(TRequest), typeof(TResponse));
            config?.Invoke(options);
            info.IsolationLevel = options.IsolationLevel;
            info.EnableAsyncExecution = options.EnableAsyncExecution;
            info.AuthorizedRoles = new ImmutableArray<string>().AddRange(options.AuthorizedRoles);
            info.CommandTimeout = options.CommandTimeout;

            RegisterContentType(info.RequestType, info.RequestContentType);
            RegisterContentType(info.ResponseType, info.ResponseContentType);
            return info;
        }


        public IEnumerable<Type> DataContractTypes => _requiredContentTypes.Keys;

        public IEnumerable<string> GetRequiredContentTypes(Type dataContractType)
        {
            if (dataContractType == null) throw new ArgumentNullException(nameof(dataContractType));
            if (_requiredContentTypes.TryGetValue(dataContractType, out var set))
            {
                return set.AsEnumerable();
            }

            throw new ArgumentOutOfRangeException(nameof(dataContractType));
        }


        private void RegisterContentType(Type type, string contentType)
        {
            if (false == _requiredContentTypes.TryGetValue(type, out var set))
            {
                _requiredContentTypes[type] = 
                    (set = new HashSet<string>(StringComparer.OrdinalIgnoreCase));
            }

            set.Add(contentType);
        }

        public sealed class Options
        {
            private readonly HashSet<string> _authorizedRoles = new(StringComparer.OrdinalIgnoreCase);

            public Options(Guid oid, string procedure, Type requestType, Type responseType)
            {
                Oid = oid;
                Procedure = procedure;
                RequestType = requestType;
                ResponseType = responseType;
                IsolationLevel = IsolationLevel.ReadCommitted;
                CommandTimeout = TimeSpan.FromSeconds(30);
                Id = procedure;
                Description = procedure;
            }

            public Options With(IsolationLevel isolationLevel)
            {
                IsolationLevel = isolationLevel;
                return this;
            }

            public Options WithCommandTimeout(TimeSpan timeout)
            {
                if(timeout <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(timeout));
                CommandTimeout = timeout;
                return this;
            }

            public Options WithAsyncExecution()
            {
                EnableAsyncExecution = true;
                return this;
            }

            public Options AuthorizeRole(string role)
            {
                _authorizedRoles.Add(role
                    .ThrowIfNullOrWhiteSpaceArgument(nameof(role))
                    .Trim());
                return this;
            }

            public Options WithId(string id)
            {
                Id = id
                    .ThrowIfNullOrWhiteSpaceArgument(nameof(id))
                    .Trim();
                return this;
            }

            public Options WithDescription(string description)
            {
                Description = description
                    .ThrowIfNullOrWhiteSpaceArgument(nameof(description))
                    .Trim();
                return this;
            }

            public Guid Oid { get; }
            public string Procedure { get; }
            public Type RequestType { get; }
            public Type ResponseType { get; }

            public string Id { get; private set; }
            public string Description { get; private set; }
            public IsolationLevel IsolationLevel { get; private set; }
            public bool EnableAsyncExecution { get; private set; }
            public TimeSpan CommandTimeout { get; private set; }
            public IEnumerable<string> AuthorizedRoles => _authorizedRoles.AsEnumerable();
        }

        IEnumerator<DbRpcInfo> IEnumerable<DbRpcInfo>.GetEnumerator()
        {
            return _infos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_infos).GetEnumerator();
        }
    }
}
