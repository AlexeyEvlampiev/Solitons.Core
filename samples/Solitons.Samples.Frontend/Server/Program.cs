using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Solitons;
using Solitons.Data;
using Solitons.Samples.Azure;
using Solitons.Samples.Domain;
using Solitons.Samples.Frontend.Server;


var domainContext = SampleDomainContext.GetOrCreate();
var adB2CSettings = new ConfigurationBuilder()
    .AddInMemoryCollection(EnvironmentVariables.GetAzureActiveDirectoryB2CSettings())
    .Build()
    .GetSection(AzureActiveDirectoryB2CSettings.ConfigurationSectionName);

var pgConnectionString = EnvironmentVariables.GetPgConnectionString(config =>
{
    config.ApplicationName = "Sample Frontend Server";
    config.MinPoolSize = 2;
});


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(adB2CSettings);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ITransactionScriptProvider>(provider =>
{
    var caller = provider.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new ClaimsPrincipal();
    return new PgTransactionScriptProvider(caller, pgConnectionString);
});
builder.Services.AddTransient(serviceProviders =>
{
    var provider = serviceProviders.GetService<ITransactionScriptProvider>();
    return domainContext.Create<IDatabaseApi>(provider);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.ConfigureExceptionHandler(IAsyncLogger.Null);

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
