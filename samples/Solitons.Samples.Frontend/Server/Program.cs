using System.Security.Claims;
using Solitons.Data;
using Solitons.Samples.Azure;
using Solitons.Samples.Domain;
using Solitons.Samples.Domain.Contracts;


var connectionString = SampleEnvironment.GetPgConnectionString(config =>
{
    config.ApplicationName = "Solitons Sample Frontend";
    config.MinPoolSize = 2;
});


var domainContext = SampleDomainContext.GetOrCreate();


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ITransactionScriptProvider>(provider =>
{
    var caller = provider.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new ClaimsPrincipal();
    return new PgTransactionScriptProvider(caller, connectionString);
});
builder.Services.AddTransient(serviceProviders =>
{
    var provider = serviceProviders.GetService<ITransactionScriptProvider>();
    return domainContext.Create<ITransactionScriptApi>(provider);
});

builder.Services
    .AddControllersWithViews(config=> config.RespectBrowserAcceptHeader = true)
    .AddXmlSerializerFormatters();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
