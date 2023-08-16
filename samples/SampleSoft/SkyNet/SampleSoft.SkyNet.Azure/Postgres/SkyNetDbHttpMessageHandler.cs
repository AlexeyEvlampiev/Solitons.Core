using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Net;
using Microsoft.Identity.Client;
using Npgsql;
using NpgsqlTypes;
using Solitons;
using Solitons.Data;

namespace SampleSoft.SkyNet.Azure.Postgres;

/// <summary>
/// A specialized HttpMessageHandler designed to process HTTP requests via Postgres using the Npgsql .NET data provider.
/// It is intended to be used within an HTTP request pipeline where incoming requests are handled and processed by a Postgres database.
/// </summary>
sealed class SkyNetDbHttpMessageHandler : DbHttpMessageHandler<NpgsqlConnection>
{
    [DebuggerStepThrough]
    public SkyNetDbHttpMessageHandler(NpgsqlConnection connection) 
        : base(connection)
    {
    }

    public SkyNetDbHttpMessageHandler(string connectionString) 
        : base(() => new NpgsqlConnection(connectionString))
    {
    }

    protected override async Task ExecuteAsync(
        NpgsqlConnection connection, 
        HttpRequestMessage request, 
        HttpResponseMessage response,
        CancellationToken cancellation)
    {
        var headers = request.Headers
            .Select(h => KeyValuePair.Create(h.Key, h.Value.Join(",")))
            .ToDictionary(StringComparer.Ordinal);
        if (false == headers.TryGetValue("SKYNET-IDENTITY", out var identity))
        {
            identity = "anonymous";
        }


        var content = await (request.Content?.ReadAsStringAsync(cancellation) ?? Task.FromResult("{}"));
        content = content.DefaultIfNullOrWhiteSpace("{}");
        await using var command = connection.CreateCommand();
        command.CommandText = @$"
        SET LOCAL rpc.caller TO '{identity.ToLowerInvariant()}'; 
        SET ROLE skynetdb_api;
        SELECT * FROM api.http_invoke('{request.Method}'::text, '{request.RequestUri}'::text, @headers, @content);";
        command.Parameters.Add(new NpgsqlParameter("@headers", NpgsqlDbType.Hstore){ Value = headers });
        command.Parameters.Add(new NpgsqlParameter("@content", NpgsqlDbType.Jsonb) { Value = content });
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellation);
        if (false == await reader.ReadAsync(cancellation))
        {
            throw new InvalidOperationException("The database operation did not return any row. Check the SQL command and parameters.");
        }

        var statusCode = reader.GetInt32(0);
        headers = reader.GetFieldValue<Dictionary<string, string>>(1);
        content = reader.GetString(2);
        response.StatusCode = (HttpStatusCode)statusCode;
        response.Content = new StringContent(content);

        var comparer = StringComparer.OrdinalIgnoreCase;
        foreach (var header in headers)
        {
            if (comparer.Equals(header.Key, "Content-Type"))
            {
                response.Content.Headers.Remove("Content-Type");
                response.Content.Headers.Add("Content-Type", header.Value);
                continue;
            }
    
            bool added = response.Headers.TryAddWithoutValidation(header.Key, header.Value);
            Debug.WriteLine(added ? $"Added {header.Key} header." : $"Could not add {header.Key} header");
        }
    }
}