using System;
using System.Diagnostics;
using System.Text.Json;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{ContentType} {DataTransferMethod}")]
    public sealed record DomainTransientStorageReceipt
    {
        private Lazy<byte[]> _content;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataTransferMethod"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <param name="dtoTypeId"></param>
        public DomainTransientStorageReceipt(DataTransferMethod dataTransferMethod, string content, string contentType, Guid dtoTypeId)
        {
            DataTransferMethod = dataTransferMethod;
            Content = content;
            ContentType = contentType;
            DtoTypeId = dtoTypeId;
            _content = new Lazy<byte[]>(() => JsonSerializer.SerializeToUtf8Bytes(this));
        }

        public DataTransferMethod DataTransferMethod { get; }
        public string Content { get; }
        public string ContentType { get; }
        public Guid DtoTypeId { get; }

        public override string ToString() => JsonSerializer.Serialize(this);

        public byte[] ToArray() => _content.Value;

        internal static bool TryDeserialize(byte[] bytes, out DomainTransientStorageReceipt receipt)
        {
            receipt = null;
            if (bytes is null) return false;
            try
            {
                var json = bytes.ToBase64String();
                receipt = JsonSerializer
                    .Deserialize<DomainTransientStorageReceipt>(json)
                    .ThrowIfNull(()=> new FormatException());
                return receipt.Content.IsNullOrWhiteSpace() == false &&
                       receipt.ContentType.IsNullOrWhiteSpace() == false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }
}
