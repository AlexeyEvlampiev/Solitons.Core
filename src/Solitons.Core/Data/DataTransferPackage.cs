using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Solitons.Data;

/// <summary>
/// Represents a data transfer object.
/// </summary>
public sealed class DataTransferPackage
{
    #region Metadata Keys
    /// <summary>
    /// The key for the intent ID metadata property.
    /// </summary>
    private const string IntentIdKey = "sys.intentId";

    /// <summary>
    /// The key for the type ID metadata property.
    /// </summary>
    private const string TypeIdKey = "sys.typeId";

    /// <summary>
    /// The key for the content metadata property.
    /// </summary>
    private const string ContentKey = "sys.content";

    /// <summary>
    /// The key for the encoding metadata property.
    /// </summary>
    private const string EncodingKey = "sys.encoding";

    /// <summary>
    /// The key for the content type metadata property.
    /// </summary>
    private const string ContentTypeKey = "sys.contentType";

    /// <summary>
    /// The key for the correlation ID metadata property.
    /// </summary>
    private const string CorrelationIdKey = "sys.correlationId";

    /// <summary>
    /// The key for the session ID metadata property.
    /// </summary>
    private const string SessionIdKey = "sys.sessionId";

    /// <summary>
    /// The key for the message ID metadata property.
    /// </summary>
    private const string MessageIdKey = "sys.messageId";

    /// <summary>
    /// The key for the send to address metadata property.
    /// </summary>
    private const string ToKey = "sys.to";

    /// <summary>
    /// The key for the sender address or identifier metadata property.
    /// </summary>
    private const string FromKey = "sys.from";

    /// <summary>
    /// The key for the content digital signature metadata property.
    /// </summary>
    private const string SignatureKey = "sys.signature";

    /// <summary>
    /// The key for the reply to address metadata property.
    /// </summary>
    private const string ReplyToKey = "sys.replyTo";

    /// <summary>
    /// The key for the session identifier to reply to metadata property.
    /// </summary>
    private const string ReplyToSessionIdKey = "sys.replyToSessionId";

    /// <summary>
    /// The key for the expiration time metadata property.
    /// </summary>
    private const string ExpiredOnKey = "sys.expiredOn";
    #endregion

