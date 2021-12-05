using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Solitons.Blobs.Common;

namespace Solitons.Azure.Referencelmpl
{
    public sealed class TransientAzureBlobContainerStorage : TransientBlobContainerStorage
    {
        private readonly BlobServiceClient _innerClient;

        [DebuggerNonUserCode]
        public TransientAzureBlobContainerStorage(string connectionString)
        {
            _innerClient = new BlobServiceClient(
                connectionString
                    .ThrowIfNullOrWhiteSpaceArgument(nameof(connectionString)));
        }


        [DebuggerNonUserCode]
        public TransientAzureBlobContainerStorage(BlobServiceClient innerClient)
        {
            _innerClient = innerClient ?? throw new ArgumentNullException(nameof(innerClient));
        }

        [DebuggerNonUserCode]
        internal TransientAzureBlobContainerStorage(string connectionString, IClock clock) : base(clock)
        {
            _innerClient = new BlobServiceClient(
                connectionString
                    .ThrowIfNullOrWhiteSpaceArgument(nameof(connectionString)));
        }

        [DebuggerNonUserCode]
        internal TransientAzureBlobContainerStorage(BlobServiceClient innerClient, IClock clock) : base(clock)
        {
            _innerClient = innerClient ?? throw new ArgumentNullException(nameof(innerClient));
        }


        [DebuggerNonUserCode]
        public static ITransientStorage Create(string connectionString) =>
            new TransientAzureBlobContainerStorage(
                connectionString
                    .ThrowIfNullOrWhiteSpaceArgument(nameof(connectionString)));

        [DebuggerNonUserCode]
        internal static ITransientStorage Create(string connectionString, IClock clock)
        {
            return new TransientAzureBlobContainerStorage(
                connectionString
                    .ThrowIfNullOrWhiteSpaceArgument(nameof(connectionString)),
                clock);
        }


        protected override IBlobContainerClient GetContainerClient(string containerName) =>
            new BlobContainerProxy(_innerClient.GetBlobContainerClient(containerName));

        protected override IObservable<string> GetContainerNames()
        {
            return Observable.Create<string>(SubscribeAsync);

            async Task SubscribeAsync(IObserver<string> observer, CancellationToken cancellation)
            {
                await foreach (var item in _innerClient.GetBlobContainersAsync(cancellationToken: cancellation))
                {
                    observer.OnNext(item.Name);
                }

                observer.OnCompleted();
            }
        }

        protected override Task DeleteContainerAsync(string containerName, CancellationToken cancellation) =>
            _innerClient.DeleteBlobContainerAsync(containerName, cancellationToken: cancellation);

        sealed class BlobContainerProxy : IBlobContainerClient
        {
            private readonly BlobContainerClient _innerClient;

            public BlobContainerProxy(BlobContainerClient innerClient)
            {
                _innerClient = innerClient ?? throw new ArgumentNullException(nameof(innerClient));
            }

            public string Name => _innerClient.Name;

            public Task CreateIfNotExistsAsync(CancellationToken cancellation) =>
                _innerClient.CreateIfNotExistsAsync(cancellationToken: cancellation);

            public IBlobClient GetBlobClient(string blobName) => new BlobProxy(_innerClient.GetBlobClient(blobName));
        }

        sealed class BlobProxy : IBlobClient
        {
            private readonly BlobClient _innerClient;

            public BlobProxy(BlobClient innerClient)
            {
                _innerClient = innerClient ?? throw new ArgumentNullException(nameof(innerClient));
            }

            public string Name => _innerClient.Name;

            public Task UploadAsync(Stream stream, CancellationToken cancellation) =>
                _innerClient.UploadAsync(stream, cancellationToken: cancellation);

            public async Task<Stream> DownloadAsync(CancellationToken cancellation)
            {
                var response = await _innerClient.DownloadAsync(cancellation);
                return response?.Value?.Content;
            }

            public Uri Sign(DateTimeOffset expiresOn, IPAddress startAddress, IPAddress endAddress)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = _innerClient.GetParentBlobContainerClient().Name,
                    BlobName = _innerClient.Name,
                    Resource = "b"
                };

                sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
                sasBuilder.SetPermissions(BlobSasPermissions.Read |
                                          BlobSasPermissions.Write);

                Uri sasUri = _innerClient.GenerateSasUri(sasBuilder);
                return sasUri;
            }
        }
    }
}
