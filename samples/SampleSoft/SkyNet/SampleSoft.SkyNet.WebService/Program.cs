
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using SampleSoft.SkyNet.Azure.Http;
using SampleSoft.SkyNet.Azure.Postgres;
using Solitons;
using Solitons.Common;
using Solitons.Diagnostics;
using LogLevel = Solitons.Diagnostics.LogLevel;

namespace SampleSoft.SkyNet.WebService;

public sealed class Program : ProgramBase
{
    private readonly RootCommand _rootCommand = new RootCommand();

    [DebuggerStepThrough]
    public static Task<int> Main(string[] args) => new Program(args).RunAsync();

    //[DebuggerStepThrough]
    public Program(string[] args) : base(args)
    {
        _rootCommand.SetHandler(RunAsync);
        _rootCommand.Description = "SkyNet Web Service";
    }


    [DebuggerNonUserCode]
    public override Task<int> RunAsync(CancellationToken cancellation = default) => _rootCommand.InvokeAsync(Arguments.ToArray());

    private async Task<int> RunAsync(InvocationContext context)
    {
        var cancellation = CancellationToken.None;

        //var amqpTransportType = context.ParseResult.GetValueForOption(_amqpTransportOption);


        var builder = WebApplication.CreateBuilder(base.Arguments.ToArray());


        // Add services to the container.
        builder.Services.AddAuthorization();



        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var rootLogger = IAsyncLogger.Null;

        if (app.Environment.IsDevelopment())
        {

        }


        await using var disposer = new AsyncStackAutoDisposer();

        var client = new SkyNetDbHttpClient("Database connection string goes here");
        disposer.AddResource((IAsyncDisposable)client);


        app.Map("/{*resource}", async (
            HttpContext aspNetContext, 
            IAsyncLogger logger) =>
        {
            Trace.WriteLine(aspNetContext.Request.GetDisplayUrl());
            using var httpRequest = HttpConverter.Convert(aspNetContext.Request);
            httpRequest.AddLogger(logger);
            using var response = await client.SendAsync(httpRequest, cancellation);
            await HttpConverter.PopulateAsync(aspNetContext.Response, response);
        });

        var startedUtc = DateTimeOffset.UtcNow;
        rootLogger = rootLogger.WithProperty("startedAt", startedUtc);
        try
        {
            await Task.WhenAny(
                client.RunAsync(cancellation),
                app.RunAsync(cancellation));

            await rootLogger
                .InfoAsync("Execution completed.", log => log
                .WithProperty("totalDuration", (DateTime.UtcNow) - startedUtc));
            return 0;
        }
        catch (OperationCanceledException ex)
        {
            await rootLogger
                .InfoAsync("Execution cancelled.", log => log
                .WithProperty("totalDuration", (DateTime.UtcNow) - startedUtc));
            return 0;
        }
        catch (Exception e)
        {
            await rootLogger
                .ErrorAsync(e, log => log
                .WithProperty("totalDuration", (DateTime.UtcNow) - startedUtc));
            return 1;
        }
        finally
        {
            await disposer.DisposeAsync();
        }

    }
}