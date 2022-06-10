using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Solitons.Collections;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseRpcCommandFactory
    {
        private readonly Func<Type, object> _innerFactory;
        private readonly Dictionary<Guid, Type> _commandTypeByOid;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerFactory"></param>
        /// <param name="assemblies"></param>
        public DatabaseRpcCommandFactory(Func<Type, object> innerFactory, IEnumerable<Assembly> assemblies)
        {
            _innerFactory = innerFactory;
            _commandTypeByOid = IDatabaseRpcCommand
                .GetTypes(assemblies)
                .ToDictionary(type => type.GUID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerFactory"></param>
        /// <param name="assemblies"></param>
        [DebuggerStepThrough]
        public DatabaseRpcCommandFactory(Func<Type, object> innerFactory, params Assembly[] assemblies)
            : this(innerFactory, assemblies.AsEnumerable())
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerFactory"></param>
        /// <param name="assembly"></param>
        [DebuggerStepThrough]
        public DatabaseRpcCommandFactory(Func<Type, object> innerFactory, Assembly assembly)
            : this(innerFactory, FluentArray.Create(assembly))
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IDatabaseRpcCommand Create(Guid oid)
        {
            if (_commandTypeByOid.TryGetValue(oid, out var type))
            {
                return (IDatabaseRpcCommand)_innerFactory.Invoke(type);
            }

            throw new InvalidOperationException(
                new StringBuilder("Could not instantiate the requested Database RPC Command object.")
                    .Append($" Command ID: {oid}.")
                    .ToString());
        }
    }
}
