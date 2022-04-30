using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Caching
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IActiveCacheEntry<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<T?> GetAsync(CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ageTolerance"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<T?> GetAsync(TimeSpan ageTolerance, CancellationToken cancellation = default);
    }
}
