using System.Diagnostics;
using System.Net;
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
                        //logger.LogError($"Something went wrong: {contextFeature.Error}");
                        //await context.Response.WriteAsync(new ErrorDetails()
                        //{
                        //    StatusCode = context.Response.StatusCode,
                        //    Message = "Internal Server Error."
                        //}.ToString());
                    }
                });
            });
        }
    }
}
