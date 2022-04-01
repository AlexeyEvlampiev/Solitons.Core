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
        public Task<object> InvokeAsync(
            DbCommandAttribute annotation,
            object payload,
            IDataContractSerializer serializer,
            Func<object, Task>? onResponse,
            CancellationToken cancellation)
        {
            return _innerProvider.InvokeAsync(annotation, payload, serializer, onResponse, cancellation);
        }



        [DebuggerStepThrough]
        public override string ToString() => _innerProvider.ToString() ?? base.ToString()!;

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerProvider.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerProvider.GetHashCode();

        
    }
}
