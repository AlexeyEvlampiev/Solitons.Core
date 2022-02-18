using System.Diagnostics;
using System.Security.Cryptography;

namespace Solitons.Security.Cryptography
{
    sealed class DefaultRsaSasProvider : RsaSasProvider
    {
        [DebuggerStepThrough]
        public DefaultRsaSasProvider(string delimiter) : base(delimiter)
        {

        }

        protected override object CreateHashAlg() => SHA256.Create();
    }
}
