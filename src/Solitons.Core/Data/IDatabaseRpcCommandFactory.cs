using System;
using System.Collections.Generic;
using System.Reflection;
using Solitons.Collections;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public partial interface IDatabaseRpcCommandFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="oid"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    IDatabaseRpcCommand Create(Guid oid);
}

public partial interface IDatabaseRpcCommandFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="innerFactory"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IDatabaseRpcCommandFactory Create(
        Func<Type, IDatabaseRpcCommand> innerFactory,
        IEnumerable<Assembly> assemblies)
    {
        return new DatabaseRpcCommandFactory(innerFactory, assemblies);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="innerFactory"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IDatabaseRpcCommandFactory Create(
        Func<Type, IDatabaseRpcCommand> innerFactory,
        Assembly assembly)
    {
        return new DatabaseRpcCommandFactory(innerFactory, FluentArray.Create(assembly));
    }
}