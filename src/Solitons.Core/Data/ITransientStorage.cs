using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public partial interface ITransientStorage
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="expiresAfter"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task<TransientStorageReceipt> UploadAsync(Stream stream, TimeSpan expiresAfter, CancellationToken cancellation = default);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="receipt"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task<Stream> DownloadAsync(TransientStorageReceipt receipt, CancellationToken cancellation = default);

}

public partial interface ITransientStorage
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="expiresAfter"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public async Task<TransientStorageReceipt> UploadAsync(byte[] bytes, TimeSpan expiresAfter, CancellationToken cancellation = default)
    {
        ThrowIf.ArgumentNull(bytes, nameof(bytes));
        expiresAfter.ThrowIfArgumentLessThan(TimeSpan.Zero, nameof(expiresAfter));
        cancellation.ThrowIfCancellationRequested();
        await using var stream = new MemoryStream(bytes);
        return await UploadAsync(stream, expiresAfter, cancellation);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="receipt"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [DebuggerStepThrough]
    public async Task<byte[]> DownloadBytesAsync(TransientStorageReceipt receipt, CancellationToken cancellation = default)
    {
        if (receipt == null) throw new ArgumentNullException(nameof(receipt));
        cancellation.ThrowIfCancellationRequested();
        await using var stream = await DownloadAsync(receipt, cancellation);
        await using var memory = new MemoryStream();
        await stream.CopyToAsync(memory, cancellation);
        memory.Seek(0, SeekOrigin.Begin);
        return memory.ToArray();
    }
}