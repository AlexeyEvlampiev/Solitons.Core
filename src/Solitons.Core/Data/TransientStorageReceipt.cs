using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public sealed class TransientStorageReceipt
{
    private const string TokenKey = "token";
    private const string MethodKey = "method";
    private const string SourceIdKey = "source-id";
    private const string SourceNameKey = "source-name";
    private const string ExpiresOnKey = "expires-on";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="token"></param>
    /// <param name="expiresOn"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public TransientStorageReceipt(ITransientStorage source, string token, DateTimeOffset expiresOn)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        Token = ThrowIf.ArgumentNullOrWhiteSpace(token, nameof(token));
        ExpiresOnUtc = expiresOn.DateTime.ThrowIfArgumentLessOrEqual(DateTime.UtcNow, nameof(expiresOn));
        var sourceType = source.GetType();
        TransientStorageId = sourceType.GUID;
        TransientStorageName = sourceType.FullName ?? sourceType.GUID.ToString();
        DataTransferMethod = DataTransferMethod.ByReference;
        ExpiresOnUtc = expiresOn.DateTime;
    }

    private TransientStorageReceipt(byte[] bytes)
    {
        Token = bytes.ToBase64String();
        DataTransferMethod = DataTransferMethod.ByValue;
        TransientStorageId = Guid.Empty;
        TransientStorageName = "InMemory";
    }

    private TransientStorageReceipt(Dictionary<string, string> fields)
    {
        TransientStorageId = Guid.Parse(fields[SourceIdKey]);
        TransientStorageName = fields[SourceNameKey];
        Token = fields[TokenKey];
        DataTransferMethod = (DataTransferMethod)int.Parse(fields[MethodKey]);
    }

    public string TransientStorageName { get; }


    public Guid TransientStorageId { get; }

    public string Token { get; }

    public DataTransferMethod DataTransferMethod { get; }

    public DateTime ExpiresOnUtc { get; }


    public override string ToString()
    {
        var fields = new Dictionary<string, string>()
        {
            [TokenKey] = Token,
            [MethodKey] = ((int)DataTransferMethod).ToString(),
            [SourceIdKey] = TransientStorageId.ToString(),
            [SourceNameKey] = TransientStorageName
        };
        var json = JsonSerializer.Serialize(fields);
        return json.ToBase64(Encoding.UTF8);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="receipt"></param>
    /// <returns></returns>
    public static TransientStorageReceipt Parse(string receipt)
    {
        var json = ThrowIf
            .ArgumentNullOrWhiteSpace(receipt, nameof(receipt))
            .AsBase64Bytes()
            .ToUtf8String();
        var fields = JsonSerializer
            .Deserialize<Dictionary<string, string>>(json)
            .ThrowIfNull();
        return new TransientStorageReceipt(fields);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TransientStorageReceipt CreateInMemoryStorageReceipt(byte[] bytes)
    {
        if (bytes == null) throw new ArgumentNullException(nameof(bytes));
        return new TransientStorageReceipt(bytes);
    }
}