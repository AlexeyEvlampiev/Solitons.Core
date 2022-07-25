using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILargeObjectQueueConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task ProcessAsync(
            ILargeObjectQueueConsumerCallback callback,
            CancellationToken cancellation);
    }
}
