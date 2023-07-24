using System;
using System.Data;
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
    public async Task HandleManagedTransactions()
    {
        var connection = new FakeDbConnection();
        connection.Mock.SetupGet(_ => _.State).Returns(ConnectionState.Open);
        connection.Mock.Setup(_ => _.Close())
            .Callback(() => Assert.Fail("Handler may not close externally managed connection."));
        connection.Mock.Setup(_ => _.Dispose(It.IsAny<bool>()))
            .Callback(() => Assert.Fail("Handler may not dispose externally managed connection."));
        connection.Mock.Setup(_ => _.DisposeAsync())
            .Callback(() => Assert.Fail("Handler may not dispose externally managed connection."));

        var transaction = new FakeDbTransaction();
        transaction.Mock.SetupGet(_ => _.DbConnection).Returns(connection);
        transaction.Mock.Setup(_ => _.Commit())
            .Callback(() => Assert.Fail("Handler may not commit externally managed transaction."));
        transaction.Mock.Setup(_ => _.Rollback())
            .Callback(() => Assert.Fail("Handler may not rollback externally managed transaction."));
        transaction.Mock.Setup(_ => _.Dispose(It.IsAny<bool>()))
            .Callback(() => Assert.Fail("Handler may not dispose externally managed transaction."));
        transaction.Mock.Setup(_ => _.DisposeAsync())
            .Callback(() => Assert.Fail("Handler may not dispose externally managed transaction."));

        var target = new FakeDbHttpMessageHandler(transaction);
        var client = new HttpClient(target);

        var request = new HttpRequestMessage(HttpMethod.Get, "db://test");
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        target.Mock.Verify(_ => _.ExecuteAsync(
            connection,
            request,
            response,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ManagedItsOwnTransactions()
    {
        var connection = new FakeDbConnection();
        var transaction = new FakeDbTransaction();

        transaction.Mock.SetupGet(_ => _.DbConnection).Returns(connection);
        connection.Mock.Setup(_ => _.BeginDbTransaction(It.IsAny<IsolationLevel>())).Returns(transaction);

        var target = new FakeDbHttpMessageHandler("Fake connection string");
        target.Mock.Setup(_ => _.CreateConnection("Fake connection string")).Returns(connection);



        var client = new HttpClient(target);
        var request = new HttpRequestMessage(HttpMethod.Get, "db://test");
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        target.Mock.Verify(_ => _.CreateConnection(It.IsAny<string>()), Times.Once);
        target.Mock.Verify(_ => _.BeginTransactionAsync(
            request,
            connection,
            It.IsAny<CancellationToken>()), Times.Once);

        connection.Mock.Verify(_ => _.Open(), Times.Once);
        connection.Mock.Verify(_ => _.Close(), Times.Once);
        connection.Mock.Verify(_ => _.DisposeAsync(), Times.Once);

        transaction.Mock.Verify(_ => _.Commit(), Times.Once);
        transaction.Mock.Verify(_ => _.Rollback(), Times.Never);
        transaction.Mock.Verify(_ => _.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task RetryOnTransientErrorsWithDefaultPolicy()
    {
        int attemptNumber = 0;
        var maxAttemptNumber = DbHttpRequestMessage
            .DefaultMaxRetryAttemptNumber
            .Max(3);
        var target = new FakeDbHttpMessageHandler("Fake connection string");
        target.Mock.EveryExecuteAsync
            .Callback(delegate
            {
                if (attemptNumber < DbHttpRequestMessage.DefaultMaxRetryAttemptNumber)
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
    public async Task RetryOnTransientErrorsWithCustomPolicy()
    {
        int attemptNumber = 0;
        int policyInvocationsCount = 0;
        var target = new FakeDbHttpMessageHandler("Fake connection string");
        target.Mock.EveryExecuteAsync
            .Callback(delegate
            {
                if (attemptNumber < 5)
                {
                    attemptNumber++;
                    throw new FakeDbException(isTransient: true);
                }
            });
        var client = new HttpClient(target);
        var request = new DbHttpRequestMessage(HttpMethod.Get, "db://api/test")
            .WithRetryPolicy((args, token) =>
            {
                policyInvocationsCount++;
                return Task.FromResult(args.AttemptNumber < 5);
            });
        var response = await client.SendAsync(request);
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(5, attemptNumber);
        Assert.Equal(5, policyInvocationsCount);
    }

    [Fact]
    public async Task NeverThrow()
    {
        var target = new FakeDbHttpMessageHandler("Some connection");
        target.Mock.EveryExecuteAsync
            .Throws(new Exception("Test error"));
        var client = new HttpClient(target);

        var response = await client.GetAsync("db://api/test");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        target.Mock.Setup(_ => _.BeginTransactionAsync(
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<DbConnection>(),
                It.IsAny<CancellationToken>()))
            .Throws(new Exception("Starting transaction test error"));
        response = await client.GetAsync("db://api/test");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);


        target.Mock.Setup(_ => _.CreateConnection(
                It.IsAny<string>()))
            .Throws(new Exception("Creating connection test error."));
        response = await client.GetAsync("db://api/test");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
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
        request.Options.SetAsyncLogger(logger);
        
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