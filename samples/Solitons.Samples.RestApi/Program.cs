using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Solitons.Data;
using Solitons.Samples.Azure;
using Solitons.Samples.Domain;
using Solitons.Samples.Domain.Contracts;

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

var connectionString = SampleEnvironment.GetPgConnectionString(config =>
{
    config.ApplicationName = "Solitons Sample Rest API";
    config.MinPoolSize = 2;
});


var domainContext = SampleDomainContext.GetOrCreate();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ITransactionScriptProvider>(provider =>
{
    var caller = provider.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new ClaimsPrincipal();
    return new PgTransactionScriptProvider(caller, connectionString);
});
builder.Services.AddTransient(serviceProviders =>
{
    var provider = serviceProviders.GetService<ITransactionScriptProvider>();
    return domainContext.Create<IDatabaseApi>(provider);
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
