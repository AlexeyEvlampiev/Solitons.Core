using Npgsql;
using Polly;
using Solitons.Samples.Domain;
using Solitons.Web;
using Solitons.Web.Common;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using Solitons.Common;

namespace Solitons.Samples.RestApi.Backend
{
    public sealed class SampleDbHttpEventHandler : HttpEventHandler
    {
        private readonly IReadOnlyDictionary<Type, DatabaseHttpTriggerArgsAttribute> _commands;
        private readonly AsyncPolicy _retryPolicy;
        private readonly string _connectionString;

        private SampleDbHttpEventHandler(string connectionString, SampleDomainContext context) 
            : base(context.GetSerializer())
        {
            _commands = context.GetDatabaseExternalTriggerArgs<DatabaseHttpTriggerArgsAttribute>();
            _connectionString = connectionString;
            _retryPolicy = Policy
                .Handle<NpgsqlException>(ex => ex.IsTransient)
                .RetryAsync(3);
        }

        [DebuggerStepThrough]
        public SampleDbHttpEventHandler(string connectionString) 
            : this(connectionString, SampleDomainContext.GetOrCreate())
        {            

        }


        protected override async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var clone = principal
                .ThrowIfNullArgument(nameof(principal))
                .Clone();
            var extended = (ClaimsIdentity)clone.Identity;
            extended.AddClaim(new Claim(ClaimTypes.NameIdentifier, "00000000-0000-0000-0000-000000000001"));
            return clone;
        }

        protected override async Task<WebResponse> GetResponseAsync(WebRequest request, IAsyncLogger logger, CancellationToken cancellation)
        {
            var httpEventArgs = request.HttpEventArgs
                .ThrowIfNull(()=> new NullReferenceException($"{typeof(WebRequest)}.{nameof(request.HttpEventArgs)}"));

            if (false == _commands.TryGetValue(httpEventArgs.GetType(), out var attribute))
                return WebResponse.Create(System.Net.HttpStatusCode.NotFound);

           
            var caller = request.Caller;
            var userIdClaim = caller.Claims.Where(c=> c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

            if(userIdClaim is null || Guid.TryParse(userIdClaim.Value, out var userId) == false)
                return WebResponse.Create(System.Net.HttpStatusCode.Unauthorized);

            var argsText = Serializer.Serialize(httpEventArgs, attribute.ProcedureEventArgsContentType);
            var timeout = TimeSpan.Parse(attribute.DatabaseOperationTimeout);
            await using var connection = new NpgsqlConnection(_connectionString);
            await using var command = new NpgsqlCommand($@"
                DO
                $$
                BEGIN
                    PERFORM api.set_user_context('{userId}');
                END;
                $$;            
                SELECT status, content_type, content FROM api.{attribute.Procedure}(@http_args, @content_type, @content);");
            command.Parameters.AddWithValue("http_args", NpgsqlTypes.NpgsqlDbType.Jsonb, argsText);
            command.Parameters.AddWithValue("content_type", attribute.ProcedurePayloadContentType);
            command.Parameters.AddWithValue("content", DBNull.Value);
            command.CommandTimeout = Convert.ToInt32(timeout.TotalSeconds);
            command.Connection = connection;

            return await _retryPolicy.ExecuteAsync(GetResponseAsync);

            async Task<WebResponse> GetResponseAsync()
            {
                await connection.OpenAsync(cancellation);
                await using var transaction = await connection.BeginTransactionAsync(attribute.IsolationLevel, cancellation);
                await using var record = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellation);
                if (false == await record.ReadAsync())
                    throw new NotImplementedException();
                var status = (System.Net.HttpStatusCode)record.GetInt32("status");
                var contentType = record.GetString("content_type");
                var content = record.GetString("content");
                record.Close();
                await transaction.CommitAsync();
                var _ = connection.CloseAsync();


                return WebResponse.Create(status, content, contentType);
            }                                 
        }

        protected override IHttpEventArgsAttribute? FindHttpEventArgsDescriptor(object httpEventArgs) => 
            _commands.TryGetValue(httpEventArgs.GetType(), out var attribute) ? attribute : null;
    }
}
