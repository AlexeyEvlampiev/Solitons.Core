using System;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public sealed class AsyncFunc_WithRetry_Should
    {
        [Theory]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 0, HttpStatusCode.ServiceUnavailable)]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 1, HttpStatusCode.InternalServerError)]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 2, HttpStatusCode.OK)]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 100, HttpStatusCode.OK)]
        public async Task ApplyRetryLogic(string responsesCsv, int retriesCount, HttpStatusCode expectedResponse)
        {
            var service = TestService.Create(responsesCsv);
            var actualCode = await service
                .Convert(svc => AsyncFunc.Cast(svc.InvokeAsync))
                .WithRetry(response => response
                    .Where(code => code != HttpStatusCode.OK)
                    .Take(retriesCount))
                .Invoke();
            Assert.Equal(expectedResponse, actualCode);
        }



        class TestService
        {
            private readonly HttpStatusCode[] _responseSequence;
            private readonly object _locker = new();
            private int _index = 0;

            private TestService(string httpStatusCodes)
            {
                _responseSequence = httpStatusCodes
                    .Convert(text => Regex.Split(text, @"\W+"))!
                    .Skip(string.IsNullOrWhiteSpace)
                    .Select(Enum.Parse<HttpStatusCode>)
                    .ToArray();
            }

            public static TestService Create(string httpStatusCodes) => new(httpStatusCodes);


            public Task<HttpStatusCode> InvokeAsync()
            {
                lock (_locker)
                {
                    var result = _responseSequence[_index++];
                    if (_index > _responseSequence.Length - 1)
                    {
                        _index = 0;
                    }
                    //Debug.WriteLine(result);
                    return Task.FromResult(result);
                }
            }
        }
    }
}
