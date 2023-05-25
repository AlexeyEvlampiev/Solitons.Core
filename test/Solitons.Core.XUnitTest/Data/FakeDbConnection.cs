using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using Moq;
using IsolationLevel = System.Data.IsolationLevel;

namespace Solitons.Data;

/// <summary>
/// Represents a mock database connection class for testing purposes.
/// </summary>
public sealed class FakeDbConnection : DbConnection
{
    /// <summary>
    /// Interface defining the behaviour of a database connection.
    /// </summary>
    public interface ICallback
    {
        DbTransaction BeginDbTransaction(IsolationLevel isolationLevel);

        void ChangeDatabase(string databaseName);
        void Close();
        void Open();
        string ConnectionString { get; set; }
        string Database { get; }
        ConnectionState State { get; }
        string DataSource { get; }
        string ServerVersion { get; }
        DbCommand CreateDbCommand();
        void Dispose(bool disposing);
        ValueTask DisposeAsync();
    }

    /// <summary>
    /// Mock class for <see cref="ICallback"/> interface.
    /// </summary>
    public sealed class Callback : Mock<ICallback>
    {
        private ConnectionState _state = ConnectionState.Closed;

        /// <summary>
        /// Initializes a new instance of the Callback class with a closed state.
        /// </summary>
        public Callback()
        {
            SetupGet(_ => _.State).Returns(_state);
            Setup(_ => _.Open()).Callback(() => _state = ConnectionState.Open);
        }
    }

    /// <summary>
    /// Gets the mocked <see cref="Callback"/> object.
    /// </summary>
    public Callback Mock { get; } = new Callback();

    /// <inheritdoc />
    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        return Mock.Object.BeginDbTransaction(isolationLevel);
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    public override void ChangeDatabase(string databaseName)
    {
        Mock.Object.ChangeDatabase(databaseName);
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    public override void Close()
    {
        Mock.Object.Close();
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    public override void Open()
    {
        Mock.Object.Open();
    }

    /// <inheritdoc />
    public override string ConnectionString
    {
        get => Mock.Object.ConnectionString;
        set => Mock.Object.ConnectionString = value;
    }

    /// <inheritdoc />
    public override string Database => Mock.Object.Database;

    /// <inheritdoc />
    public override ConnectionState State => Mock.Object.State;

    /// <inheritdoc />
    public override string DataSource => Mock.Object.DataSource;

    /// <inheritdoc />
    public override string ServerVersion => Mock.Object.ServerVersion;

    /// <inheritdoc />
    [DebuggerStepThrough]
    protected override DbCommand CreateDbCommand() => Mock.Object.CreateDbCommand();

    /// <inheritdoc />
    [DebuggerStepThrough]
    protected override void Dispose(bool disposing) => Mock.Object.Dispose(disposing);

    /// <inheritdoc />
    [DebuggerStepThrough]
    public override ValueTask DisposeAsync() => Mock.Object.DisposeAsync();

}