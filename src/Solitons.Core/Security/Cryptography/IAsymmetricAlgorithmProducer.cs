using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security.Cryptography
{
    public interface IAsymmetricAlgorithmProducer
    {
        /// <summary>
        /// Computes the hash value of a subset of the specified byte array using the
        /// specified hash algorithm, and signs the resulting hash value.
        /// </summary>
        /// <param name="buffer">The input data for which to compute the hash</param>
        /// <param name="offset">The offset into the array from which to begin using data</param>
        /// <param name="count">The number of bytes in the array to use as data. </param>
        /// <returns>The underlying algorithm signature for the specified data.</returns>
        /// <param name="cancellation"></param>
        Task<byte[]> SignDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellation = default);

        /// <summary>
        /// Computes the hash value of a subset of the specified byte array using the
        /// specified hash algorithm, and signs the resulting hash value.
        /// </summary>
        /// <param name="buffer">The input data for which to compute the hash</param>
        /// <param name="cancellation"></param>
        /// <returns>The RSA signature for the specified data.</returns>
        Task<byte[]> SignDataAsync(byte[] buffer, CancellationToken cancellation = default);

        /// <summary>
        /// Computes the hash value of a subset of the specified byte array using the
        /// specified hash algorithm, and signs the resulting hash value.
        /// </summary>
        /// <param name="inputStream">The input data for which to compute the hash</param>
        /// <param name="cancellation"></param>
        /// <returns>The RSA signature for the specified data.</returns>
        Task<byte[]> SignDataAsync(Stream inputStream, CancellationToken cancellation = default);

        /// <summary>
        /// Computes the hash value of a subset of the specified byte array using the
        /// specified hash algorithm, and signs the resulting hash value.
        /// </summary>
        /// <param name="rgbHash">The input data for which to compute the hash</param>
        /// <param name="cancellation"></param>
        /// <returns>The RSA signature for the specified data.</returns>
        Task<byte[]> SignHashAsync(byte[] rgbHash, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<byte[]> EncryptAsync(byte[] data, CancellationToken cancellation = default);
    }
}
