using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Caching
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of the entity to cache</typeparam>
    public partial interface IReadThroughCache<T> where T : class
    {
        /// <summary>
        /// Reads an entity object through the underlying cache.
        /// </summary>
        /// <param name="maxTtl">Cache duration tolerance. Exceeding this threshold shall trigger an automatic entity update.</param>
        /// <param name="cancellation">Cancellation token</param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        Task<T?> ReadAsync(TimeSpan maxTtl, CancellationToken cancellation = default);
    }

    public partial interface IReadThroughCache<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public Task<T?> ReadAsync(CancellationToken cancellation = default)
        {
            cancellation.ThrowIfCancellationRequested();
            return ReadAsync(TimeSpan.Zero, cancellation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxTtl">Cache duration tolerance. Exceeding this threshold shall trigger an automatic entity update.</param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">An entity could not be read</exception>
        [DebuggerStepThrough]
        public async Task<T> ReadFirstAsync(TimeSpan maxTtl, CancellationToken cancellation = default)
        {
            return await ReadAsync(maxTtl, cancellation)
                ?? throw new InvalidOperationException($"{typeof(T)} entity could not be read.");
        }
    }
}
