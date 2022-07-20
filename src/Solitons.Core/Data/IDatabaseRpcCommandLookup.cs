using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Solitons.Collections;
using Solitons.Data.Common;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public interface IDatabaseRpcCommandLookup
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="oid"></param>
    /// <returns></returns>
    Type? FindCommandType(Guid oid);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<Type> GetTypes();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceAssemblies"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static IDatabaseRpcCommandLookup Create(IEnumerable<Assembly> sourceAssemblies)
    {
        return new DatabaseRpcCommandLookup(sourceAssemblies);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceAssembly"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static IDatabaseRpcCommandLookup Create(Assembly sourceAssembly)
    {
        return new DatabaseRpcCommandLookup(FluentArray
            .Create(sourceAssembly)
            .AsEnumerable());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceAssembly"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static IDatabaseRpcCommandLookup Create(params Assembly[] sourceAssembly)
    {
        return new DatabaseRpcCommandLookup(sourceAssembly.AsEnumerable());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    [DebuggerNonUserCode]
    public IDatabaseRpcCommandFactory AsFactory(IServiceProvider serviceProvider)
    {
        return new DatabaseRpcCommandFactory(this, serviceProvider);
    }
}
