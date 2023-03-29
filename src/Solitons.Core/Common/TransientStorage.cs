using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Data;

namespace Solitons.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TransientStorage : ITransientStorage
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Guid _id;


        /// <summary>
        /// 
        /// </summary>
        protected TransientStorage()
        {
            var att = (GuidAttribute)GetType()
                .GetCustomAttribute(typeof(GuidAttribute))
                .ThrowIfNull();
            _id = Guid.Parse(att.Value);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="expiresAfter"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<TransientStorageReceipt> UploadAsync(Stream stream, TimeSpan expiresAfter, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receipt"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<Stream> DownloadAsync(TransientStorageReceipt receipt, CancellationToken cancellation = default);

        [DebuggerStepThrough]
        async Task<TransientStorageReceipt> ITransientStorage.UploadAsync(Stream stream, TimeSpan expiresAfter, CancellationToken cancellation)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (false == stream.CanRead) throw new ArgumentException($"{nameof(stream.CanRead)} is false.", nameof(stream));
            if (expiresAfter < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(expiresAfter));
            cancellation.ThrowIfCancellationRequested();
            var receipt = await UploadAsync(stream, expiresAfter, cancellation);
            if (receipt.DataTransferMethod != DataTransferMethod.ByReference)
            {
                throw new InvalidOperationException(new StringBuilder()
                    .Append($"{GetType()}.{nameof(UploadAsync)} returned invalid receipt.")
                    .Append($" {nameof(receipt.DataTransferMethod)} expected value is {nameof(DataTransferMethod.ByReference)}")
                    .Append($" Actual value is {receipt.DataTransferMethod}")
                    .ToString());
            }

            return receipt;
        }

        async Task<Stream> ITransientStorage.DownloadAsync(TransientStorageReceipt receipt, CancellationToken cancellation)
        {
            if (receipt == null) throw new ArgumentNullException(nameof(receipt));
            if (receipt.DataTransferMethod == DataTransferMethod.ByValue)
            {
                var bytes = receipt.Token.AsBase64Bytes();
                return new MemoryStream(bytes);
            }


            if (receipt.TransientStorageId != _id)
            {
                throw new ArgumentOutOfRangeException(new StringBuilder()
                        .Append($"Transient storage reciept cannot be processed.")
                        .Append($" Receipt storage id: {receipt.TransientStorageId} ({receipt.TransientStorageName}).")
                        .Append($" Actual storage id: {_id} ({GetType().FullName}).")
                        .ToString(),
                    nameof(receipt));
            }
                
            cancellation.ThrowIfCancellationRequested();
            var stream = await DownloadAsync(receipt, cancellation);
            return stream
                .ThrowIfNull($"{GetType()}.{nameof(DownloadAsync)} returned null.")
                .ThrowIfCanNotRead();
        }
    }
}
