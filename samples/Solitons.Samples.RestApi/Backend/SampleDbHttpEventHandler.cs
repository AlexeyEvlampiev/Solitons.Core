using Npgsql;
using Polly;
using Solitons.Samples.Domain;
using Solitons.Samples.Domain.Contracts;
using Solitons.Web;
using Solitons.Web.Common;
using System.Data;
using System.Diagnostics;
using System.Reactive.Linq;

namespace Solitons.Samples.RestApi.Backend
{
    public sealed class SampleDbHttpEventHandler : HttpEventListener
    {
        private readonly IReadOnlyDictionary<Type, SampleDbHttpTriggerAttribute> _commands;
        private readonly IDomainSerializer _serializer;
        private readonly AsyncPolicy _policy;
        private readonly string _connectionString;

        public SampleDbHttpEventHandler(string connectionString)
        {
            var context = SampleDomainContext.GetOrCreate();
            _commands = context.GetDbCommandArgs<SampleDbHttpTriggerAttribute>();
            _serializer = context.GetSerializer();
            _connectionString = connectionString;
            _policy = Policy
                .Handle<NpgsqlException>(ex=> ex.IsTransient)
                .RetryAsync(3);
        }

        protected override bool CanProcess(WebRequest request)
        {
            var args = request?.HttpEventArgs;
            if(args is null) return false;
            return _commands.ContainsKey(args.GetType());
        }

        protected override async Task<WebResponse> ProcessAsync(WebRequest request, CancellationToken cancellation)
        {
            var args = request?.HttpEventArgs.ThrowIfNull(()=> new NullReferenceException($"{typeof(WebRequest)}.{nameof(request.HttpEventArgs)}"));
            if (false == _commands.TryGetValue(args.GetType(), out var attribute))
                return WebResponse.Create(System.Net.HttpStatusCode.NotFound);

            if(attribute.ResponseObjectType is not null)
            {
                if (request.Accept.Contains("*/*"))
                {
                    Debug.WriteLine("Accept: */*");
                }
                else if (false == request.Accept.Any(contentType=> _serializer.CanSerialize(attribute.ResponseObjectType, contentType)))
                {
                    if (request.Accept.Any())
                    {
                        return WebResponse.Create(System.Net.HttpStatusCode.ExpectationFailed, "Expectation failed");
                    }
                }
            }
            var argsText = _serializer.Serialize(args, attribute.ProcedureArgsContentType);
            var timeout = TimeSpan.Parse(attribute.DatabaseOperationTimeout);
            await using var connection = new NpgsqlConnection(_connectionString);
            await using var command = new NpgsqlCommand($"SELECT status, content_type, content FROM api.{attribute.Procedure}(@http_args, @content_type, @content);");
            command.Parameters.AddWithValue("http_args", NpgsqlTypes.NpgsqlDbType.Jsonb, argsText);
            command.Parameters.AddWithValue("content_type", attribute.ProcedurePayloadContentType);
            command.Parameters.AddWithValue("content", DBNull.Value);
            command.CommandTimeout = Convert.ToInt32(timeout.TotalSeconds);
            command.Connection = connection;
            await connection.OpenAsync(cancellation);
            await using var transaction = await connection.BeginTransactionAsync(attribute.IsolationLevel, cancellation);
            await using var record = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellation);
            if (false == await record.ReadAsync())
                throw new NotImplementedException();
            var status = record.GetInt32("status");
            var contentType = record.GetString("content_type");
            var content = record.GetString("content");
            record.Close();
            await transaction.CommitAsync();
            

            if(attribute.ResponseObjectType is null || status >= 300)
            {
                return WebResponse.Create((System.Net.HttpStatusCode)status, content, contentType);
            }
            else
            {
                var dto = _serializer.Deserialize(attribute.ResponseObjectType, contentType, content);
                return WebResponse.Create((System.Net.HttpStatusCode)status, dto);
            }                      
        }
    }
}
