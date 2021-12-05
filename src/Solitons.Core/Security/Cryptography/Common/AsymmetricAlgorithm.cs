using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security.Cryptography.Common
{
    public abstract class AsymmetricAlgorithm : IAsymmetricAlgorithm
    {
        protected abstract Task<byte[]> SignDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellation);
        protected abstract Task<byte[]> SignDataAsync(byte[] buffer, CancellationToken cancellation);

        [DebuggerStepThrough]
        Task<byte[]> IAsymmetricAlgorithmProducer.SignDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellation)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            offset.ThrowIfArgumentOutOfRange(0, buffer.Length - 1, nameof(offset));
            count.ThrowIfArgumentOutOfRange(0, buffer.Length - offset, nameof(count));
            cancellation.ThrowIfCancellationRequested();
            return SignDataAsync(buffer, offset, count, cancellation);
        }

        [DebuggerStepThrough]
        Task<byte[]> IAsymmetricAlgorithmProducer.SignDataAsync(byte[] buffer, CancellationToken cancellation)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            cancellation.ThrowIfCancellationRequested();
            return SignDataAsync(buffer, cancellation);
        }

        Task<byte[]> IAsymmetricAlgorithmProducer.SignDataAsync(Stream inputStream, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        Task<byte[]> IAsymmetricAlgorithmProducer.SignHashAsync(byte[] rgbHash, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        Task<byte[]> IAsymmetricAlgorithmProducer.EncryptAsync(byte[] data, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        Task<bool> IAsymmetricAlgorithmConsumer.VerifyDataAsync(byte[] buffer, byte[] signature, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        Task<bool> IAsymmetricAlgorithmConsumer.VerifyHashAsync(byte[] rgbHash, byte[] rgbSignature, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        Task<byte[]> IAsymmetricAlgorithmConsumer.DecryptAsync(byte[] data, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}
