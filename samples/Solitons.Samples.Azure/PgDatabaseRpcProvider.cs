using System.Data;
using System.Diagnostics;
using Npgsql;
using NpgsqlTypes;
using Polly;
using Polly.Retry;
using Solitons.Data;
using Solitons.Data.Common;

namespace Solitons.Samples.Azure
{
    public sealed class PgDatabaseRpcProvider : DatabaseRpcProvider
    {
        private readonly string _connectionString;

        private static readonly AsyncRetryPolicy RetryPolicy = Policy
            .Handle<NpgsqlException>(ex => ex.IsTransient)
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200));

        public PgDatabaseRpcProvider(string connectionString)
        {
            _connectionString = connectionString.ThrowIfNullOrWhiteSpaceArgument(nameof(connectionString));
        }


        protected override Task<T> InvokeAsync<T>(DatabaseRpcCommandMetadata metadata, string request, Func<string, Task<T>> callback, CancellationToken cancellation)
        {
            return RetryPolicy.ExecuteAsync(async () =>
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await using var command = new NpgsqlCommand($"SELECT api.{metadata.Procedure}(@request);", connection);
                command.CommandTimeout = (int)metadata.OperationTimeout.TotalSeconds;
                var requestType = metadata.Request.ContentType switch
                {
                    "application/json" => NpgsqlDbType.Jsonb,
                    "application/xml" => NpgsqlDbType.Xml,
                    _ => throw new NotImplementedException()
                };
                command.Parameters.AddWithValue("request", requestType, request);
                await connection.OpenAsync(cancellation);
                await using var transaction = await connection.BeginTransactionAsync(metadata.IsolationLevel, cancellation);
                var dbResponse = await command.ExecuteScalarAsync(cancellation) ?? new NullReferenceException();
                var response = await callback.Invoke(dbResponse as string ?? throw new NullReferenceException());
                await transaction.CommitAsync(CancellationToken.None);
                return response!;
            });
        }

        protected override Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation)
        {
            return RetryPolicy.ExecuteAsync(async () =>
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await using var command = new NpgsqlCommand($"SELECT api.rpc_enqueue(@oid, @request);", connection);
                command.CommandTimeout = (int)metadata.OperationTimeout.TotalSeconds;
                command.Parameters.AddWithValue("request", request);
                command.Parameters.AddWithValue("oid", metadata.CommandOid);
                await connection.OpenAsync(cancellation);
                await using var transaction = await connection.BeginTransactionAsync(metadata.IsolationLevel, cancellation);
                await command.ExecuteNonQueryAsync(cancellation);
                await transaction.CommitAsync(CancellationToken.None);
            });
        }

        protected override Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, Func<Task> callback, CancellationToken cancellation)
        {
            return RetryPolicy.ExecuteAsync(async () =>
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await using var command = new NpgsqlCommand($"SELECT api.rpc_enqueue(@oid, @request);", connection);
                command.CommandTimeout = (int)metadata.OperationTimeout.TotalSeconds;

                command.Parameters.AddWithValue("request", request);
                command.Parameters.AddWithValue("oid", metadata.CommandOid);
                await connection.OpenAsync(cancellation);
                await using var transaction = await connection.BeginTransactionAsync(metadata.IsolationLevel, cancellation);
                await command.ExecuteNonQueryAsync(cancellation);
                await callback.Invoke();
                await transaction.CommitAsync(CancellationToken.None);
            });
        }

        protected override Task ProcessQueueAsync(string queueName, CancellationToken cancellation)
        {
            return RetryPolicy.ExecuteAsync(async () =>
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await using var command = new NpgsqlCommand($"SELECT * FROM api.rpc_dequeue(@queue);", connection);
                command.CommandTimeout = (int)TimeSpan.FromHours(2).TotalSeconds;

                command.Parameters.AddWithValue("queue", queueName);
                await connection.OpenAsync(cancellation);
                while (false == cancellation.IsCancellationRequested)
                {
                    try
                    {
                        await using var record = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellation);
                        if (await record.ReadAsync(cancellation) == false)
                        {
                            throw new InvalidOperationException($"Command returned 0 records. See command {command.CommandText}");
                        }

                        var success = record.GetBoolean(0);
                        var delay = record.GetTimeSpan(1);
                        var sequenceNumber = record.GetInt64(2);
                        var dequeueCount = record.GetInt32(3);
                        var procedure = record.IsDBNull(4) ? default(string) : record.GetString(4);
                        var comment = record.GetString(7);

                        Debug.WriteLine(success
                            ? $"\tPostgres message processed."
                            : $"\tCould not process postgres message. Reason: {comment}");

                        await Task.Delay(delay, cancellation);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        await Task.Delay(5000, cancellation);
                    }
                }
            });
        }
    }
}