    /// <summary>
    /// The base64-encoded content digital signature.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string? _signatureBase64;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataTransferPackage"/> class with the specified type ID, media content, and encoding.
    /// </summary>
    /// <param name="typeId">The GUID associated with the serialized type.</param>
    /// <param name="mediaContent">The media content.</param>
    /// <param name="encoding">The encoding of the media content.</param>
    /// <exception cref="ArgumentNullException">Thrown when the content or encoding parameter is null.</exception>
    [DebuggerStepThrough]
    public DataTransferPackage(Guid typeId, TextMediaContent mediaContent, Encoding encoding) 
        : this(typeId, mediaContent.Content, mediaContent.ContentType, encoding)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataTransferPackage"/> class with the specified type ID, content, content type, and encoding.
    /// </summary>
    /// <param name="typeId">The GUID associated with the serialized type.</param>
    /// <param name="content">The content.</param>
    /// <param name="contentType">The content type.</param>
    /// <param name="encoding">The encoding of the content.</param>
    /// <exception cref="ArgumentNullException">Thrown when the content, content type, or encoding parameter is null.</exception>
    public DataTransferPackage(Guid typeId, string content, string contentType, Encoding encoding)
    {
        content = ThrowIf.ArgumentNull(content, nameof(content));
        contentType = ThrowIf.ArgumentNullOrWhiteSpace(contentType, nameof(contentType)).Trim();
        encoding = ThrowIf.ArgumentNull(encoding, nameof(encoding));

        TypeId = ThrowIf.ArgumentNullOrEmpty(typeId);
        Content = content.ToBytes(encoding);
        ContentType = contentType;
        Encoding = encoding;
        Properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataTransferPackage"/> class with the specified type ID, content as byte array, content type, and encoding.
    /// </summary>
    /// <param name="typeId">The GUID associated with the serialized type.</param>
    /// <param name="content">The content as a byte array.</param>
    /// <param name="contentType">The content type.</param>
    /// <param name="encoding">The encoding of the content.</param>
    /// <exception cref="ArgumentNullException">Thrown when the content, content type, or encoding parameter is null.</exception>
    private DataTransferPackage(Guid typeId, byte[] content, string contentType, Encoding encoding)
    {
        content = ThrowIf.ArgumentNull(content, nameof(content));
        contentType = ThrowIf.ArgumentNullOrWhiteSpace(contentType, nameof(contentType)).Trim();
        encoding = ThrowIf.ArgumentNull(encoding, nameof(encoding));

        TypeId = ThrowIf.ArgumentNullOrEmpty(typeId);
        Content = content;
        ContentType = contentType;
        Encoding = encoding;
        Properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the GUID associated with the serialized type.
    /// </summary>
    public Guid TypeId { get; }

    /// <summary>
    /// Gets or sets the package intent GUID.
    /// </summary>
    public Guid? IntentId { get; set; }

    /// <summary>
    /// Gets the type of the package content.
    /// </summary>
    public string ContentType { get; }

    /// <summary>
    /// Gets the package content as a read-only list of bytes.
    /// </summary>
    public IReadOnlyList<byte> Content { get; }

    /// <summary>
    /// Gets the content encoding.
    /// </summary>
    public Encoding Encoding { get; }


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
    public string? SignatureBase64
    {
        [DebuggerNonUserCode]
        get => _signatureBase64;
        [DebuggerStepThrough]
        set
        {
            if (value == null)
            {
                _signatureBase64 = null;
            }

            _signatureBase64 = value.IsBase64String()
                ? value
                : throw new InvalidOperationException(
                    $"The assigned value is not a valid base64 string");
        }
    }

    /// <summary>
    /// Gets or sets the digital signature as a byte array.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public byte[]? Signature
    {
        [DebuggerNonUserCode]
        get => _signatureBase64?.AsBase64Bytes();
        [DebuggerNonUserCode]
        set => _signatureBase64 = value?.ToBase64String();
    }

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
    /// Returns a string representation of the <see cref="DataTransferPackage"/> instance.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    [DebuggerStepThrough]
    public override string ToString() => ToString(IClock.System);

    /// <summary>
    /// Returns a string representation of the <see cref="DataTransferPackage"/> instance using the specified clock for time-related properties.
    /// </summary>
    /// <param name="clock">The clock to use for time-related properties.</param>
    /// <returns>A string that represents the current object.</returns>
    internal string ToString(IClock clock)
    {
        var data = new Dictionary<string, string>
        {
            [TypeIdKey] = TypeId.ToString("N"),
            [ContentKey] = Content.ToArray().ToBase64String(),
            [EncodingKey] = Encoding.BodyName,
            [ContentTypeKey] = ContentType
        };

        foreach (var pair in Properties)
        {
            if (pair.Key.IsNullOrWhiteSpace() || pair.Value.IsNullOrWhiteSpace())
                continue;
            data[pair.Key.Trim()] = pair.Value;
        }

        if (IntentId.HasValue)
        {
            data[IntentIdKey] = IntentId.Value.ToString("N");
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

        if (SignatureBase64.IsPrintable())
            data[SignatureKey] = SignatureBase64!;

        if (false == ReplyTo.IsNullOrWhiteSpace())
            data[ReplyToKey] = ReplyTo!;

        if (false == ReplyToSessionId.IsNullOrWhiteSpace())
            data[ReplyToSessionIdKey] = ReplyToSessionId!;

        if(TimeToLive.HasValue)
            data[ExpiredOnKey] = clock.UtcNow.Add(TimeToLive.Value).ToString("O");
        return JsonSerializer.Serialize(data);
    }



    /// <summary>
    /// Parses the specified JSON string representation of a <see cref="DataTransferPackage"/> instance.
    /// </summary>
    /// <param name="package">The JSON string representation of the <see cref="DataTransferPackage"/> instance.</param>
    /// <returns>A <see cref="DataTransferPackage"/> instance that is represented by the specified JSON string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="package"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the specified JSON string is not a valid representation of a <see cref="DataTransferPackage"/> instance.</exception>
    [DebuggerStepThrough]
    public static DataTransferPackage Parse(string package) => Parse(package, IClock.System);


    /// <summary>
    /// Parses the specified JSON string representation of a <see cref="DataTransferPackage"/> instance using the specified clock for time-related properties.
    /// </summary>
    /// <param name="package">The JSON string representation of the <see cref="DataTransferPackage"/> instance.</param>
    /// <param name="clock">The clock to use for time-related properties.</param>
    /// <returns>A <see cref="DataTransferPackage"/> instance that is represented by the specified JSON string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="package"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the specified JSON string is not a valid representation of a <see cref="DataTransferPackage"/> instance.</exception>
    internal static DataTransferPackage Parse(string package, IClock clock)
    {
        var data = ThrowIf
            .ArgumentNullOrWhiteSpace(package, nameof(package))
            .Convert(text => JsonSerializer.Deserialize<Dictionary<string, string>>(text))
            .ThrowIfNull($"Invalid json");

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


        var encoding = encodingName
            .Convert(Encoding.GetEncoding, ex => throw new FormatException("Unknown encoding type", ex));
        var content = Convert.FromBase64String(contentBase64);
        var result = new DataTransferPackage(typeId, content, contentType,  encoding);

        if(data.TryGetValue(IntentIdKey, out value))
            result.IntentId = Guid.Parse(value);

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
            result.SignatureBase64 = value;

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
    /// Implicitly converts the specified <see cref="DataTransferPackage"/> instance to its string representation.
    /// </summary>
    /// <param name="package">The <see cref="DataTransferPackage"/> instance to convert.</param>
    /// <returns>A string that represents the specified <see cref="DataTransferPackage"/> instance.</returns>
    public static implicit operator string(DataTransferPackage package) => package.ToString();

    /// <summary>
    /// Implicitly converts the specified <see cref="DataTransferPackage"/> instance to a <see cref="TextMediaContent"/> instance.
    /// </summary>
    /// <param name="package">The <see cref="DataTransferPackage"/> instance to convert.</param>
    /// <returns>A <see cref="TextMediaContent"/> instance that contains the content and content type of the specified <see cref="DataTransferPackage"/> instance.</returns>
    public static implicit operator TextMediaContent(DataTransferPackage package)
    {
        var content = package.Encoding.GetString(package.Content.ToArray());
        return new TextMediaContent(content, package.ContentType);
    }

}