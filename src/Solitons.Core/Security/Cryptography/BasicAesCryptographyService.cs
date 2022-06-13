using System.Security.Cryptography;

namespace Solitons.Security.Cryptography
{
    sealed class BasicAesCryptographyService : AesCryptographyService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        private readonly int _keySize;
        private readonly int _blockSize;
        private readonly PaddingMode _paddingMode;

        public BasicAesCryptographyService(byte[] key, byte[] iv, int keySize, int blockSize, PaddingMode paddingMode)
        {
            _key = key;
            _iv = iv;
            _keySize = keySize;
            _blockSize = blockSize;
            _paddingMode = paddingMode;
        }
        protected override void Configure(Aes alg)
        {
            alg.KeySize = _keySize;
            alg.BlockSize = _blockSize;
            alg.Padding = _paddingMode;
            alg.Key = _key;
            alg.IV = _iv;
        }
    }
}
