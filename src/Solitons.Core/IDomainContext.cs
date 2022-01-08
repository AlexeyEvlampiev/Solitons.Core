using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

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
        IDomainSerializer GetSerializer();

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
            new GenericDomainContext(assembly
                .ThrowIfNullArgument(nameof(assembly))
                .ToEnumerable());

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
