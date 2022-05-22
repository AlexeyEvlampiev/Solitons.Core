using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DataTransferPackage
    {
        private const string TypeIdKey = "sys.typeId";
        private const string ContentKey = "sys.content";
        private const string EncodingKey = "sys.encoding";
        private const string ContentTypeKey = "sys.contentType";
        private const string TransactionTypeIdKey = "sys.trnTypeId";
        private const string CorrelationIdKey = "sys.correlationId";
        private const string SessionIdKey = "sys.sessionId";
        private const string MessageIdKey = "sys.messageId";
        private const string ToKey = "sys.to";
        private const string FromKey = "sys.from";
        private const string SignatureKey = "sys.signature";
        private const string ReplyToKey = "sys.replyTo";
        private const string ReplyToSessionIdKey = "sys.replyToSessionId";
        private const string ExpiredOnKey = "sys.expiredOn";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <param name="encoding"></param>
        public DataTransferPackage(Guid typeId, string content, string contentType, Encoding encoding)
        {
            content = content.ThrowIfNullArgument(nameof(content));
            contentType = contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)).Trim();
            encoding = encoding.ThrowIfNullArgument(nameof(encoding));

            TypeId = typeId.ThrowIfEmptyArgument(nameof(typeId));
            TransactionTypeId = TypeId;
            Content = content.ToBytes(encoding);
            ContentType = contentType;
            Encoding = encoding;

        }


        private DataTransferPackage(Guid typeId, byte[] content, string contentType, Encoding encoding)
        {
            content = content.ThrowIfNullArgument(nameof(content));
            contentType = contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)).Trim();
            encoding = encoding.ThrowIfNullArgument(nameof(encoding));

            TypeId = typeId.ThrowIfEmptyArgument(nameof(typeId));
            TransactionTypeId = TypeId;
            Content = content;
            ContentType = contentType;
            Encoding = encoding;
        }

        public Guid TypeId { get; }

        public string ContentType { get; }
        public IReadOnlyList<byte> Content { get; }
        public Encoding Encoding { get; }
        public Guid TransactionTypeId { get; set; }
        
        public string? CorrelationId { get; set; }
        public string? SessionId { get; set; }
        public string? MessageId { get; set; }
        public string? To { get; set; }
        public string? From { get; set; }
        public byte[]? Signature { get; set; }
        public string? ReplyTo { get; set; }
        public string? ReplyToSessionId { get; set; }
        public TimeSpan? TimeToLive { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public override string ToString() => ToString(IClock.System);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string ToString(IClock clock)
        {
            var data = new Dictionary<string, string>
            {
                [TypeIdKey] = TypeId.ToString("N"),
                [ContentKey] = Content.ToArray().ToBase64String(),
                [EncodingKey] = Encoding.BodyName,
                [ContentTypeKey] = ContentType,
                [TransactionTypeIdKey] = TransactionTypeId.ToString("N")
            };

            if(false == CorrelationId.IsNullOrWhiteSpace()) 
                data[CorrelationIdKey] = CorrelationId!;

            if(false == SessionId.IsNullOrWhiteSpace())
                data[SessionIdKey] = SessionId!;

            if (false == MessageId.IsNullOrWhiteSpace())
                data[MessageIdKey] = MessageId!;

            if (false == To.IsNullOrWhiteSpace())
                data[ToKey] = To!;

            if (false == From.IsNullOrWhiteSpace())
                data[FromKey] = From!;

            if (Signature is not null)
                data[SignatureKey] = Signature.ToBase64String();

            if (false == ReplyTo.IsNullOrWhiteSpace())
                data[ReplyToKey] = ReplyTo!;

            if (false == ReplyToSessionId.IsNullOrWhiteSpace())
                data[ReplyToSessionIdKey] = ReplyToSessionId!;

            if(TimeToLive.HasValue)
                data[ExpiredOnKey] = clock.UtcNow.Add(TimeToLive.Value).ToString("O");
            return JsonSerializer.Serialize(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DataTransferPackage Parse(string package) => Parse(package, IClock.System);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <param name="clock"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        internal static DataTransferPackage Parse(string package, IClock clock)
        {
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(package
                .ThrowIfNullOrWhiteSpaceArgument(nameof(package)));
            if (data is null) throw new FormatException($"Invalid json");

            if (false == data.TryGetValue(TypeIdKey, out var value) ||
                false == Guid.TryParse(value, out var typeId))
            {
                throw new FormatException($"Type ID is missing");
            }

            if (false == data.TryGetValue(ContentKey, out var contentBase64))
            {
                throw new FormatException($"Content is missing");
            }

            if (false == data.TryGetValue(ContentTypeKey, out var contentType))
            {
                throw new FormatException($"Content Type is missing");
            }

            if (false == data.TryGetValue(EncodingKey, out var encodingName))
            {
                throw new FormatException($"Encoding is missing");
            }

            if (false == data.TryGetValue(TransactionTypeIdKey, out value) ||
                false == Guid.TryParse(value, out var transactionTypeId))
            {
                throw new FormatException($"Transaction type ID is missing");
            }

            var encoding = TryCatch
                .Invoke(()=> Encoding.GetEncoding(encodingName), ex=> throw new FormatException("Unknown encoding type", ex));
            var content = Convert.FromBase64String(contentBase64);
            var result = new DataTransferPackage(typeId, content, contentType,  encoding);

            if (transactionTypeId != Guid.Empty)
                result.TransactionTypeId = transactionTypeId;

            if (data.TryGetValue(CorrelationIdKey, out value))
                result.CorrelationId = value;

            if (data.TryGetValue(SessionIdKey, out value))
                result.SessionId = value;

            if (data.TryGetValue(MessageIdKey, out value))
                result.MessageId = value;

            if (data.TryGetValue(ToKey, out value))
                result.To = value;

            if (data.TryGetValue(FromKey, out value))
                result.From = value;

            if (data.TryGetValue(SignatureKey, out value))
                result.Signature = Convert.FromBase64String(value);

            if (data.TryGetValue(ReplyToKey, out value))
                result.ReplyTo = value;

            if (data.TryGetValue(ReplyToSessionIdKey, out value))
                result.ReplyToSessionId = value;

            if (data.TryGetValue(ExpiredOnKey, out value) && 
                DateTime.TryParse(value, out var expiredOn))
            {
                var ttl = expiredOn - clock.UtcNow;
                result.TimeToLive = ttl > TimeSpan.Zero ? ttl : TimeSpan.Zero;
            }


            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static implicit operator string(DataTransferPackage package) => package.ToString();
    }

}
