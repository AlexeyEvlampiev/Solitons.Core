using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public  interface IDatabaseRpcProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="parseResponse"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<T> InvokeAsync<T>(
            DatabaseRpcCommandMetadata metadata, 
            string request, 
            Func<string, Task<T>> parseResponse, 
            CancellationToken cancellation);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, Func<Task> callback, CancellationToken cancellation);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="ctsToken"></param>
        /// <returns></returns>
        Task ProcessQueueAsync(string queueName, CancellationToken ctsToken);
    }
}
