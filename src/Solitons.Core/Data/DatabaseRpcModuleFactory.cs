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
    public sealed class DatabaseRpcModuleFactory
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly Dictionary<Guid, Type> _commandTypes = new();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="customFilter"></param>
        [DebuggerStepThrough]
        public DatabaseRpcModuleFactory(Assembly assembly, Func<Type, bool>? customFilter = null)
            : this(FluentEnumerable.Yield(assembly), customFilter)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="customFilter"></param>
        [DebuggerStepThrough]
        public DatabaseRpcModuleFactory(IEnumerable<Assembly> assemblies, Func<Type, bool>? customFilter = null) 
            : this(assemblies
                .Distinct()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IDatabaseRpcCommand).IsAssignableFrom(type))
                .Where(type => false == (type.IsInterface || type.IsAbstract))
                .Where(type => customFilter?.Invoke(type) ?? true))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandTypes"></param>
        public DatabaseRpcModuleFactory(IEnumerable<Type> commandTypes)
        {
            foreach (var commandType in commandTypes.Distinct())
            {
                if (commandType.IsInterface || 
                    commandType.IsAbstract)
                {
                    throw new InvalidOperationException(new StringBuilder("Invalid RPC command type.")
                        .Append(" Interfaces and asbtract classes cannot be registered as RPC command types.")
                        .Append($" See type {commandType}.")
                        .ToString());
                }

                if (typeof(IDatabaseRpcCommand).IsAssignableFrom(commandType))
                {
                    _commandTypes[commandType.GUID] = commandType;
                    return;
                }

                throw new InvalidOperationException(new StringBuilder("Invalid RPC command type.")
                    .Append($" The {commandType} type does not implement the required {typeof(IDatabaseRpcCommand)} interface.")
                    .ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public IEnumerable<Type> GetCommandTypes() => _commandTypes.Values;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public IDatabaseRpcModule Create(IServiceProvider provider) => new DatabaseRpcModule(_commandTypes, provider);
    }
}
