using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using Azure.Storage.Blobs;
using Solitons.Common;

namespace Solitons.Samples.Azure
{
    public sealed class BlobStorageAsyncLogger : AsyncLogger
    {
        private readonly BlobContainerClient _container;

        public BlobStorageAsyncLogger(BlobContainerClient container)
        {
            _container = container.ThrowIfNullArgument(nameof(container));
        }


        protected override async Task LogAsync(ILogEntry entry)
        {
            var dto = entry.AsDataTransferObject();
            var blobName = $"{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}";
            var bytes = dto.ToJsonString().ToBytes(Encoding.UTF8);
            await _container.UploadBlobAsync(blobName, new BinaryData(bytes));
        }

        public async Task MigrateAsync(Func<ILogEntry, Task> callback, CancellationToken cancellation)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            cancellation.ThrowIfCancellationRequested();
            while (cancellation.IsCancellationRequested == false)
            {
                await foreach (var item in _container.GetBlobsAsync(cancellationToken: cancellation))
                {
                    var blob = _container.GetBlobClient(item.Name);
                    var json = await blob
                        .DownloadContentAsync(cancellation)
                        .ToObservable()
                        .Select(download=> download.Value.Content.ToString());
                    var entry = LogEntryData.Parse(json);
                    await callback.Invoke(entry);
                }
                await Task.Delay(TimeSpan.FromSeconds(5), cancellation);
            }
            cancellation.ThrowIfCancellationRequested();
        }

    }
}
