using System.Data;
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
            string procedure, 
            string content, 
            string contentType,
            int timeoutInSeconds, 
            IsolationLevel isolationLevel,
            CancellationToken cancellation)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await using var command = new NpgsqlCommand($"SELECT api.{procedure}(@request);", connection);

            var requestType = contentType switch
            {
                "application/json"=> NpgsqlDbType.Jsonb,
                "application/xml" => NpgsqlDbType.Xml,
                _=> throw new NotImplementedException()
            };
            command.Parameters.AddWithValue("request", requestType, content);
            await connection.OpenAsync(cancellation);
            var response = await command.ExecuteScalarAsync(cancellation) ?? throw new NullReferenceException();
            return response.ToString()!;
        }

    }
}
