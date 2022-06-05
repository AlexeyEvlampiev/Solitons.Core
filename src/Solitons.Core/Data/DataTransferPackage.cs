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

        #region Metadata Keys
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
        #endregion


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
            Properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
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
            Properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the GUID associated with the serialized type
        /// </summary>
        public Guid TypeId { get; }

        /// <summary>
        /// Gets the type of the package content.
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        /// Gets the package content.
        /// </summary>
        public IReadOnlyList<byte> Content { get; }

        /// <summary>
        /// Gets the content encoding.
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// Gets or sets the identifier of the intended system transaction.
        /// </summary>
        public Guid TransactionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the correlation.
        /// </summary>
        public string? CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the session.
        /// </summary>
        public string? SessionId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the message.
        /// This is a user-defined value that message brokers can use to identify duplicate messages.
        /// </summary>
        public string? MessageId { get; set; }

        /// <summary>
        /// Gets or sets the send to address.
        /// </summary>
        public string? To { get; set; }

        /// <summary>
        /// Gets or sets the sender address or identifier
        /// </summary>
        public string? From { get; set; }

        /// <summary>
        /// Gets or sets the content digital signature
        /// </summary>
        public byte[]? Signature { get; set; }

        /// <summary>
        /// Gets or sets the address to reply to.
        /// </summary>
        public string? ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the session identifier to reply to.
        /// </summary>
        public string? ReplyToSessionId { get; set; }

        /// <summary>
        /// Gets or sets the message’s time to live value. This is the duration after which the message expires, starting from when the message is sent or serialized.
        /// </summary>
        public TimeSpan? TimeToLive { get; set; }

        /// <summary>
        /// Gets the collection of custom properties
        /// </summary>
        public Dictionary<string, string> Properties { get; }


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

            foreach (var pair in Properties)
            {
                if (pair.Key.IsNullOrWhiteSpace() || pair.Value.IsNullOrWhiteSpace())
                    continue;
                data[pair.Key.Trim()] = pair.Value;
            }

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

            result.Properties.Clear();
            foreach (var pair in data.Skip(item=> item.Key.StartsWith("sys.")))
            {
                result.Properties.Add(pair.Key, pair.Value);
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
