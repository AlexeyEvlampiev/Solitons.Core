using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IDomainTransientStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CanUpload(Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="expiresAfter"></param>
        /// <param name="minStorageBytes"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<DomainTransientStorageReceipt> UploadAsync(object dto, TimeSpan expiresAfter, int minStorageBytes, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="expiresAfter"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<DomainTransientStorageReceipt> UploadAsync(Stream stream, TimeSpan expiresAfter, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receipt"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<object> DownloadAsync(DomainTransientStorageReceipt receipt, CancellationToken cancellation = default);

    }

    public partial interface IDomainTransientStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="expiresAfter"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public async Task<DomainTransientStorageReceipt> UploadAsync(byte[] bytes, TimeSpan expiresAfter, CancellationToken cancellation = default)
        {
            bytes.ThrowIfNullArgument(nameof(bytes));
            expiresAfter.ThrowIfArgumentLessThan(TimeSpan.Zero, nameof(expiresAfter));
            cancellation.ThrowIfCancellationRequested();
            await using var stream = new MemoryStream(bytes);
            return await UploadAsync(stream, expiresAfter, cancellation);
        }
    }
}
