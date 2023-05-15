using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Solitons.Net.Http;

// ReSharper disable once InconsistentNaming
public class HttpRetryPolicyHandler_SendAsync_Should
{
    [Fact]
    public async Task WorkAsync()
    {
        var fakeHandler = new FakeHttpHandler(new HttpStatusCode[]
        {
            HttpStatusCode.GatewayTimeout,
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.OK
        });


        var target = new HttpRetryPolicyHandler(fakeHandler);
        var client = new HttpClient(target);
        var response = await client.GetAsync("https://test.tst/test");

    }



    sealed class FakeHttpHandler : HttpMessageHandler
    {
        private int _counter = 0;
        private readonly HttpStatusCode[] _sequence;

        public FakeHttpHandler(HttpStatusCode[] sequence)
        {
            _sequence = sequence;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var statusCode = _sequence[_counter++];
            var response = new HttpResponseMessage(statusCode);
            return Task.FromResult(response);
        }
    }
}