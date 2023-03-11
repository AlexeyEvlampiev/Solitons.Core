using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Solitons.Net.Http.Common;

namespace Solitons.Net.Http
{
    public sealed class SqLiteHttpTransactionHandler : HttpTransactionHandler
    {
        private readonly string _connectionString;

        private readonly Route[] _routes = {
            new CustomerListRoute(),
            new CustomerCreateRoute()
        };

        private SqLiteHttpTransactionHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static IHttpTransactionHandler Create(string connectionString) => 
            new SqLiteHttpTransactionHandler(connectionString);

        protected override async Task<HttpTransactionCallbackBase> ExecuteAsync(
            HttpRequestMessage request, 
            CancellationToken cancellation)
        {
            var sql = await request.Content!.ReadAsStringAsync(cancellation);
            var connection = new SQLiteConnection(_connectionString);
            
            try
            {
                await using var command = new SQLiteCommand(sql, connection);
                await connection.OpenAsync(cancellation);
                var transaction = await connection.BeginTransactionAsync(cancellation);
                await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellation);
                {
                    if (false == await reader.ReadAsync(cancellation))
                    {
                        throw new InvalidOperationException();
                    }
                    return new SqLiteHttpTransactionCallback(request, reader, transaction, cancellation);
                }
            }
            catch (Exception e)
            {
                await connection.DisposeAsync();
                throw;
            }
        }

        public override HttpMessageHandler AsHttpMessageHandler(HttpTransactionInterceptor interceptor)
        {
            var innerHandler = base.AsHttpMessageHandler(interceptor);
            var routingHandler = new RoutingHandler(_routes)
            {
                InnerHandler = innerHandler
            };
            return routingHandler;
        }


        abstract class Route
        {
            public abstract bool IsMatch(HttpRequestMessage request);
            public abstract HttpResponseMessage? PreProcess(HttpRequestMessage request);
        }

        sealed class CustomerListRoute : Route
        {
            public override bool IsMatch(HttpRequestMessage request) =>
                Regex.IsMatch(request.RequestUri?.PathAndQuery ?? "", @"/customers\b") &&
                request.Method == HttpMethod.Get;

            public override HttpResponseMessage? PreProcess(HttpRequestMessage request)
            {
                request.Content = new StringContent(@"
                SELECT 
                    200,
                    json_object('Content-Type', 'application/json'),
                    json_object('data', json_group_array(json_object('id', Id, 'name', Name, 'email', Email)))
                FROM Customers;");
                return null;
            }
        }


        sealed class CustomerCreateRoute : Route
        {
            public override bool IsMatch(HttpRequestMessage request) =>
                Regex.IsMatch(request.RequestUri?.PathAndQuery ?? "", @"/customers\b") &&
                request.Method == HttpMethod.Post;

            public override HttpResponseMessage? PreProcess(HttpRequestMessage request)
            {
                var payload = request.Content?.ReadAsStringAsync()?.Result;
                var json = JObject.Parse(payload);
                var name = json["name"]?.ToString();
                var email = json["email"]?.ToString();
                if (name.IsNullOrWhiteSpace())
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Customer name is required")
                    };
                }
                if (email.IsNullOrWhiteSpace())
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Customer email is required")
                    };
                }

                request.Content = new StringContent($@"
                INSERT INTO Customers(Name, Email) 
                VALUES ('{name}', '{email}');
                SELECT 
                    202,
                    json_object('Content-Type', 'application/json'),
                    json_object(
                        'id', last_insert_rowid(),
                        'name', Name,
                        'email', Email
                    ) AS customer
                FROM Customers
                WHERE rowid = last_insert_rowid();");
                return null;
            }
        }


        sealed class RoutingHandler : DelegatingHandler
        {
            private readonly IEnumerable<Route> _routes;

            public RoutingHandler(IEnumerable<Route> routes)
            {
                _routes = routes;
            }

            //[DebuggerStepThrough]
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var route = _routes.FirstOrDefault(r => r.IsMatch(request));
                if (route is null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                return route.PreProcess(request) ?? 
                       await base.SendAsync(request, cancellationToken);
            }
        }
    }
}
