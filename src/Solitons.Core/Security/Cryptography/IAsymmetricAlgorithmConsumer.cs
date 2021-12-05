using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security.Cryptography
{
    public interface IAsymmetricAlgorithmConsumer
    {
        /// <summary>
        /// Verifies the signature of a hash value.
        /// </summary>
        Task<bool> VerifyDataAsync(byte[] buffer, byte[] signature, CancellationToken cancellation = default);

        /// <summary>
        /// Verifies the signature of a hash value.
        /// </summary>
        public Task<bool> VerifyHashAsync(byte[] rgbHash, byte[] rgbSignature, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<byte[]> DecryptAsync(byte[] data, CancellationToken cancellation = default);
    }
}
