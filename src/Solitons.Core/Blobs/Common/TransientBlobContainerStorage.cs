using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Common;

namespace Solitons.Blobs.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class TransientBlobContainerStorage : TransientStorage, IBlobTransientStorage
    {
        private readonly IClock _clock;
        private readonly INamingService _namingService;
        private readonly ConcurrentDictionary<string, IObservable<IBlobContainerClient>> _transientContainers = new(StringComparer.Ordinal);

        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        protected TransientBlobContainerStorage() 
            : this(new NamingService(), IClock.System)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        protected TransientBlobContainerStorage(IClock clock)
            : this(new NamingService(), clock.ThrowIfNullArgument(nameof(clock)))
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="namingService"></param>
        [DebuggerNonUserCode]
        protected TransientBlobContainerStorage(INamingService namingService) 
            : this(namingService, IClock.System)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="namingService"></param>
        /// <param name="clock"></param>
        [DebuggerNonUserCode]
        protected TransientBlobContainerStorage(INamingService namingService, IClock clock)
        {
            _namingService = namingService ?? throw new ArgumentNullException(nameof(namingService));
            _clock = clock ?? IClock.System;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        protected abstract IBlobContainerClient GetContainerClient(string containerName);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract IObservable<string> GetContainerNames();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task DeleteContainerAsync(string containerName, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="expiresOn"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected sealed override async Task<string> UploadAsync(Stream stream, DateTimeOffset expiresOn, CancellationToken cancellation)
        {
            expiresOn.ThrowIfArgumentLessThan(_clock.UtcNow, nameof(expiresOn));
            var containerName = _namingService
                .GenerateContainerName(expiresOn)
                .ThrowIfNullOrWhiteSpace(()=> new InvalidOperationException($"{_namingService.GetType()}.{nameof(_namingService.GenerateContainerName)} returned empty result."));

            var container = await _transientContainers.GetOrAdd(containerName, () =>
            {
                var ctr = GetContainerClient(containerName);
                return ctr.CreateIfNotExistsAsync(cancellation)
                    .ToObservable()
                    .Select(_ => ctr);
            });

            IBlobClient blob = container
                .GetBlobClient(_namingService.GenerateBlobName())
                .ThrowIfNull($"{container.GetType()}.{nameof(container.GetBlobClient)} returned null.");
            await blob.UploadAsync(stream, cancellation);
            return _namingService
                .BuildReceipt(container.Name, blob.Name)
                .ThrowIfNullOrWhiteSpace(()=> new InvalidOperationException());
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobId"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected sealed override async Task<Stream> DownloadAsync(string blobId, CancellationToken cancellation)
        {
            if (_namingService.TryParseReceipt(blobId, 
                out string containerName, 
                out string blobName,
                out DateTime expiresOn) &&
                expiresOn >= _clock.UtcNow)
            {
                var container = GetContainerClient(containerName);
                var blob = container.GetBlobClient(blobName);
                return await blob.DownloadAsync(cancellation);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IObservable<EntryExpiredEventArgs> GetExpiredEntries()
        {
            return GetContainerNames()
                .Where(_namingService.IsTransientContainerName)
                .TakeWhile(containerName =>
                {
                    var expiresOnUtc = _namingService.ExtractExpirationUtc(containerName);
                    return _clock.UtcNow > expiresOnUtc;
                })
                .Select(containerName =>
                {
                    return new EntryExpiredEventArgs(
                        containerName, (cancellationToken) => DeleteContainerAsync(containerName, cancellationToken));
                });
        }

        [DebuggerStepThrough]
        Uri IBlobTransientStorage.Sign(
            string receipt, 
            DateTimeOffset expiresOn, 
            IPAddress startAddress, 
            IPAddress endAddress)
        {
            receipt = receipt.ThrowIfNullOrWhiteSpaceArgument(nameof(receipt)).Trim();
            expiresOn.ThrowIfArgumentLessThan(_clock.UtcNow, nameof(expiresOn));
            startAddress ??= endAddress;
            endAddress ??= startAddress;

            if (_namingService.TryParseReceipt(receipt, out var containerName, out var blobName, out var storageExpiresOn))
            {
                expiresOn.ThrowIfArgumentGreaterThan(storageExpiresOn, nameof(expiresOn));
                var container = GetContainerClient(containerName);
                var blob = container.GetBlobClient(blobName);
                return blob.Sign(expiresOn, startAddress, endAddress);
            }

            throw new ArgumentException("Invalid blob reference format", nameof(receipt));
        }
    }

    public abstract partial class TransientBlobContainerStorage
    {
        /// <summary>
        /// 
        /// </summary>
        protected interface INamingService
        {
            string GenerateContainerName(DateTimeOffset expiresOn);
            bool TryParseReceipt(string receipt, out string containerName, out string blobName, out DateTime expiresOn);
            string BuildReceipt(string containerName, string blobName);
            string GenerateBlobName();
            bool IsTransientContainerName(string containerName);
            DateTime ExtractExpirationUtc(string containerName);
        }

        protected interface IBlobContainerClient
        {
            string Name { get; }
            Task CreateIfNotExistsAsync(CancellationToken cancellation);
            IBlobClient GetBlobClient(string blobName);
        }

        protected interface IBlobClient
        {
            string Name { get; }

            Task UploadAsync(Stream stream, CancellationToken cancellation);
            Task<Stream> DownloadAsync(CancellationToken cancellation);
            Uri Sign(DateTimeOffset expiresOn, IPAddress startAddress, IPAddress endAddress);
        }

        protected class NamingService : INamingService
        {
            private const string Postfix = "704938aaf52643fc9b60ede209c23ca5";
            private readonly Regex _receiptRegex = new(@$"^(?<blob>[^@\s]+)@(?<container>(?<date>\d{{12}}){Postfix})$");
            private readonly Regex _containerNameRegex = new Regex($@"^(?<date>\d{{12}}){Postfix}$");

            [DebuggerNonUserCode]
            public NamingService()
            {
            }

            protected virtual DateTimeOffset Ceiling(DateTimeOffset expiresOn) => expiresOn.AddDays(1.1).Date;

            public string GenerateContainerName(DateTimeOffset expiresOn)
            {
                var ceiling = Ceiling(expiresOn);
                if (ceiling - expiresOn < TimeSpan.FromSeconds(1))
                    throw new InvalidOperationException($"{GetType()}.{nameof(Ceiling)} returned) invalid value.");
                ceiling = ceiling.AddMilliseconds((-ceiling.Millisecond));
                ceiling = ceiling.AddSeconds((-ceiling.Second));
                return $"{ceiling:yyyyMMddhhmm}{Postfix}";
            }

            public bool TryParseReceipt(string receipt, out string containerName, out string blobName, out DateTime expiresOn)
            {
                var match = _receiptRegex.Match(receipt);
                if (match.Success)
                {
                    containerName = match.Groups["container"].Value;
                    blobName = match.Groups["blob"].Value;
                    var dateString = match.Groups["date"].Value;
                    var status = DateTime.TryParseExact(
                        dateString,
                        "yyyyMMddhhmm",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out expiresOn);
                    return status;
                }

                containerName = null;
                blobName = null;
                expiresOn = default;
                return false;
            }


            public string BuildReceipt(string containerName, string blobName) => $"{blobName}@{containerName}";
            public string GenerateBlobName() => Guid.NewGuid().ToString("N");
            public bool IsTransientContainerName(string containerName) => containerName?.Contains(Postfix) == true;

            public DateTime ExtractExpirationUtc(string containerName)
            {
                var match = _containerNameRegex.Match(containerName);
                if (match.Success)
                {
                    var dateString = match.Groups["date"].Value;
                    if (DateTime.TryParseExact(
                        dateString,
                        "yyyyMMddhhmm",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var expiresOn))
                    {
                        return expiresOn;
                    }
                }
                return DateTime.MaxValue;
            }
        }
    }
}
