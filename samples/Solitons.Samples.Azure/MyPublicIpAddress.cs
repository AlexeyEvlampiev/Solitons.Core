

using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Polly;

namespace Solitons.Samples.Azure
{
    public static class MyPublicIpAddress
    {
        private static IPAddress? _value;

        public static Task<IPAddress> GetAsync()
        {
            if (_value is not null) return Task.FromResult(_value);
            return Policy
                .Handle<HttpRequestException>(ex => ex.StatusCode.HasValue && (int)ex.StatusCode >= 400)
                .WaitAndRetryAsync(10, count=> TimeSpan.FromMilliseconds(count*100))
                .ExecuteAsync(async () =>
                {
                    using var client = new HttpClient();
                    var input = await client.GetStringAsync("http://checkip.dyndns.org/");
                    var match = Regex.Match(input, @"(?i)\baddress:\s*([^<>\s]+)");
                    Debug.Assert(match.Success);
                    var addressText = match.Groups[1].Value;
                    var address = IPAddress.Parse(addressText);
                    Interlocked.Exchange(ref _value, address);
                    return address;
                });
        }
    }
}
