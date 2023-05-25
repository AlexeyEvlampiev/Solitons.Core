using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Net;
using System.Text;
using Npgsql;
using NpgsqlTypes;

namespace Solitons.Data;

public sealed class PgHttpMessageHandler : DbHttpMessageHandler<NpgsqlConnection>
{
    [DebuggerStepThrough]
    public PgHttpMessageHandler(string connectionString)
        : base(connectionString)
    {
        using var conn = new NpgsqlConnection(connectionString);
    }

    [DebuggerStepThrough]
    public PgHttpMessageHandler(NpgsqlTransaction transaction) 
        : base(transaction)
    {
    }

    protected override async Task ExecuteAsync(
            NpgsqlConnection connection,
            HttpRequestMessage request,
            HttpResponseMessage response,
            CancellationToken cancellation)
    {
        var method = request.Method.ToString();
        var url = request.RequestUri?.PathAndQuery ?? "/";
        var body = request.Content is null
            ? "{}"
            : await request.Content!.ReadAsStringAsync(cancellation);
        var headers = request.Headers
            .ToDictionary(header => header.Key, header => header.Value.Join(","));

        await using var command = connection.CreateCommand();

        command.CommandText = @"SELECT * FROM api.http_invoke(@method, @url, @headers, @body)";
        command.Parameters.AddRange(new[]
        {
                new NpgsqlParameter("@method", NpgsqlDbType.Text) { Value = method },
                new NpgsqlParameter("@url", NpgsqlDbType.Text) { Value = url },
                new NpgsqlParameter("@headers", NpgsqlDbType.Hstore) { Value = headers },
                new NpgsqlParameter("@body", NpgsqlDbType.Jsonb) { Value = body }
            });
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellation);
        if (await reader.ReadAsync(cancellation))
        {
            response.StatusCode = (HttpStatusCode)(reader.GetInt32(0))!;
            headers = reader.GetFieldValue<Dictionary<string, string>>(1)!;
            foreach (var header in headers)
            {
                response.Headers.Remove(header.Key);
                response.Headers.Add(header.Key, header.Value);
            }
            body = reader.GetString(2)!;
            response.Content = new StringContent(body, Encoding.UTF8, "application/json");
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    protected override async Task<DbTransaction> BeginTransactionAsync(
        HttpRequestMessage request,
        NpgsqlConnection connection,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        DbTransaction transaction = await connection.BeginTransactionAsync(cancellation);
        return transaction;
    }

    [DebuggerNonUserCode]
    protected override NpgsqlConnection CreateProviderConnection(string connectionString) => new NpgsqlConnection(connectionString);
}