using System.Diagnostics;
using System.Net;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Identity.Web;
using Solitons;
using Solitons.Reflection;
using Solitons.Samples.Azure;
using Solitons.Samples.Domain;
using Solitons.Samples.Frontend.Server;



const string appletId = "Sample Web Server";


var azureServiceFactory = new AzureServiceFactory(IEnvironment.System);

var logger = azureServiceFactory
    .GetLogger()
    .WithAssemblyInfo(Assembly.GetExecutingAssembly())
    .WithEnvironmentInfo()
    .WithAppletId(appletId)
    .FireAndForget(AppletEvent.StartingUp);


var schedulers = Enumerable
    .Range(1, Environment.ProcessorCount)
    .Select(_ => new EventLoopScheduler())
    .ToArray();

var objectGraphInspectors = schedulers
    .Select(scheduler=> new ObjectGraphInspector(scheduler))
    .ToArray();

var sasUriPropertyInspector = azureServiceFactory.GetSasUriPropertyInspector();


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

builder.Services.AddTransient<ObjectGraphInspector>(serviceProviders =>
{
    var env = serviceProviders.GetService<IWebHostEnvironment>();
    var httpContext = serviceProviders.GetService<IHttpContextAccessor>()?.HttpContext;
    var clientIpAddress = httpContext?.Connection?.RemoteIpAddress ?? IPAddress.Any;

    var objectInspector = objectGraphInspectors.GetRandomElement();

    if (env.IsDevelopment() && 
        IPAddress.IsLoopback(clientIpAddress))
    {
        return objectInspector
            .WithPropertyInspector(sasUriPropertyInspector
                .WithIpAddress(MyPublicIpAddress.Value));
    }

    return objectInspector
        .WithPropertyInspector(sasUriPropertyInspector
            .WithIpAddress(clientIpAddress));
});

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
        .WithProperty(LogPropertyNames.UserEmail, email)
        .WithProperty(LogPropertyNames.RequestUri, url)
        .WithProperty(LogPropertyNames.RemoteIpAddress, remoteIpAddress.ToString());

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
    logger
        .AsObservable()
        .Subscribe(IAsyncLogger.Trace.AsObserver());
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
