using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons
{
    sealed class DomainTransientStorage : IDomainTransientStorage
    {
        public const string StreamContentType = "application/octet-stream";
        private readonly ITransientStorage _transientStorage;
        private readonly IDomainSerializer _serializer;

        public DomainTransientStorage(
            ITransientStorage innerStorage,
            IDomainSerializer serializer)
        {
            _transientStorage = innerStorage ?? throw new ArgumentNullException(nameof(innerStorage));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }


        public async Task<DomainTransientStorageReceipt> UploadAsync(object dto, TimeSpan expiresAfter, int minStorageBytes, CancellationToken cancellation = default)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            expiresAfter.ThrowIfArgumentLessThan(TimeSpan.Zero, nameof(expiresAfter));
            cancellation.ThrowIfCancellationRequested();

            if (dto is Stream stream)
            {
                return await UploadAsync(stream, expiresAfter, cancellation);
            }

            if (dto is byte[] bytes)
            {
                await using var memory = new MemoryStream(bytes);
                return await UploadAsync(memory, expiresAfter, cancellation);
            }

            if (false == _serializer.CanSerialize(dto, out _))
                throw new ArgumentException($"{dto.GetType()} does not belong to the underlined domain.");

            var dtoBytes = _serializer
                .Serialize(dto, out var contentType)
                .ToBytes(Encoding.UTF8);

            var method = (dtoBytes.Length > minStorageBytes)
                ? DataTransferMethod.ByReference
                : DataTransferMethod.ByValue;

            if (method == DataTransferMethod.ByReference)
            {
                var blobId = await _transientStorage.UploadAsync(dtoBytes, expiresAfter, cancellation);
                return new DomainTransientStorageReceipt(method, blobId, contentType, dto.GetType().GUID);
            }
            else
            {
                var base64 = dtoBytes.ToBase64String();
                var receipt = new DomainTransientStorageReceipt(method, base64, contentType, dto.GetType().GUID);
                if (receipt.ToArray().Length < minStorageBytes)
                    return receipt;
                var blobId = await _transientStorage.UploadAsync(dtoBytes, expiresAfter, cancellation);
                return new DomainTransientStorageReceipt(method, blobId, contentType, dto.GetType().GUID);
            }
        }

        public async Task<DomainTransientStorageReceipt> UploadAsync(Stream stream, TimeSpan expiresAfter, CancellationToken cancellation = default)
        {
            stream
                .ThrowIfNullArgument(nameof(stream))
                .ThrowIfCanNotReadArgument(nameof(stream));
            expiresAfter
                .ThrowIfArgumentLessThan(TimeSpan.Zero, nameof(expiresAfter));
            cancellation
                .ThrowIfCancellationRequested();
            var blobId = await _transientStorage.UploadAsync(stream, expiresAfter, cancellation);
            return new DomainTransientStorageReceipt(DataTransferMethod.ByReference, blobId, StreamContentType, typeof(Stream).GUID);
        }

        public async Task<object> DownloadAsync(DomainTransientStorageReceipt receipt, CancellationToken cancellation = default)
        {
            if (receipt == null) throw new ArgumentNullException(nameof(receipt));
            cancellation.ThrowIfCancellationRequested();

            switch (receipt.DataTransferMethod)
            {
                case (DataTransferMethod.ByReference):
                    if (StreamContentType.Equals(receipt.ContentType, StringComparison.OrdinalIgnoreCase))
                    {
                        return await _transientStorage.DownloadAsync(receipt.Content, cancellation);
                    }
                    else
                    {
                        await using var stream = await _transientStorage.DownloadAsync(receipt.Content, cancellation);
                        using var reader = new StreamReader(stream);
                        var dtoString = await reader.ReadToEndAsync();
                        return _serializer.Deserialize(receipt.DtoTypeId, receipt.ContentType, dtoString);
                    }
                case (DataTransferMethod.ByValue):
                    {
                        var content = receipt.Content
                            .AsBase64Bytes()
                            .ToUtf8String();
                        return _serializer.Deserialize(receipt.DtoTypeId, receipt.ContentType, content);
                    }
                default:
                    throw new NotSupportedException($"{receipt.DataTransferMethod}");
            }           
        }

        public bool CanUpload(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
