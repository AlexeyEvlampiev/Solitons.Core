using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Collections;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IDatabaseRpcCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool CanAccept(MediaContent request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<MediaContent> InvokeAsync(MediaContent request, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task SendAsync(MediaContent request, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        Task SendAsync(object dto, CancellationToken cancellation = default);
    }


    public partial interface IDatabaseRpcCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .Distinct()
                .SelectMany(a => a.GetTypes())
                .Where(type =>
                {
                    if (type.IsAbstract) return false;
                    return typeof(IDatabaseRpcCommand).IsAssignableFrom(type);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<Type> GetTypes(params Assembly[] assemblies) => GetTypes(assemblies.AsEnumerable());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<Type> GetTypes(Assembly assembly) => GetTypes(FluentArray.Create(assembly));
    }
}
