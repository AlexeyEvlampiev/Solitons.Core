using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    sealed class DatabaseRpcProviderProxy : IDatabaseRpcProvider
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IDatabaseRpcProvider _innerProvider;

        private DatabaseRpcProviderProxy(IDatabaseRpcProvider provider)
        {
            _innerProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public static IDatabaseRpcProvider Wrap(IDatabaseRpcProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            return provider is DatabaseRpcProviderProxy proxy
                ? proxy
                : new DatabaseRpcProviderProxy(provider);
        }

        [DebuggerStepThrough]
        public Task<string> InvokeAsync(DbCommandAttribute info, string payload, CancellationToken cancellation)
        {
            return _innerProvider.InvokeAsync(info, payload, cancellation);
        }

        [DebuggerStepThrough]
        public string Serialize(object request, string contentType)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return _innerProvider.Serialize(request, contentType);
        }

        [DebuggerStepThrough]
        public object Deserialize(string content, string contentType, Type type)
        {
            return _innerProvider.Deserialize(content, contentType, type);
        }

        public bool CanSerialize(Type type, string contentType)
        {
            return _innerProvider.CanSerialize(type, contentType);
        }

        [DebuggerStepThrough]
        public override string ToString() => _innerProvider.ToString() ?? base.ToString()!;

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerProvider.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerProvider.GetHashCode();
    }
}
