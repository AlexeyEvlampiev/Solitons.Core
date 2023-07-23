using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
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
        ValueTask<DbTransaction> BeginTransactionAsync(HttpRequestMessage request, DbConnection connection, CancellationToken cancellation);
        DbConnection CreateConnection(string connectionString);
        void ExecuteAsync(DbConnection connection, HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellation);
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


            Setup(_ => _.CreateConnection(It.IsAny<string>())).Returns(new FakeDbConnection());
            Setup(_ => _.BeginTransactionAsync(
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<DbConnection>(),
                It.IsAny<CancellationToken>()))
                .Returns(DefaultBeginTransactionAsync);
        }

        public ISetup<ICallback> EveryExecuteAsync { get; }

        ValueTask<DbTransaction> DefaultBeginTransactionAsync(
            HttpRequestMessage request, 
            DbConnection connection,
            CancellationToken cancellation) => 
            ValueTask.FromResult(connection.BeginTransaction());
        
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
    /// Initializes a new instance of the FakeDbHttpMessageHandler class with a specific database transaction.
    /// </summary>
    public FakeDbHttpMessageHandler(DbTransaction transaction) : base(transaction)
    {
    }

    /// <summary>
    /// Initializes a new instance of the FakeDbHttpMessageHandler class with a specific connection string.
    /// </summary>
    public FakeDbHttpMessageHandler(string connectionString) : base(connectionString)
    {
        var connection = new FakeDbConnection();
        var transaction = new FakeDbTransaction();

        transaction.Mock.SetupGet(_ => _.DbConnection).Returns(connection);
        connection.Mock.Setup(_ => _.BeginDbTransaction(It.IsAny<IsolationLevel>())).Returns(transaction);
        this.Mock
            .Setup(_ => _.CreateConnection(It.IsAny<string>()))
            .Returns(connection);
    }

    /// <summary>
    /// Gets the mocked <see cref="Callback"/> object.
    /// </summary>
    public Callback Mock { get; } = new Callback();

    /// <inheritdoc />
    [DebuggerStepThrough]
    protected override ValueTask<DbTransaction> BeginTransactionAsync(
        HttpRequestMessage request,
        DbConnection connection,
        CancellationToken cancellation) => Mock.Object.BeginTransactionAsync(request, connection, cancellation);

    /// <inheritdoc />
    protected override DbConnection CreateConnection(string connectionString) => Mock.Object.CreateConnection(connectionString);

    /// <inheritdoc />
    protected override async Task ExecuteAsync(
        DbConnection connection,
        HttpRequestMessage request,
        HttpResponseMessage response,
        CancellationToken cancellation) => Mock.Object.ExecuteAsync(connection, request, response, cancellation);
}