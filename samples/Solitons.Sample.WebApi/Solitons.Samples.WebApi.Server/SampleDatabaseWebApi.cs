using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using Solitons.Data.Web;

namespace Solitons.Samples.WebApi.Server
{
    public class SampleDatabaseWebApi : DatabaseWebApi
    {
        
        public async Task ProcessAsync(HttpContext context, CancellationToken cancellation = default)
        {
            var request = new HttpRequestMessage(
                new HttpMethod(context.Request.Method), 
                new Uri(context.Request.GetEncodedUrl()));

            var response = await HandleAsync(request, cancellation);
            context.Response.StatusCode = (int)response.StatusCode;
            context.Response.ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";
            await response.Content.CopyToAsync(context.Response.Body, null, CancellationToken.None);
        }


        protected override ISession CreateCommand(string correlationId)
        {
            return new SampleSession();
        }

        sealed class SampleSession : SessionBase
        {
            protected override Task<HttpResponseMessage> HandleAsync(HttpRequestMessage message, CancellationToken cancellation)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    Content = new StringContent("Iron Maiden")
                });
            }

            protected override Task RollbackIfActiveAsync()
            {
                return Task.CompletedTask;
            }

            protected override ValueTask DisposeAsync()
            {
                return ValueTask.CompletedTask;
            }
        }
    }
}
