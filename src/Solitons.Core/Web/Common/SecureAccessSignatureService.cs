using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Solitons.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SecureAccessSignatureService : ISecureAccessSignatureService
    {
        private readonly IClock _clock;
        private readonly Regex _queryStringRegex = new Regex("[?]?(?<query>.+?)&sig=(?<sign>[^&]+)");
        private readonly Regex _queryParameterRegex = new Regex(@"[?&]?(?<key>\w+)=(?<value>[^&]+)");


        [DebuggerNonUserCode]
        public SecureAccessSignatureService() : this(IClock.System)
        {

        }

        [DebuggerNonUserCode]
        protected SecureAccessSignatureService(IClock clock)
        {
            _clock = clock.ThrowIfNullArgument(nameof(clock));
        }



        protected abstract bool IsGenuineQueryString(string signedQueryString, string signature);
        protected abstract string SignQueryString(string queryString);

        protected abstract void Dispose();


        [DebuggerStepThrough]
        bool ISecureAccessSignatureService.IsGenuineQueryString(
            string queryString, 
            out DateTime? expiryTime, 
            out DateTime? startTime, 
            out IPAddress startAddress, 
            out IPAddress endAddress)
        {
            queryString.ThrowIfNullOrWhiteSpaceArgument(nameof(queryString));
            var match = _queryStringRegex.Match(queryString);
            if (false == match.Success)
                throw new FormatException();
            expiryTime = null;
            startTime = null;
            startAddress = null;
            endAddress = null;
            var query = match.Groups["query"].Value;
            var sign = match.Groups["sign"].Value;
            if (IsGenuineQueryString(query, sign))
            {                
                var data = query.ToBytes(Encoding.ASCII);
                var comparer = StringComparer.OrdinalIgnoreCase;
                for (Match parameterMatch = _queryParameterRegex.Match(query); parameterMatch.Success; parameterMatch = parameterMatch.NextMatch())
                {
                    var name = parameterMatch.Groups["key"].Value;
                    var value = parameterMatch.Groups["value"].Value;
                    if (comparer.Equals("st", name))
                    {
                        startTime = value.Convert(DateTime.Parse);
                    }
                    else if (comparer.Equals("se", name))
                    {
                        expiryTime = value.Convert(DateTime.Parse);
                    }
                    else if (comparer.Equals("sip", name))
                    {
                        var addresses = value.Split('-');
                        if (addresses.Length != 2)
                            throw new FormatException();
                        startAddress = IPAddress.Parse(addresses[0]);
                        endAddress = IPAddress.Parse(addresses[1]);
                    }
                }

                return true;

            }            

            return false;
        }



        [DebuggerStepThrough]
        string ISecureAccessSignatureService.SignQueryString(
            string queryString, 
            DateTime expiryTime, 
            DateTime? startTime, 
            IPAddress startAddress, 
            IPAddress endAddress)
        {
            queryString.ThrowIfNullOrWhiteSpaceArgument(nameof(queryString));
            expiryTime.ThrowIfLessThan(_clock.UtcNow.DateTime, () => new ArgumentOutOfRangeException(nameof(expiryTime)));
            startAddress ??= endAddress;
            endAddress ??= startAddress;
            var builder = new StringBuilder(queryString);
            if (startTime.HasValue)
                builder.Append($"&st={startTime:O}");
            builder.Append($"&se={expiryTime:O}");
            if (startAddress is not null)
                builder.Append($"&sip={startAddress}-{endAddress}");
            return SignQueryString(builder.ToString());
        }


        [DebuggerStepThrough]
        void IDisposable.Dispose() => Dispose();
    }
}
