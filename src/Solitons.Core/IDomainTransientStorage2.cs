using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IDomainTransientStorage2
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="contentType"></param>
        /// <param name="expiresOn"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<string> UploadAsync(object dto, string contentType, DateTimeOffset expiresOn, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receipt"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<object> DownloadAsync(string receipt, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        bool CanSave(object dto, string contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        bool CanSave(object dto, out string contentType);
    }

    /// <summary>
    /// 
    /// </summary>
    public partial interface IDomainTransientStorage2 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="contentType"></param>
        /// <param name="expiresAfter"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public Task<string> UploadAsync(object dto, string contentType, TimeSpan expiresAfter, CancellationToken cancellation = default)
        {
            dto.ThrowIfNullArgument(nameof(dto));
            contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType));
            expiresAfter.ThrowIfArgumentLessThan(TimeSpan.Zero, nameof(expiresAfter));
            cancellation.ThrowIfCancellationRequested();

            return UploadAsync(
                dto,
                contentType,
                DateTimeOffset.UtcNow.Add(expiresAfter),
                cancellation);
        }
    }
}
