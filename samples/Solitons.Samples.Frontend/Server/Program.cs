using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Identity.Web;
using Solitons;
using Solitons.Diagnostics;
using Solitons.Samples.Azure;
using Solitons.Samples.Domain;
using Solitons.Samples.Frontend.Server;
using Solitons.Security;


const string appletId = "Sample Web Server";


var azureServiceFactory = new AzureServiceFactory(IEnvironment.System);

var logger = azureServiceFactory
    .GetLogger()
    .WithAssemblyInfo(Assembly.GetExecutingAssembly())
    .WithEnvironmentInfo()
    .WithAppletId(appletId)
    .FireAndForget(AppletEvent.StartingUp);


var adB2CSettings = new ConfigurationBuilder()
    .AddInMemoryCollection(azureServiceFactory.GetAzureActiveDirectoryB2CSettings())
    .Build()
    .GetSection(AzureActiveDirectoryB2CSettings.ConfigurationSectionName);

var pgConnectionString = azureServiceFactory.GetPgConnectionString(config =>
{
    config.ApplicationName = appletId;
    config.CommandTimeout = 3;
    config.MinPoolSize = 2;
    config.MaxPoolSize = 10;
});


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ISecureBlobAccessUriBuilder>(azureServiceFactory.GetSecureBlobAccessUriBuilder());

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(adB2CSettings);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ISampleDbApi>(serviceProviders =>
{
    var caller = serviceProviders.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new ClaimsPrincipal();
    var provider = new PgTransactionScriptProvider(caller, pgConnectionString);
    var databaseApi = new SampleDbApi(provider);
    return databaseApi;
});

builder.Services.AddTransient<IAsyncLogger>(serviceProviders =>
{
    var context = serviceProviders.GetService<IHttpContextAccessor>()?.HttpContext;
    if (context is null) return logger;
    var caller = context.User;
    var url = context.Request.GetDisplayUrl();

    var remoteIpAddress = context.Connection.RemoteIpAddress ?? IPAddress.None;

    string email = caller.FindFirstValue("emails");
    return logger
        .WithUserEmail(email)
        .WithRequestUri(url)
        .WithRemoteIpAddress(remoteIpAddress.ToString());

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
    app.UseMachinePublicIpAsRemoteAddress();
    logger
        .AsObservable()
        .Subscribe(IAsyncLogger.Console.AsObserver());
    Trace.Listeners.Add(new ConsoleTraceListener());
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.ConfigureExceptionHandler(logger);

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
