using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    sealed class DatabaseRpcCommandLookup : IDatabaseRpcCommandLookup
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly Dictionary<Guid, Type> _commandTypeByOid;

        private DatabaseRpcCommandLookup(IEnumerable<Type> types)
        {
            _commandTypeByOid = types.ToDictionary(type => type.GUID);
        }

        [DebuggerStepThrough]
        public IDatabaseRpcCommandLookup Where(Func<Type, bool> filter) => new DatabaseRpcCommandLookup(GetTypes().Where(filter));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        public DatabaseRpcCommandLookup(IEnumerable<Assembly> assemblies)
        {
            _commandTypeByOid = assemblies
                .Distinct()
                .SelectMany(a=> a.GetTypes())
                .Where(type =>
                {
                    if (type.IsAbstract) return false;
                    return typeof(IDatabaseRpcCommand).IsAssignableFrom(type);
                })
                .ToDictionary(type => type.GUID);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public Type? FindCommandType(Guid oid)
        {
            if (_commandTypeByOid.TryGetValue(oid, out var type))
            {
                return type;
            }

            return null;
        }

        public IEnumerable<Type> GetTypes() => _commandTypeByOid.Values;
    }
}
