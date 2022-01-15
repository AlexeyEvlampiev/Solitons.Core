using System.Data;
using System.Security.Claims;
using Npgsql;
using NpgsqlTypes;
using Polly;
using Polly.Retry;
using Solitons.Data.Common;

namespace Solitons.Samples.Azure;

public class PgTransactionScriptProvider : TransactionScriptProvider
{
    private readonly string _connectionString;
    private readonly ClaimsPrincipal _caller;
    private static readonly AsyncRetryPolicy RetryPolicy = Policy
        .Handle<NpgsqlException>(ex => ex.IsTransient)
        .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200));


    public PgTransactionScriptProvider(ClaimsPrincipal caller, string connectionString)
    {
        _caller = caller.ThrowIfNullArgument(nameof(caller));
        _connectionString = connectionString.ThrowIfNullOrWhiteSpaceArgument(nameof(connectionString));
    }



    protected override Task<string> InvokeAsync(
        string procedure, 
        string content, 
        string contentType,
        int timeoutInSeconds, 
        IsolationLevel isolationLevel,
        Func<Task>? completionCallback,
        CancellationToken cancellation)
    {
        return RetryPolicy.ExecuteAsync(InvokeDbCommandAsync);

        async Task<string> InvokeDbCommandAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await using var command = new NpgsqlCommand($"SELECT api.{procedure}(@request);", connection);
            command.CommandTimeout = timeoutInSeconds;

            var requestType = requestMetadata.ContentType switch
            {
                "application/json" => NpgsqlDbType.Jsonb,
                "application/xml" => NpgsqlDbType.Xml,
                _ => throw new NotImplementedException()
            };
            command.Parameters.AddWithValue("request", requestType, request);
            await connection.OpenAsync(cancellation);
            await using var transaction = await connection.BeginTransactionAsync(isolationLevel, cancellation);
            var response = await command.ExecuteScalarAsync(cancellation) ?? throw new NullReferenceException();
            if (completionCallback != null)
                await completionCallback.Invoke();
            await transaction.CommitAsync(CancellationToken.None);
            await connection.CloseAsync();
            return response.ToString()!;
        }
    }

}