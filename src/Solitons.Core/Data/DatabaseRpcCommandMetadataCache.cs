using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Solitons.Data
{
    internal sealed class DatabaseRpcCommandMetadataCache
    {
        private readonly ReaderWriterLockSlim _lock = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HashSet<Assembly> _assemblies = new();

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly Dictionary<Type, DatabaseRpcCommandMetadata> _metadata = new();

        [DebuggerStepThrough]
        public DatabaseRpcCommandMetadata GetOrCreate(Type rpcType) => GetOrCreate(rpcType, 0);

        private DatabaseRpcCommandMetadata GetOrCreate(Type rpcType, int reentryCount)
        {
            if (reentryCount > 1)
            {
                throw new InvalidOperationException($"{nameof(reentryCount)} > 1");
            }

            if (false == _lock.TryEnterReadLock(1000))
            {
                throw new TimeoutException($"Read lock could not be taken");
            }
            try
            {
                if (_metadata.TryGetValue(rpcType, out var metadata))
                {
                    Debug.Assert(_assemblies.Contains(rpcType.Assembly));
                    Debug.Assert(metadata != null);
                    return metadata;
                }

                if (_assemblies.Contains(rpcType.Assembly))
                {
                    DatabaseRpcCommandMetadata.Get(rpcType);
                    throw new InvalidOperationException();
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }

            Register(rpcType.Assembly);

            return GetOrCreate(rpcType, (reentryCount + 1));
        }



        private void Register(Assembly assembly)
        {
            if (false == _lock.TryEnterWriteLock(3000))
            {
                throw new TimeoutException();
            }

            try
            {
                if (_assemblies.Contains(assembly))
                {
                    return;
                }

                var metadata = DatabaseRpcCommandMetadata.Get(assembly);
                foreach (var m in metadata)
                {
                    _metadata[m.CommandType] = m;
                }

                _assemblies.Add(assembly);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
