using Moq;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// Represents a mock database transaction class for testing purposes.
/// </summary>
public sealed class FakeDbTransaction : DbTransaction
{
    /// <summary>
    /// Interface defining the behaviour of a database transaction.
    /// </summary>
    public interface ICallback
    {
        void Commit();
        void Rollback();
        DbConnection DbConnection { get; }
        IsolationLevel IsolationLevel { get; }
        void Dispose(bool disposing);
        ValueTask DisposeAsync();
    }

    /// <summary>
    /// Mock class for ICallback interface.
    /// </summary>
    public sealed class Callback : Mock<ICallback>
    {
        /// <summary>
        /// Initializes a new instance of the Callback class with a fake database connection.
        /// </summary>
        public Callback()
        {
            var connection = new FakeDbConnection();
            this.SetupGet(_ => _.DbConnection).Returns(connection);
        }
    }

    /// <summary>
    /// Initializes a new instance of the FakeDbTransaction class with a fake database connection.
    /// </summary>
    public FakeDbTransaction()
    {
        var connection = new FakeDbConnection();
        Mock.SetupGet(_ => _.DbConnection).Returns(connection);
    }
    /// <summary>
    /// Initializes a new instance of the FakeDbTransaction class with a specific database connection.
    /// </summary>
    /// <param name="connection">The database connection to use for the transaction.</param>
    public FakeDbTransaction(FakeDbConnection connection)
    {
        Mock.SetupGet(_ => _.DbConnection).Returns(connection);
    }

    /// <summary>
    /// Gets the mocked <see cref="Callback"/> object.
    /// </summary>
    public Callback Mock { get; } = new Callback();

    /// <inheritdoc />
    public override void Commit()
    {
        Debug.Write($"Commiting via {nameof(Commit)}");
        Mock.Object.Commit();
    }

    public override Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        Debug.Write($"Commiting via {nameof(CommitAsync)}");
        Mock.Object.Commit();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override void Rollback() => Mock.Object.Rollback();

    /// <inheritdoc />
    protected override DbConnection DbConnection => Mock.Object.DbConnection;

    /// <inheritdoc />
    public override IsolationLevel IsolationLevel => Mock.Object.IsolationLevel;

    /// <inheritdoc />
    protected override void Dispose(bool disposing) => Mock.Object.Dispose(disposing);

    /// <inheritdoc />
    public override ValueTask DisposeAsync() => Mock.Object.DisposeAsync();
}
