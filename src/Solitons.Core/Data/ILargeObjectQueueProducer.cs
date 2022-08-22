

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILargeObjectQueueProducer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <param name="preferredMethod"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<DataTransferMethod> SendAsync(
            DataTransferPackage package,
            DataTransferMethod preferredMethod = DataTransferMethod.ByValue,
            CancellationToken cancellation = default);

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
    }
}
