using System.Data;
using System.Security.Claims;
using Npgsql;
using NpgsqlTypes;
using Polly;
using Polly.Retry;
using Solitons.Data;
using Solitons.Data.Common;
using Solitons.Samples.Domain;

namespace Solitons.Samples.Azure;

public class PgDatabaseRpcProvider : DatabaseRpcProvider
{
    private readonly string _connectionString;
    private readonly SampleDataContractSerializer _serializer = new();
    private readonly ClaimsPrincipal _caller;
    private static readonly AsyncRetryPolicy RetryPolicy = Policy
        .Handle<NpgsqlException>(ex => ex.IsTransient)
        .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200));


    public PgDatabaseRpcProvider(ClaimsPrincipal caller, string connectionString)
    {
        _caller = caller.ThrowIfNullArgument(nameof(caller));
        _connectionString = connectionString.ThrowIfNullOrWhiteSpaceArgument(nameof(connectionString));
    }




    protected override Task<string> InvokeAsync(DbCommandAttribute annotation, string payload, CancellationToken cancellation)
    {
        return RetryPolicy.ExecuteAsync(InvokeDbCommandAsync);

        async Task<string> InvokeDbCommandAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await using var command = new NpgsqlCommand($"SELECT api.{annotation.Procedure}(@request);", connection);
            command.CommandTimeout = (int)annotation.CommandTimeout.TotalSeconds;

            var requestType = annotation.RequestContentType switch
            {
                "application/json" => NpgsqlDbType.Jsonb,
                "application/xml" => NpgsqlDbType.Xml,
                _ => throw new NotImplementedException()
            };
            command.Parameters.AddWithValue("request", requestType, payload);
            await connection.OpenAsync(cancellation);
            await using var transaction = await connection.BeginTransactionAsync(annotation.IsolationLevel, cancellation);
            var response = await command.ExecuteScalarAsync(cancellation) ?? throw new NullReferenceException();
            await transaction.CommitAsync(CancellationToken.None);
            await connection.CloseAsync();
            return response.ToString()!;
        }
    }


    protected override object Deserialize(string content, string contentType, Type type)
    {
        return _serializer.Deserialize(type, contentType, content);
    }


    protected override string Serialize(object request, string contentType)
    {
        return _serializer.Serialize(request, contentType);
    }
}