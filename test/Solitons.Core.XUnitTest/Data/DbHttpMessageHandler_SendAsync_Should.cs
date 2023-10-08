using System;
using System.Data.Common;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Solitons.Diagnostics;
using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class DbHttpMessageHandler_SendAsync_Should
{

    [Fact]
    public async Task RetryOnTransientErrorsWithDefaultPolicy()
    {
        int attemptNumber = 0;
        var maxAttemptNumber = DbHttpMessageHandler
            .DefaultMaxRetryAttemptNumber
            .Max(3);
        var target = new FakeDbHttpMessageHandler("Fake connection string");
        target.Mock.EveryExecuteAsync
            .Callback(delegate
            {
                if (attemptNumber < DbHttpMessageHandler.DefaultMaxRetryAttemptNumber)
                {
                    attemptNumber++;
                    throw new FakeDbException(isTransient: true);
                }
            });
        var client = new HttpClient(target);
        var response = await client.GetAsync("db://api/test");
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(maxAttemptNumber, attemptNumber);
    }

    [Fact]
    public async Task ReturnInternalServerCodeOnNonTransientException()
    {
        var target = new FakeDbHttpMessageHandler();
        target.Mock.EveryExecuteAsync
            .Throws(new FakeDbException(isTransient: false));

        var client = new HttpClient(target);
        var request = new HttpRequestMessage(HttpMethod.Get, "db://test");
        var response = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        target.Mock.Verify(_ => _.ExecuteAsync(
            It.IsAny<DbConnection>(),
            request,
            response,
            It.IsAny<CancellationToken>()), Times.Once);
    }




    [Fact]
    public async Task LogExceptionsWhenConfigured()
    {
        var target = new FakeDbHttpMessageHandler("Some connection");
        target.Mock.EveryExecuteAsync
            .Throws(new Exception("Test error"));

        var logger = new FakeLogger();
        var errorLoggedCount = 0;
        logger.Mock
            .Setup(_ => _.LogAsync(It.IsAny<LogEventArgs>()))
            .Callback((LogEventArgs args) =>
            {
                if (args.Level == LogLevel.Error &&
                    StringComparer.Ordinal.Equals("Test error", args.Message))
                {
                    errorLoggedCount++;
                };
            });

        var client = new HttpClient(target);
        var request = new HttpRequestMessage(HttpMethod.Get, "db://api/test");
        IAsyncLogger.Set(request.Options, logger);
        
        var response = await client.SendAsync(request);
        Assert.Equal(1, errorLoggedCount);
    }


    [Fact]
    public async Task ReturnGatewayTimeoutOnTimeoutException()
    {
        var target = new FakeDbHttpMessageHandler("Some connection");
        var client = new HttpClient(target);

        target.Mock.EveryExecuteAsync
            .Throws(new TimeoutException("Test timeout"));
        var response = await client.GetAsync("db://api/test");
        Assert.Equal(HttpStatusCode.GatewayTimeout, response.StatusCode);

        target.Mock.EveryExecuteAsync
            .Throws(new FakeDbException("Test DB timeout", new TimeoutException())); 
        response = await client.GetAsync("db://api/test");
        Assert.Equal(HttpStatusCode.GatewayTimeout, response.StatusCode);
    }
}