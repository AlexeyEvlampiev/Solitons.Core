using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface ITransientStorage
    {
        /// <summary>
        /// Uploads the specified bytes to this transient storage asynchronously.
        /// </summary>
        /// <param name="stream">The bytes to be uploaded.</param>
        /// <param name="expiresOn"></param>
        /// <param name="cancellation">The upload cancellation token.</param>
        /// <returns>The uploaded blob ID.</returns>
        Task<string> UploadAsync(Stream stream, DateTimeOffset expiresOn, CancellationToken cancellation = default);

        /// <summary>
        /// Downloads persisted bytes sequence identified with the given upload receipt asynchronously.
        /// </summary>
        /// <param name="blobId">The upload receipt.</param>
        /// <param name="cancellation">The download cancellation token.</param>
        /// <returns>The downloaded bytes.</returns>
        Task<Stream> DownloadAsync(string blobId, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IObservable<EntryExpiredEventArgs> GetExpiredEntries();
    }

    public partial interface ITransientStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="expiresOn"></param>
        /// <param name="cancellation"></param>
        /// <returns>The uploaded blob ID.</returns>
        [DebuggerStepThrough]
        public async Task<string> UploadAsync(
            byte[] bytes,
            DateTimeOffset expiresOn,
            CancellationToken cancellation = default)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            cancellation.ThrowIfCancellationRequested();
            await using var stream = bytes.ToMemoryStream();
            return await UploadAsync(stream, expiresOn, cancellation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="expiresAfter"></param>
        /// <param name="cancellation"></param>
        /// <returns>The uploaded blob ID.</returns>
        [DebuggerStepThrough]
        public Task<string> UploadAsync(
            Stream stream,
            TimeSpan expiresAfter,
            CancellationToken cancellation = default) =>
            UploadAsync(stream, DateTimeOffset.UtcNow.Add(expiresAfter), cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="expiresAfter"></param>
        /// <param name="cancellation"></param>
        /// <returns>The uploaded blob ID.</returns>
        [DebuggerStepThrough]
        public async Task<string> UploadAsync(
            byte[] bytes,
            TimeSpan expiresAfter,
            CancellationToken cancellation = default)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            expiresAfter.ThrowIfArgumentLessThan(TimeSpan.Zero, nameof(expiresAfter));
            cancellation.ThrowIfCancellationRequested();
            await using var stream = bytes.ToMemoryStream();
            return await UploadAsync(stream, DateTimeOffset.UtcNow.Add(expiresAfter), cancellation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dueTime"></param>
        /// <param name="scanPeriod"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IObservable<EntryExpiredEventArgs> GetExpiredEntryNotifications(DateTimeOffset dueTime, TimeSpan scanPeriod)
        {
            scanPeriod.ThrowIfArgumentLessThan(TimeSpan.Zero, nameof(scanPeriod));
            return Observable
                .Timer(dueTime, scanPeriod)
                .SelectMany(tick => GetExpiredEntries());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scanPeriod"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IObservable<EntryExpiredEventArgs> GetExpiredEntryNotifications(TimeSpan scanPeriod)
            => GetExpiredEntryNotifications(DateTimeOffset.UtcNow, scanPeriod);
    }
}
