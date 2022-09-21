using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IDatabaseRpcModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        bool Contains(Guid commandId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        IDatabaseRpcCommand GetCommand(Guid commandId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<MediaContent> InvokeAsync(Guid commandId, MediaContent request, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="content"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        Task SendAsync(Guid commandId, MediaContent content, CancellationToken cancellation = default);
    }

    public partial interface IDatabaseRpcModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool Contains<T>() where T : IDatabaseRpcCommand => Contains(typeof(T).GUID);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        [DebuggerStepThrough]
        public T GetCommand<T>() where T : IDatabaseRpcCommand
        {
            return (T)GetCommand(typeof(T).GUID);
        }
    }
}
