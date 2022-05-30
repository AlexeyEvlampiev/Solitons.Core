using System.Security.Claims;
using Npgsql;
using NpgsqlTypes;
using Polly;
using Polly.Retry;
using Solitons.Data;
using Solitons.Data.Common;

namespace Solitons.Samples.Azure
{
    public sealed class PgProcedureProvider : DatabaseRpcProvider
    {
        private readonly ClaimsPrincipal _caller;
        private readonly string _connectionString;

        private static readonly AsyncRetryPolicy RetryPolicy = Policy
            .Handle<NpgsqlException>(ex => ex.IsTransient)
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200));

        public PgProcedureProvider(ClaimsPrincipal caller, string connectionString)
        {
            _caller = caller ?? throw new ArgumentNullException(nameof(caller));
            _connectionString = connectionString.ThrowIfNullOrWhiteSpaceArgument(nameof(connectionString));
        }
        protected override Task<string> InvokeAsync(DatabaseRpcCommand commandInfo, string request, CancellationToken cancellation)
        {
            return RetryPolicy.ExecuteAsync(async () =>
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await using var command = new NpgsqlCommand($"SELECT api.{commandInfo.Procedure}(@request);", connection);
                command.CommandTimeout = (int)commandInfo.OperationTimeout.TotalSeconds;
                var requestType = commandInfo.RequestInfo.ContentType switch
                {
                    "application/json" => NpgsqlDbType.Jsonb,
                    "application/xml" => NpgsqlDbType.Xml,
                    _ => throw new NotImplementedException()
                };
                command.Parameters.AddWithValue("request", requestType, request);
                await connection.OpenAsync(cancellation);
                await using var transaction = await connection.BeginTransactionAsync(commandInfo.IsolationLevel, cancellation);
                var dbResponse = await command.ExecuteScalarAsync(cancellation) ?? new NullReferenceException();
                var response = dbResponse.ToString() ?? throw new NullReferenceException();
                await transaction.CommitAsync(CancellationToken.None);
                return response;
            });
        }

        protected override Task InvokeAsync(DatabaseRpcCommand commandInfo, string request, Func<string, Task> callback, CancellationToken cancellation)
        {
            return RetryPolicy.ExecuteAsync(async () =>
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await using var command = new NpgsqlCommand($"SELECT api.{commandInfo.Procedure}(@request);", connection);
                command.CommandTimeout = (int)commandInfo.OperationTimeout.TotalSeconds;
                var requestType = commandInfo.RequestInfo.ContentType switch
                {
                    "application/json" => NpgsqlDbType.Jsonb,
                    "application/xml" => NpgsqlDbType.Xml,
                    _ => throw new NotImplementedException()
                };
                command.Parameters.AddWithValue("request", requestType, request);
                await connection.OpenAsync(cancellation);
                await using var transaction = await connection.BeginTransactionAsync(commandInfo.IsolationLevel, cancellation);
                var dbResponse = await command.ExecuteScalarAsync(cancellation) ?? new NullReferenceException();
                var response = dbResponse.ToString() ?? throw new NullReferenceException();
                await callback.Invoke(response);
                await transaction.CommitAsync(CancellationToken.None);
                return response;
            });
        }

        protected override Task SendAsync(DatabaseRpcCommand commandInfo, string request, CancellationToken cancellation)
        {
            return RetryPolicy.ExecuteAsync(async () =>
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await using var command = new NpgsqlCommand($"SELECT api.{commandInfo.Procedure}(@request);", connection);
                command.CommandTimeout = (int)commandInfo.OperationTimeout.TotalSeconds;
                var requestType = commandInfo.RequestInfo.ContentType switch
                {
                    "application/json" => NpgsqlDbType.Jsonb,
                    "application/xml" => NpgsqlDbType.Xml,
                    _ => throw new NotImplementedException()
                };
                command.Parameters.AddWithValue("request", requestType, request);
                await connection.OpenAsync(cancellation);
                await using var transaction = await connection.BeginTransactionAsync(commandInfo.IsolationLevel, cancellation);
                await command.ExecuteNonQueryAsync(cancellation);
                await transaction.CommitAsync(CancellationToken.None);
            });
        }

        protected override Task SendAsync(DatabaseRpcCommand commandInfo, string request, Func<Task> callback, CancellationToken cancellation)
        {
            return RetryPolicy.ExecuteAsync(async () =>
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await using var command = new NpgsqlCommand($"SELECT api.{commandInfo.Procedure}(@request);", connection);
                command.CommandTimeout = (int)commandInfo.OperationTimeout.TotalSeconds;
                var requestType = commandInfo.RequestInfo.ContentType switch
                {
                    "application/json" => NpgsqlDbType.Jsonb,
                    "application/xml" => NpgsqlDbType.Xml,
                    _ => throw new NotImplementedException()
                };
                command.Parameters.AddWithValue("request", requestType, request);
                await connection.OpenAsync(cancellation);
                await using var transaction = await connection.BeginTransactionAsync(commandInfo.IsolationLevel, cancellation);
                await command.ExecuteNonQueryAsync(cancellation);
                await callback.Invoke();
                await transaction.CommitAsync(CancellationToken.None);
            });
        }
    }
}
