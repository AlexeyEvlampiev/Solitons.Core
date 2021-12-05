

using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security.Cryptography
{
    sealed class RSAAsymmetricAlgorithm : Solitons.Security.Cryptography.Common.AsymmetricAlgorithm
    {
        private readonly RSA _rsa;
        private readonly RSACryptoServiceProvider _provider = new();
        private readonly RSAParameters _key;

        private RSAAsymmetricAlgorithm(RSA rsa)
        {
            _rsa = rsa ?? throw new ArgumentNullException(nameof(rsa));
            _key = _provider.ExportParameters(true);
            _provider.ImportParameters(_key);
        }

        public static IAsymmetricAlgorithm Create(Action<RSA> config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            var rsa = RSA.Create();
            config.Invoke(rsa);
            return new RSAAsymmetricAlgorithm(rsa);
        }

        public static async Task<IAsymmetricAlgorithm> CreateAsync(Func<RSA, Task> config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            var rsa = RSA.Create();
            await config.Invoke(rsa);
            return new RSAAsymmetricAlgorithm(rsa);
        }


        protected override Task<byte[]> SignDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        protected override Task<byte[]> SignDataAsync(byte[] buffer, CancellationToken cancellation)
        {
            var signature = _provider.SignData(buffer, SHA256.Create());
            return Task.FromResult(signature);
        }

    }
}
