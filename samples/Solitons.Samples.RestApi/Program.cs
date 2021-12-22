using Solitons;
using Solitons.Samples.Domain;
using Solitons.Samples.RestApi;
using Solitons.Samples.RestApi.Backend;
using System.Security.Claims;



var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


var connectionString = IEnvironment.System
    .GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.Process)
    .ThrowIfNullOrWhiteSpace(()=> new InvalidOperationException("ConnectionString environment variable is required."));

var server = SampleDomainContext
    .GetOrCreate()
    .BuildWebServer(new SampleDbHttpEventHandler(connectionString));
// .WithInterceptors

var logger = IAsyncLogger.Null;
var converter = new AspNetMessageConverter();

app.Map("/{**eventArgs}", async (HttpRequest aspNetHttpRequest, ClaimsPrincipal caller) =>
{
    var webRequest = converter.ToWebRequest(aspNetHttpRequest, caller);
    var webResponse = await server.InvokeAsync(webRequest, logger);
    return converter.ToAspNetResult(webResponse, webRequest);
});

app.Run();
