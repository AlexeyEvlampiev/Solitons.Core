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


    protected override bool CanSerialize(Type type, string contentType) => _serializer.CanSerialize(type, contentType);


    protected override RpcCommand BuildRpcCommand(DbCommandAttribute annotation)
    {
        return new PgRpcCommand(annotation, _serializer, _connectionString);
    }

    sealed class PgRpcCommand : RpcCommand
    {
        private readonly string _connectionString;

        public PgRpcCommand(
            DbCommandAttribute annotation,
            IDataContractSerializer serializer,
            string connectionString) : base(annotation, serializer)
        {
            _connectionString = connectionString;
        }

        protected override Task<object> InvokeAsync(object request, CancellationToken cancellation)
        {
            return RetryPolicy.ExecuteAsync(async () =>
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await using var command = new NpgsqlCommand($"SELECT api.{Annotation.Procedure}(@request);", connection);
                command.CommandTimeout = (int)Annotation.CommandTimeout.TotalSeconds;
                var requestType = Annotation.RequestContentType switch
                {
                    "application/json" => NpgsqlDbType.Jsonb,
                    "application/xml" => NpgsqlDbType.Xml,
                    _ => throw new NotImplementedException()
                };
                var payload = SerializeRequest(request);
                command.Parameters.AddWithValue("request", requestType, payload);
                await connection.OpenAsync(cancellation);
                await using var transaction = await connection.BeginTransactionAsync(Annotation.IsolationLevel, cancellation);
                var dbResponse = await command.ExecuteScalarAsync(cancellation) ?? new NullReferenceException();
                payload = dbResponse.ToString() ?? throw new NullReferenceException();
                var response = DeserializeResponse(payload);
                await transaction.CommitAsync(CancellationToken.None);
                return response;
            });
        }
    }
}