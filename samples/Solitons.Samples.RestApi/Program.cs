using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Solitons;
using Solitons.Data;
using Solitons.Samples.Domain;
using Solitons.Samples.Domain.Contracts;
using Solitons.Samples.RestApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
}).AddXmlSerializerFormatters();

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

var env = IEnvironment.System;
var connectionString = env
    .GetEnvironmentVariable("SOLITONS_SAMPLE_CONNECTION_STRING")
    .ThrowIfNullOrWhiteSpace(() =>
        new InvalidOperationException("SOLITONS_SAMPLE_CONNECTION_STRING environment variable is missing."));
connectionString = new NpgsqlConnectionStringBuilder(connectionString)
{
    ApplicationName = "Sample API",
    MinPoolSize = 2
}.ConnectionString;

var context = SampleDomainContext.GetOrCreate();
var serializer = context.GetSerializer();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ClaimsPrincipal>(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new ClaimsPrincipal());
builder.Services.AddTransient<ITransactionScriptApiProvider>(provider =>
{
    var caller = provider.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new ClaimsPrincipal();
    return new PgTransactionScriptApiProvider(caller, serializer, connectionString);
});
builder.Services.AddTransient<ITransactionScriptApi>(serviceProviders =>
{
    var provider = serviceProviders.GetService<ITransactionScriptApiProvider>();
    return context.Implement<ITransactionScriptApi>(provider);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
