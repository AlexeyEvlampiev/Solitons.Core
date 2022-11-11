﻿using System.Net;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using System.Linq;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public sealed class AsyncFunc_WithRetryOnError_Should
    {
        [Theory]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 0, false)]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 1, false)]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 2, true)]
        [InlineData(@"ServiceUnavailable, InternalServerError, OK", 100, true)]
        public async Task ApplyRetryLogic(string responsesCsv, int retriesCount, bool expectedToSucceed)
        {
            var service = TestService.Create(responsesCsv);

            var func = service
                .Convert(svc => AsyncFunc.Cast(svc.InvokeAsync))
                .WithRetryOnError(errors => errors
                    .Take(retriesCount));

            if (expectedToSucceed)
            {
                Assert.Equal(HttpStatusCode.OK, await func.Invoke());
            }
            else
            {
                await Assert.ThrowsAsync<TestException>(func.Invoke);
            }
        }

        sealed class TestException : Exception
        {
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

                    if ((int)result >= 400)
                        throw new TestException();
                    return Task.FromResult(result);
                }
            }
        }
    }
}
