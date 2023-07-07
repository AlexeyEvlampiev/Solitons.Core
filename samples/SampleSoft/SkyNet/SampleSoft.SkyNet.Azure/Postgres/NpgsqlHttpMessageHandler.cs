using System.Data.Common;
using System.Diagnostics;
using Npgsql;
using Solitons.Data;

namespace SampleSoft.SkyNet.Azure.Postgres;

public abstract class NpgsqlHttpMessageHandler : DbHttpMessageHandler
{
    protected NpgsqlHttpMessageHandler(NpgsqlTransaction transaction) : base(transaction)
    {
    }

    protected NpgsqlHttpMessageHandler(string connectionString) : base(connectionString)
    {
    }

    protected abstract Task PgExecAsync(
        NpgsqlConnection connection, 
        HttpRequestMessage request, 
        HttpResponseMessage response, 
        CancellationToken cancellation);

    [DebuggerNonUserCode]
    protected override DbConnection CreateConnection(string connectionString) => new NpgsqlConnection(connectionString);

    [DebuggerStepThrough]
    protected sealed override Task ExecuteAsync(
        DbConnection connection, 
        HttpRequestMessage request, 
        HttpResponseMessage response,
        CancellationToken cancellation) =>
        PgExecAsync(
            (NpgsqlConnection)connection, 
            request, 
            response, 
            cancellation);

    [DebuggerStepThrough]
    protected sealed override ValueTask<DbTransaction> BeginTransactionAsync(
        HttpRequestMessage request, 
        DbConnection connection, 
        CancellationToken cancellation) =>
        BeginPgTransactionAsync(request, (NpgsqlConnection)connection, cancellation);

    protected virtual async ValueTask<DbTransaction> BeginPgTransactionAsync(
        HttpRequestMessage request,
        NpgsqlConnection connection,
        CancellationToken cancellation)
    {
        return await connection.BeginTransactionAsync(cancellation);
    }
}