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
    public sealed class Extensions_Func_WithRetry_Should
    {
        [Theory]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 0, HttpStatusCode.ServiceUnavailable)]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 1, HttpStatusCode.InternalServerError)]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 2, HttpStatusCode.OK)]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 100, HttpStatusCode.OK)]
        public async Task ApplyRetryLogic(string responsesCsv, int maxRetryAttempts, HttpStatusCode expectedResponse)
        {
            var service = TestService.Create(responsesCsv);
            var actualCode = await service
                .Convert(svc => AsyncFunc.Wrap(svc.InvokeAsync))
                .WithRetryOnResult(responses => responses
                    .Where(code => code != HttpStatusCode.OK)
                    .Take(maxRetryAttempts))
                .Invoke();
            Assert.Equal(expectedResponse, actualCode);
            Assert.True(service.InvocationsCount <= maxRetryAttempts + 1);
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

            public int InvocationsCount { get; private set; }

            public Task<HttpStatusCode> InvokeAsync()
            {
                lock (_locker)
                {
                    InvocationsCount++;
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
