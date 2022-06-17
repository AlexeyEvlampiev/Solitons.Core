using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILargeObjectQueue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="preferredMethod"></param>
        /// <param name="config"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<DataTransferMethod> SendAsync(
            object dto,
            DataTransferMethod preferredMethod = DataTransferMethod.ByValue,
            Action<DataTransferPackage>? config = default,
            CancellationToken cancellation = default);

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
