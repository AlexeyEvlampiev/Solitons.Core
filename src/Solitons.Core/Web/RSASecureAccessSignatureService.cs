using Solitons.Web.Common;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RSASecureAccessSignatureService : SecureAccessSignatureService
    {
        private readonly RSACryptoServiceProvider  _rsa;
        private readonly HashAlgorithm _hashAlgorithm = MD5.Create();
  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rsa"></param>
        public RSASecureAccessSignatureService(RSACryptoServiceProvider rsa)
        {
            _rsa = rsa.ThrowIfNullArgument(nameof(rsa));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public RSASecureAccessSignatureService(Action<RSACryptoServiceProvider> config)
        {
            config.ThrowIfNullArgument(nameof(config));
            _rsa = new RSACryptoServiceProvider();
            config.Invoke(_rsa);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        protected override string SignQueryString(string queryString)
        { 
            var data = Encoding.ASCII.GetBytes(queryString);
            try
            { 
                var sig = _rsa
                    .SignData(data, _hashAlgorithm)
                    .ToBase64String();                
                return $"{queryString}&sig={sig}";
            }
            catch (CryptographicException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }

        }

        protected override bool IsGenuineQueryString(string signedQueryString, string signature)
        {
            var data = Encoding.ASCII.GetBytes(signedQueryString);
            var sig = signature.AsBase64Bytes();
            return _rsa.VerifyData(data, _hashAlgorithm, sig);
        }

        protected override void Dispose()
        {
            _rsa.Dispose();
            _hashAlgorithm.Dispose();
        }

        
    }
}
