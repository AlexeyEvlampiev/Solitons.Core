using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Identity.Web;
using Solitons;
using Solitons.Reflection;
using Solitons.Samples.Azure;
using Solitons.Samples.Domain;
using Solitons.Samples.Frontend.Server;


var staticRoots = new HashSet<object>();

var azFactory = new AzureFactory();

var logger = azFactory
    .GetLogger()
    .WithProperty("assembly",typeof(Program).Assembly.FullName);

var signer = azFactory.GetReadOnlySasAccessSigner();

#if DEBUG
staticRoots.Add(logger
    .AsObservable()
    .Subscribe(log=> Debug.WriteLine($"{log.Level}: {log.Message}")));
#endif

var adB2CSettings = new ConfigurationBuilder()
    .AddInMemoryCollection(azFactory.GetAzureActiveDirectoryB2CSettings())
    .Build()
    .GetSection(AzureActiveDirectoryB2CSettings.ConfigurationSectionName);

var pgConnectionString = azFactory.GetPgConnectionString(config =>
{
    config.ApplicationName = "Sample Frontend Server";
    config.MinPoolSize = 2;
});


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(staticRoots);
builder.Services.AddSingleton(RecursivePropertyInspector.Create(signer));

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

    string email = caller.FindFirstValue("emails");
    return logger
        .WithProperty("email", email)
        .WithProperty("url", url)
        .WithProperty("host", Environment.MachineName)
        .WithTags("Sample Frontend Server");

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
