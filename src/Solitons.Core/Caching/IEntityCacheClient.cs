using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Caching
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityCacheClient<T>
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
        /// <param name="maxEntityAge"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<T?> GetAsync(TimeSpan maxEntityAge, CancellationToken cancellation = default);
    }
}
