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
    public abstract class EntityCacheClient<T> : IEntityCacheClient<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxEntityAge"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<T?> GetAsync(TimeSpan maxEntityAge, CancellationToken cancellation = default);

        [DebuggerStepThrough]
        Task<T?> IEntityCacheClient<T>.GetAsync(CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return GetAsync(TimeSpan.Zero, cancellation);
        }

        [DebuggerStepThrough]
        Task<T?> IEntityCacheClient<T>.GetAsync(TimeSpan maxEntityAge, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return GetAsync(maxEntityAge, cancellation);
        }
    }
}
