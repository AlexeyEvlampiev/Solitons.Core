using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Common
{
    public abstract class TransientStorage : ITransientStorage
    {
        protected abstract Task<string> UploadAsync(Stream stream, DateTimeOffset expiresOn, CancellationToken cancellation);
        protected abstract Task<Stream> DownloadAsync(string blobId, CancellationToken cancellation);

        public abstract IObservable<EntryExpiredEventArgs> GetExpiredEntries();


        [DebuggerStepThrough]
        Task<string> ITransientStorage.UploadAsync(Stream stream, DateTimeOffset expiresOn, CancellationToken cancellation)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            stream.ThrowIfCanNotReadArgument(nameof(stream));
            return UploadAsync(stream, expiresOn, cancellation);
        }

        [DebuggerStepThrough]
        Task<Stream> ITransientStorage.DownloadAsync(string blobId, CancellationToken cancellation)
        {
            blobId.ThrowIfNullOrWhiteSpaceArgument(nameof(blobId));
            cancellation.ThrowIfCancellationRequested();
            return DownloadAsync(blobId, cancellation);
        }
    }
}
