using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CloudQueueBase : ICloudQueueProducer
    {
        private const string ReceiptKey = "receipt";
        private const string ContentTypeKey = "contentType";
        private const string SchemaIdKey = "schemaId";
        private const string SchemaNameKey = "schemaName";
        private const string IntentIdKey = "intentId";
        private const string IntentNameKey = "intentName";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="visibilityTimeout"></param>
        /// <param name="messageTtl"></param>
        /// <returns></returns>
        protected abstract Task SendAsync(byte[] body, TimeSpan? visibilityTimeout, TimeSpan? messageTtl);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        protected abstract string Serialize(object dto, string contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        protected abstract string Serialize(object dto, out string contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="expiresAfter"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<TransientStorageReceipt> StoreAsync(byte[] bytes, TimeSpan expiresAfter, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract QueueMessageOptions CreateMessageOptions();

        async Task<TransientStorageReceipt> ICloudQueueProducer.SendAsync(
            object dto, 
            Action<QueueMessageOptions>? config, 
            CancellationToken cancellation)
        {
            dto = dto.ThrowIfNullArgument(nameof(dto));
            cancellation.ThrowIfCancellationRequested();
            var options = CreateMessageOptions()
                .ThrowIfNull(() => new NullReferenceException($"{GetType()}.{nameof(CreateMessageOptions)} returned null."));

            config?.Invoke(options);
            string content;
            if (options.ContentType.IsNullOrWhiteSpace())
            {
                content = Serialize(dto, out var contentType);
                options.ContentType = contentType;
            }
            else
            {
                content = Serialize(dto, options.ContentType);
            }

            var bytes = content
                .ThrowIfNullOrWhiteSpace(() => new InvalidOperationException($"{GetType()}.{nameof(Serialize)} returned null or white space string."))
                .ToUtf8Bytes();

            var receipt = bytes.Length < options.MaxMessageSize 
                ? TransientStorageReceipt.CreateInMemoryStorageReceipt(bytes)
                : await StoreAsync(bytes, options.MessageTimeToLive, cancellation);

            var type = dto.GetType();
            var fields = new Dictionary<string, string>()
            {
                [ReceiptKey] = receipt.ToString(),
                [ContentTypeKey] = options.ContentType,
                [SchemaIdKey] = type.GUID.ToString(),
                [SchemaNameKey] = type.FullName!.DefaultIfNullOrWhiteSpace(type.ToString())
            };

            if (options.IntentId != Guid.Empty)
            {
                fields[IntentIdKey] = options.IntentId.ToString();
                if (false == options.IntentName.IsNullOrWhiteSpace())
                {
                    fields[IntentNameKey] = options.IntentName;
                }
            }

            try
            {
                await LocalSendAsync();
                return receipt;
            }
            catch (Exception) when(receipt.DataTransferMethod == DataTransferMethod.ByValue)
            {
                receipt = await StoreAsync(bytes, options.MessageTimeToLive, cancellation);
                fields[ReceiptKey] = receipt.ToString();
                await LocalSendAsync();
                return receipt;
            }

            Task LocalSendAsync()
            {
                var body = JsonSerializer
                    .Serialize(fields)
                    .ToUtf8Bytes();
                return SendAsync(body, options.MessageVisibilityTimeout, options.MessageTimeToLive);
            }
        }

        
    }
}
