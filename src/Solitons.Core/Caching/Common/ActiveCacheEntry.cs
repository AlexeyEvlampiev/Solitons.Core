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
    public abstract class ActiveCacheEntry<T> : IActiveCacheEntry<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ageTolerance"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<T?> GetAsync(TimeSpan ageTolerance, CancellationToken cancellation = default);

        [DebuggerStepThrough]
        Task<T?> IActiveCacheEntry<T>.GetAsync(CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return GetAsync(TimeSpan.Zero, cancellation);
        }

        [DebuggerStepThrough]
        Task<T?> IActiveCacheEntry<T>.GetAsync(TimeSpan ageTolerance, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return GetAsync(ageTolerance, cancellation);
        }
    }
}
