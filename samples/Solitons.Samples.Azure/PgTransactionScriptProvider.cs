using System.Security.Claims;
using Npgsql;
using NpgsqlTypes;
using Solitons.Data;
using Solitons.Data.Common;

namespace Solitons.Samples.Azure
{
    public class PgTransactionScriptProvider : TransactionScriptProvider
    {
        private readonly string _connectionString;
        private readonly ClaimsPrincipal _caller;


        public PgTransactionScriptProvider(ClaimsPrincipal caller, string connectionString)
        {
            _caller = caller.ThrowIfNullArgument(nameof(caller));
            _connectionString = connectionString.ThrowIfNullOrWhiteSpaceArgument(nameof(connectionString));
        }

        protected override async Task<string> InvokeAsync(
            StoredProcedureAttribute procedureMetadata,
            StoredProcedureRequestAttribute requestMetadata,
            StoredProcedureResponseAttribute responseMetadata,
            string request, 
            CancellationToken cancellation)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await using var command = new NpgsqlCommand($"SELECT api.{procedureMetadata.Procedure}(@request);", connection);

            var requestType = requestMetadata.ContentType switch
            {
                "application/json"=> NpgsqlDbType.Jsonb,
                "application/xml" => NpgsqlDbType.Xml,
                _=> throw new NotImplementedException()
            };
            command.Parameters.AddWithValue("request", requestType, request);
            await connection.OpenAsync(cancellation);
            var response = await command.ExecuteScalarAsync(cancellation) ?? throw new NullReferenceException();
            return response.ToString()!;
        }

    }
}
