using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Caching.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReadThroughCache<T> : IReadThroughCache<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxEntityAge"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<T?> GetAsync(TimeSpan maxEntityAge, CancellationToken cancellation = default);

        [DebuggerStepThrough]
        Task<T?> IReadThroughCache<T>.ReadAsync(CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return GetAsync(TimeSpan.Zero, cancellation);
        }

        [DebuggerStepThrough]
        Task<T?> IReadThroughCache<T>.ReadAsync(TimeSpan maxTtl, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return GetAsync(maxTtl, cancellation);
        }
    }
}
