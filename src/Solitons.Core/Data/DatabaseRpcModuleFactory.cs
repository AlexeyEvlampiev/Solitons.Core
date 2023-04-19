using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Solitons.Collections;

namespace Solitons.Data;

/// <summary>
/// A factory class that creates instances of the <see cref="DatabaseRpcModule"/> class.
/// </summary>
public sealed class DatabaseRpcModuleFactory
{
    /// <summary>
    /// A dictionary that stores the types of the registered RPC commands by their GUIDs.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly Dictionary<Guid, Type> _commandTypes = new();


    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseRpcModuleFactory"/> class using the specified <paramref name="assembly"/> and optional <paramref name="customFilter"/>.
    /// </summary>
    /// <param name="assembly">The assembly that contains the types to register as RPC commands.</param>
    /// <param name="customFilter">A filter function that determines whether a type should be registered as an RPC command.</param>
    [DebuggerStepThrough]
    public DatabaseRpcModuleFactory(Assembly assembly, Func<Type, bool>? customFilter = null)
        : this(FluentEnumerable.Yield(assembly), customFilter)
    {
            
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseRpcModuleFactory"/> class using the specified <paramref name="assemblies"/> and optional <paramref name="customFilter"/>.
    /// </summary>
    /// <param name="assemblies">The collection of assemblies that contain the types to register as RPC commands.</param>
    /// <param name="customFilter">A filter function that determines whether a type should be registered as an RPC command.</param>
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
    /// Initializes a new instance of the <see cref="DatabaseRpcModuleFactory"/> class using the specified collection of <paramref name="commandTypes"/>.
    /// </summary>
    /// <param name="commandTypes">The collection of types to register as RPC commands.</param>
    public DatabaseRpcModuleFactory(IEnumerable<Type> commandTypes)
    {
        foreach (var commandType in commandTypes.Distinct())
        {
            if (commandType.IsInterface || 
                commandType.IsAbstract)
            {
                throw new InvalidOperationException(new StringBuilder("Invalid RPC context type.")
                    .Append(" Interfaces and asbtract classes cannot be registered as RPC context types.")
                    .Append($" See type {commandType}.")
                    .ToString());
            }

            if (typeof(IDatabaseRpcCommand).IsAssignableFrom(commandType))
            {
                _commandTypes[commandType.GUID] = commandType;
            }
            else
            {
                throw new InvalidOperationException(new StringBuilder("Invalid RPC context type.")
                    .Append($" The {commandType} type does not implement the required {typeof(IDatabaseRpcCommand)} interface.")
                    .ToString());
            }
        }
    }

    /// <summary>
    /// Returns a collection of the types of the registered RPC commands.
    /// </summary>
    /// <returns>A collection of the types of the registered RPC commands.</returns>
    [DebuggerNonUserCode]
    public IEnumerable<Type> GetCommandTypes() => _commandTypes.Values;

    /// <summary>
    /// Creates a new instance of the <see cref="DatabaseRpcModule"/> class with the specified service provider and registered command types.
    /// </summary>
    /// <param name="provider">The service provider.</param>
    /// <returns>A new instance of the <see cref="DatabaseRpcModule"/> class.</returns>
    [DebuggerNonUserCode]
    public IDatabaseRpcModule Create(IServiceProvider provider) => new DatabaseRpcModule(_commandTypes, provider);
}