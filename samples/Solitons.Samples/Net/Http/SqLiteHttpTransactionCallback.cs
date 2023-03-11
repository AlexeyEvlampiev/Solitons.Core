using System.Data.Common;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Solitons.Net.Http.Common;

namespace Solitons.Net.Http
{
    // ReSharper disable once InconsistentNaming
    sealed class SqLiteHttpTransactionCallback : HttpTransactionCallback
    {
        private readonly DbTransaction _transaction;
        private readonly DbConnection _connection;
        private readonly CancellationToken _cancellation;

        public SqLiteHttpTransactionCallback(
            HttpRequestMessage request,
            DbDataReader record,
            DbTransaction transaction,
            CancellationToken cancellation) 
            : base(request, CreateHttpResponse(record), transaction)
        {
            _transaction = transaction;
            _connection = ThrowIf.NullReference(transaction.Connection);
            _cancellation = cancellation;
        }

        private static HttpResponseMessage CreateHttpResponse(DbDataReader record)
        {
            var statusCode = (HttpStatusCode)record.GetInt32(0);
            var headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(record.GetString(1))
                !;
            var payload = record
                .GetString(2)
                .Convert(JObject.Parse)
                .Convert(json => json.ToString(Formatting.Indented));

            var r = new HttpResponseMessage();
            r.StatusCode = statusCode;
            r.Content = new StringContent(payload);
            r.Content.Headers.Clear();
            headers.ForEach(h =>
            {
                var added = 
                        r.Headers.TryAddWithoutValidation(h.Key, h.Value) ||
                        r.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
                Debug.Assert(added);
            });
            return r;
        }

        
    }
}
