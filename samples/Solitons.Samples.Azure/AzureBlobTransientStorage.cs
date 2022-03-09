using Azure.Storage.Blobs;
using Solitons.Common;
using Solitons.Data;

namespace Solitons.Samples.Azure
{
    public sealed class AzureBlobTransientStorage : TransientStorage
    {
        private readonly BlobContainerClient _stagingContainer;

        public AzureBlobTransientStorage(BlobContainerClient stagingContainer)
        {
            _stagingContainer = stagingContainer ?? throw new ArgumentNullException(nameof(stagingContainer));
        }

        protected override async Task<TransientStorageReceipt> UploadAsync(Stream stream, TimeSpan expiresAfter, CancellationToken cancellation = default)
        {
            var expiresOn = DateTime.UtcNow.Add(expiresAfter);
            var blob = _stagingContainer.GetBlobClient($"{expiresOn.Ticks}-{Guid.NewGuid():N}");
            await blob.UploadAsync(stream, overwrite: true, cancellation);
            return new TransientStorageReceipt(this, blob.Name, DateTimeOffset.UtcNow.Add(expiresAfter));
        }

        protected override async Task<Stream> DownloadAsync(TransientStorageReceipt receipt, CancellationToken cancellation = default)
        {
            receipt.ExpiresOnUtc
                .ThrowIfLessThan(DateTime.UtcNow, ()=> 
                    new ArgumentException($"The specified receipt is expired.", nameof(receipt)));

            var blob = _stagingContainer.GetBlobClient(receipt.Token);
            var result = await blob.DownloadAsync(cancellation);
            return result.Value.Content;
        }
    }
}
