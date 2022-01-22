using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DomainTransientStorage : IDomainTransientStorage2
    {
        private const string ContentTypeKey = "ContentType";
        private const string ReferenceKey = "Reference";
        private const string SchemaKey = "Schema";
        private readonly IDomainContractSerializer _contractSerializer;

        [DebuggerNonUserCode]
        protected DomainTransientStorage(IDomainContractSerializer contractSerializer)
        {
            _contractSerializer = contractSerializer ?? throw new ArgumentNullException(nameof(contractSerializer));
        }
        protected abstract Task<string> SaveAsync(Stream stream, DateTimeOffset expiresOn, CancellationToken cancellation);

        protected abstract Task<Stream> LoadAsync(string receipt, CancellationToken cancellation);


        
        async Task<string> IDomainTransientStorage2.UploadAsync(object dto, string contentType, DateTimeOffset expiresOn, CancellationToken cancellation)
        {
            dto.ThrowIfNullArgument(nameof(dto));
            expiresOn.ThrowIfArgumentLessThan(DateTimeOffset.UtcNow, nameof(expiresOn));
            cancellation.ThrowIfCancellationRequested();

            var typeId = (dto is Stream || dto is byte[])
                ? Guid.Empty
                : dto.GetType().GUID;

            using var content =
                dto is Stream ? Disposable.Empty :
                dto is byte[] bytes ? bytes.ToMemoryStream() :
                _contractSerializer.Serialize(dto, contentType)
                    .ToMemoryStream(Encoding.UTF8);
            var stream = content as Stream ?? dto as Stream;
            stream.ThrowIfCanNotReadArgument(nameof(dto));

            var address = (await SaveAsync(stream, expiresOn, cancellation))
                .ThrowIfNullOrWhiteSpace(()=> new InvalidOperationException($"{GetType()}.{nameof(SaveAsync)} returned null async result."));
            var metadata = new Dictionary<string, string>()
            {
                [ReferenceKey] = address,
                ["Schema"] = typeId.ToString("N"),
                ["ContentType"] = contentType
            };
            return JsonSerializer.Serialize(metadata);
        }

        async Task<object> IDomainTransientStorage2.DownloadAsync(string receipt, CancellationToken cancellation)
        {
            receipt.ThrowIfNullOrWhiteSpaceArgument(nameof(receipt));
            cancellation.ThrowIfCancellationRequested();
            var metadata = JsonSerializer
                .Deserialize<Dictionary<string, string>>(receipt)
;
            if (metadata != null &&
                metadata.TryGetValue(ReferenceKey, out var reference) &&
                metadata.TryGetValue(SchemaKey, out var schemaGuid) &&
                metadata.TryGetValue(ContentTypeKey, out var contentType) &&
                Guid.TryParse(schemaGuid, out var typeId))
            {
                if (typeId == Guid.Empty)
                    return await LoadAsync(reference, cancellation);
                using var reader = new StreamReader(await LoadAsync(reference, cancellation));
                var content = await reader.ReadToEndAsync();
                return _contractSerializer.Deserialize(typeId, contentType, content);
            }

            throw new ArgumentException($"Melformed storage receipt", nameof(receipt));
        }

        [DebuggerNonUserCode]
        bool IDomainTransientStorage2.CanSave(object dto, string contentType)
        {
            if (dto is Stream stream) return stream.CanRead;
            return dto is byte[] || _contractSerializer.CanSerialize(dto, contentType);
        }

        [DebuggerNonUserCode]
        bool IDomainTransientStorage2.CanSave(object dto, out string contentType)
        {
            if (dto is Stream stream)
            {
                contentType = stream.CanRead ? "application/octet-stream" : null;
                return contentType is not null;
            }

            if (dto is byte[])
            {
                contentType = "application/octet-stream";
                return true;
            }

            return _contractSerializer.CanSerialize(dto, out contentType);
        }
    }
}
