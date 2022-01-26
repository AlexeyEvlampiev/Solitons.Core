﻿using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Solitons.Samples.Azure;

namespace Solitons.Samples.Frontend.Server
{
    public static class Exceptions
    {
        public static void UseMachinePublicIpAsRemoteAddress(this IApplicationBuilder app)
        {
            app.Use(async (HttpContext context, RequestDelegate rd) =>
            {
                context.Connection.RemoteIpAddress = await MyPublicIpAddress.GetAsync();
                await rd.Invoke(context);
            });
        }
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
