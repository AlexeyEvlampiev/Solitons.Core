using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Solitons.Data;
using static Solitons.Collections.FluentEnumerable;

namespace Solitons
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public partial interface IDomainContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDomainContractSerializer GetSerializer();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Strongly typed Transaction Script interface to be implemented.</typeparam>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        T Create<T>(ITransactionScriptProvider provider) where T : class;
    }

    public partial interface IDomainContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IDomainContext CreateGenericContext(params Type[] types) =>
            new GenericDomainContext(types
                .ThrowIfNullArgument(nameof(types)));

        /// <summary>
        /// Creates a generic instance of <see cref="DomainContext"/> built from the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IDomainContext CreateGenericContext(Assembly assembly) =>
            new GenericDomainContext(Yield(assembly
                .ThrowIfNullArgument(nameof(assembly))));

        /// <summary>
        /// Creates a generic instance of <see cref="DomainContext"/> built from the specified assemblies.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IDomainContext CreateGenericContext(params Assembly[] assemblies) =>
            new GenericDomainContext(assemblies
                .ThrowIfNullArgument(nameof(assemblies)));

        /// <summary>
        /// Creates a generic instance of <see cref="DomainContext"/> built from the specified assemblies.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IDomainContext CreateGenericContext(IEnumerable<Assembly> assemblies) =>
            new GenericDomainContext(assemblies
                .ThrowIfNullArgument(nameof(assemblies)));

    }
}
