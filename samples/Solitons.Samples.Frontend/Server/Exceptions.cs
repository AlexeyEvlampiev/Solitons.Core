using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace Solitons.Samples.Frontend.Server
{
    public static class Exceptions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IAsyncLogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var ex = contextFeature.Error;
                        Debug.WriteLine(ex.Message);
                        var correlationId = Guid.NewGuid();
                        await logger.ErrorAsync(ex.Message, log=> log
                            .WithDetails(ex.ToString())
                            .WithTag(correlationId));
                        var response = new
                        {
                            message = "Internal Server Error.",
                            correlationId = correlationId
                        };
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });
        }
    }
}
