using Solitons;
using Solitons.Samples.Domain;
using Solitons.Samples.RestApi;
using Solitons.Samples.RestApi.Backend;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var context = SampleDomainContext.GetOrCreate();
var serializer = context.GetSerializer();

var connectionString = Environment
    .GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.Process)
    .ThrowIfNullOrWhiteSpace(()=> new InvalidOperationException("ConnectionString environment variable is required."));

var server = context.BuildWebServer(new SampleDbHttpEventHandler(connectionString));
var logger = IAsyncLogger.Null;
var converter = new AspNetMessageConverter();





app.Map("/{**catchAll}", async (HttpRequest aspNetRequest, ClaimsPrincipal caller) =>
{
    var request = converter.ToWebRequest(aspNetRequest, caller);
    var result = await server.InvokeAsync(request, logger);
    return converter.ToAspNetResult(result, request);
});

app.Run();
