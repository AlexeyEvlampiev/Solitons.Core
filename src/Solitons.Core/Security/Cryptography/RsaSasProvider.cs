using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Solitons.Security.Cryptography
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RsaSasProvider
    {
        private const string DefaultDelimiter = "&sig=";
        private readonly string _delimiter;
        private readonly Regex _sasRegex;

        /// <summary>
        /// 
        /// </summary>
        [DebuggerStepThrough]
        protected RsaSasProvider() : this(DefaultDelimiter)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delimiter"></param>
        protected RsaSasProvider(string delimiter)
        {
            _delimiter = ThrowIf.ArgumentNullOrWhiteSpace(delimiter, nameof(delimiter));
            _sasRegex = new($@"^(?<data>.+){delimiter}(?<sig>\S+?)$",
                RegexOptions.Compiled |
                RegexOptions.RightToLeft);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static RsaSasProvider CreateDefault() => new DefaultRsaSasProvider(DefaultDelimiter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static RsaSasProvider CreateDefault(string delimiter) => 
            new DefaultRsaSasProvider(ThrowIf.ArgumentNullOrWhiteSpace(delimiter, nameof(delimiter)));

        protected abstract object CreateHashAlg();

        public ISasSigner AsSasSigner(RSAParameters rsaParameters) => new Signer(this, rsaParameters);

        public ISasValidator AsSasValidator(RSAParameters rsaParameters, ISasValidationCallback? callback = null) => new SasValidator(this, rsaParameters, callback);

        public string Sign(string queryString, RSAParameters rsaParameters)
        {
            if (queryString == null) throw new ArgumentNullException(nameof(queryString));
            var provider = new RSACryptoServiceProvider();
            provider.ImportParameters(rsaParameters);

            var sig = provider.SignData(queryString.ToUtf8Bytes(), CreateHashAlg()).ToBase64String();
            return $"{queryString}{_delimiter}{sig}";
        }

        public bool Validate(string sas, RSAParameters rsaParameters, out string? data, ISasValidationCallback? callback = null)
        {
            if (sas == null) throw new ArgumentNullException(nameof(sas));
            data = null;
            callback ??= new SasValidationCallback();
            var match = _sasRegex.Match(sas);
            if (false == match.Success)
            {
                callback.OnInvalidFormat("Expected format: [data]&sig=[sig]");
                return false;
            }
            Debug.Assert(match.Groups["data"].Success);
            Debug.Assert(match.Groups["sig"].Success);

            var dataSubstring = match.Groups["data"].Value;
            var sig = match.Groups["sig"].Value;
            byte[]? sigBytes = null;
            try
            {
                sigBytes = Convert.FromBase64String(sig);
            }
            catch (FormatException)
            {
                callback.OnInvalidFormat($"Invalid signature. The signature value must be a valid base64 string.");
                return false;
            }
            

            var provider = new RSACryptoServiceProvider();
            provider.ImportParameters(rsaParameters);
            if (false == provider.VerifyData(dataSubstring.ToUtf8Bytes(), CreateHashAlg(), sigBytes))
            {
                callback.OnInvalidSignature();
                return false;
            }

            data = dataSubstring;
            return true;
        }

        sealed class SasValidationCallback : ISasValidationCallback
        {
            public void OnInvalidFormat(string message) => Debug.WriteLine(message);

            public void OnInvalidSignature() {}
        }

       

        

        sealed class Signer : ISasSigner
        {
            private readonly RsaSasProvider _provider;
            private readonly RSAParameters _parameters;

            public Signer(RsaSasProvider provider, RSAParameters parameters)
            {
                _provider = provider;
                _parameters = parameters;
                Debug.Assert(provider is not null);
            }

            public string Sign(string data)
            {
                return _provider.Sign(data, _parameters);
            }
        }

        sealed class SasValidator : ISasValidator
        {
            private readonly RsaSasProvider _provider;
            private readonly RSAParameters _parameters;
            private readonly ISasValidationCallback? _callback;

            public SasValidator(RsaSasProvider provider, RSAParameters parameters, ISasValidationCallback? callback)
            {
                _provider = provider;
                _parameters = parameters;
                _callback = callback;
            }

            [DebuggerStepThrough]
            public bool IsValid(string sas, out string? data)
            {
                return _provider.Validate(sas, _parameters, out data, _callback);
            }
        }

    }
}
