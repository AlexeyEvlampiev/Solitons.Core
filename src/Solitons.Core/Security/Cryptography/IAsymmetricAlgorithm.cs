using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Solitons.Security.Cryptography
{
    public interface IAsymmetricAlgorithm : 
        IAsymmetricAlgorithmProducer, 
        IAsymmetricAlgorithmConsumer
    {
        public static IAsymmetricAlgorithm Create(Action<RSA> config) => RSAAsymmetricAlgorithm.Create(config);
        public static Task<IAsymmetricAlgorithm> CreateAsync(Func<RSA, Task> config) => RSAAsymmetricAlgorithm.CreateAsync(config);
    }
}
