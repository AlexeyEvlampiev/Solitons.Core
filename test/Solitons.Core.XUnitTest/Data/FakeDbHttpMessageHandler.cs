using System.Data;
using System.Data.Common;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;

namespace Solitons.Data;

/// <summary>
/// Represents a mock HTTP message handler with database transaction support for testing purposes.
/// </summary>
public sealed class FakeDbHttpMessageHandler : DbHttpMessageHandler
{
    /// <summary>
    /// Interface defining the behaviour of an HTTP database message handler.
    /// </summary>
    public interface ICallback
    {
        void ExecuteAsync(
            DbConnection connection, 
            HttpRequestMessage request, 
            HttpResponseMessage response,
            CancellationToken cancellation);
    }

    /// <summary>
    /// Mock class for ICallback interface.
    /// </summary>
    public sealed class Callback : Mock<ICallback>
    {

        public Callback()
        {
            EveryExecuteAsync = Setup(_ => _.ExecuteAsync(
                It.IsAny<DbConnection>(),
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<HttpResponseMessage>(),
                It.IsAny<CancellationToken>()));
            EveryExecuteAsync.Callback(DefaultExecuteHandler);
        }

        public ISetup<ICallback> EveryExecuteAsync { get; }

        
        void DefaultExecuteHandler(
            DbConnection connection,
            HttpRequestMessage request, 
            HttpResponseMessage response,
            CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
        }
    }

    /// <summary>
    /// Initializes a new instance of the FakeDbHttpMessageHandler class with a default connection string.
    /// </summary>
    public FakeDbHttpMessageHandler() : this("Fake connection string")
    {
    }



    /// <summary>
    /// Initializes a new instance of the FakeDbHttpMessageHandler class with a specific connection string.
    /// </summary>
    public FakeDbHttpMessageHandler(string connectionString) 
        : base(() =>
        {
            var connection = new FakeDbConnection();
            connection.Mock.SetupGet(_ => _.State).Returns(ConnectionState.Open);
            return connection;
        })
    {
        
    }

    /// <summary>
    /// Gets the mocked <see cref="Callback"/> object.
    /// </summary>
    public Callback Mock { get; } = new Callback();


    /// <inheritdoc />
    protected override Task ExecuteAsync(
        DbConnection connection,
        HttpRequestMessage request,
        HttpResponseMessage response,
        CancellationToken cancellation)
    {
        Mock.Object.ExecuteAsync(connection, request, response, cancellation);
        return Task.CompletedTask;
    }
}