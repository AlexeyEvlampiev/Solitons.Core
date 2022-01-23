

using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Polly;

namespace Solitons.Samples.Azure
{
    public static class MyPublicIpAddress
    {
        private static readonly Lazy<IPAddress> _lazyLookup = new Lazy<IPAddress>(Discover);

        public static IPAddress Value => _lazyLookup.Value;

        private static IPAddress Discover()
        {
            return Policy
                .Handle<HttpRequestException>(ex => ex.StatusCode.HasValue && (int)ex.StatusCode >= 400)
                .WaitAndRetryAsync(10, count=> TimeSpan.FromMilliseconds(count*100))
                .ExecuteAsync(async () =>
                {
                    using var client = new HttpClient();
                    var input = await client.GetStringAsync("http://checkip.dyndns.org/");
                    var match = Regex.Match(input, @"(?i)\baddress:\s*([^<>\s]+)");
                    Debug.Assert(match.Success);
                    var address = match.Groups[1].Value;
                    return IPAddress.Parse(address);
                })
                .GetAwaiter()
                .GetResult();
        }

    }
}
