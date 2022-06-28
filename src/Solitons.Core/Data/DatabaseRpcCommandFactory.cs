using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class DatabaseRpcCommandFactory : IDatabaseRpcCommandFactory
    {
        private readonly Func<Type, IDatabaseRpcCommand> _innerFactory;
        private readonly Dictionary<Guid, Type> _commandTypeByOid;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerFactory"></param>
        /// <param name="assemblies"></param>
        public DatabaseRpcCommandFactory(Func<Type, IDatabaseRpcCommand> innerFactory, IEnumerable<Assembly> assemblies)
        {
            _innerFactory = innerFactory;
            _commandTypeByOid = IDatabaseRpcCommand
                .GetTypes(assemblies)
                .ToDictionary(type => type.GUID);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IDatabaseRpcCommand Create(Guid oid)
        {
            if (_commandTypeByOid.TryGetValue(oid, out var type))
            {
                return _innerFactory.Invoke(type);
            }

            throw new ArgumentOutOfRangeException(nameof(oid),
                new StringBuilder("Could not instantiate the requested Database RPC Command object.")
                    .Append($" Command ID: {oid}.")
                    .ToString());
        }
    }
}
