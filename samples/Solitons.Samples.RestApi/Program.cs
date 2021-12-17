using Solitons;
using Solitons.Samples.Domain;
using Solitons.Samples.RestApi;
using Solitons.Samples.RestApi.Backend;
using System.Security.Claims;

Math.Pow(1, 2);


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

app.Map("/{**resource}", async (HttpRequest aspNetRequest, ClaimsPrincipal caller) =>
{
    var request = converter.ToWebRequest(aspNetRequest, caller);
    var result = await server.InvokeAsync(request, logger);
    return converter.ToAspNetResult(result, request);
});

app.Run();
