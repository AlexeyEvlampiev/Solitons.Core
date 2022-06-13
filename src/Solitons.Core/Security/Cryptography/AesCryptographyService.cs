using System.IO;
using System.Security.Cryptography;

namespace Solitons.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AesCryptographyService
    {
        private const int DefaultKeySize = 128;
        private const int DefaultBlockSize = 128;
        private const PaddingMode DefaultPaddingMode = PaddingMode.PKCS7;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="keySize"></param>
        /// <param name="blockSize"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static AesCryptographyService Basic(
            out byte[] key,
            byte[] iv,
            int keySize = DefaultKeySize,
            int blockSize = DefaultBlockSize,
            PaddingMode padding = DefaultPaddingMode)
        {
            using var alg = Aes.Create();
            alg.KeySize = keySize;
            alg.BlockSize = blockSize;
            alg.Padding = padding;
            alg.GenerateKey();
            key = alg.Key;
            return new BasicAesCryptographyService(alg.Key, iv, keySize, blockSize, padding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="keySize"></param>
        /// <param name="blockSize"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static AesCryptographyService Basic(
            byte[] key, 
            byte[] iv, 
            int keySize = DefaultKeySize, 
            int blockSize = DefaultBlockSize,
            PaddingMode padding = DefaultPaddingMode)
        {
            return new BasicAesCryptographyService(key, iv, keySize, blockSize, padding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="blockSize"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static AesCryptographyService Basic(
            int keySize = DefaultKeySize,
            int blockSize = DefaultBlockSize,
            PaddingMode padding = DefaultPaddingMode)
        {
            using var alg = Aes.Create();
            alg.KeySize = keySize;
            alg.BlockSize = blockSize;
            alg.Padding = padding;
            alg.GenerateKey();
            alg.GenerateIV();
            return Basic(alg.Key,alg.IV, keySize, blockSize, padding);
        }




        public static byte[] GenerateKey(
            int keySize = DefaultKeySize,
            int blockSize = DefaultBlockSize,
            PaddingMode padding = DefaultPaddingMode)
        {
            using var alg = Aes.Create();
            alg.KeySize = keySize;
            alg.BlockSize = blockSize;
            alg.Padding = padding;
            alg.GenerateKey();
            return alg.Key;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="alg"></param>
        protected abstract void Configure(Aes alg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data)
        {
            using var aes = Aes.Create();
            Configure(aes);

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            return PerformCryptography(data, encryptor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] data)
        {
            using var aes = Aes.Create();
            Configure(aes);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            return PerformCryptography(data, decryptor);
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();

            return ms.ToArray();
        }
    }
}
