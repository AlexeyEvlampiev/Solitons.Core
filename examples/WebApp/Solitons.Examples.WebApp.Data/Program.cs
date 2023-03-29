using Solitons.Examples.WebApp.Data.CommandLine;
using Solitons.Examples.WebApp.Data.Options;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Solitons.Collections;
using Solitons.Examples.WebApp.Azure;
using Solitons.Examples.WebApp.Data.Model;
using Solitons.IO;

namespace Solitons.Examples.WebApp.Data;

class Program : IWebAppDbManagerCallback
{
    private readonly SecretsRepositoryOption _secretsOption = new SecretsRepositoryOption();
    private readonly TimeoutOption _timeoutOption = new TimeoutOption();


    [DebuggerNonUserCode]
    static Task<int> Main(string[] args) => new Program().RunAsync(args);

    private async Task<int> RunAsync(string[] args)
    {
        var validator = new Validator();

        var root = new RootCommand("WebAppDb CTL.");
        

        var deployCmd = new Command("deploy", "Deploys the WebAppDb database on the target Postgres server.");
        deployCmd.AddGlobalOption(_secretsOption);
        deployCmd.AddGlobalOption(_timeoutOption);
        deployCmd.SetHandler(DeployAsync);


        var dropCmd = new Command("drop", "Drops the WebAppDb database on the target Postgres server.");
        dropCmd.AddGlobalOption(_secretsOption);
        dropCmd.AddGlobalOption(_timeoutOption);
        dropCmd.SetHandler(DropAsync);
        {
            var dropAndRecreateCmd = new Command("recreate", "Description goes here...");
            dropAndRecreateCmd.SetHandler(DropAndRecreateAsync);
            dropCmd.AddCommand(dropAndRecreateCmd);
        }
        


        root.AddCommand(deployCmd);
        root.AddCommand(dropCmd);
        return await root.InvokeAsync(args);
    }

    async Task<int> DeployAsync(InvocationContext context)
    {
        var secrets = context.ParseResult.GetValueForOption(_secretsOption)!;
        var timeoutCancellation = context.ParseResult.GetValueForOption(_timeoutOption);
        var externalCancellation = context.GetCancellationToken();
        var cts = CancellationTokenSource
            .CreateLinkedTokenSource(externalCancellation, timeoutCancellation);
        var connectionString = await secrets.GetSecretIfExistsAsync(SecretKeys.MaintenanceDbConnectionString);
        if (connectionString.IsNullOrWhiteSpace())
        {
            throw new NotImplementedException();
        }
        return await WebAppDbManager
            .Create(connectionString!, secrets)
            .CreateDbAsync(cts.Token)
            .ToObservable()
            .Select(_ => 0)
            .Catch((Exception ex) => OnError(ex));
    }

    async Task<int> DropAsync(InvocationContext context)
    {
        var secrets = context.ParseResult.GetValueForOption(_secretsOption)!;
        var timeoutCancellation = context.ParseResult.GetValueForOption(_timeoutOption);
        var externalCancellation = context.GetCancellationToken();
        var cts = CancellationTokenSource
            .CreateLinkedTokenSource(externalCancellation, timeoutCancellation);
        var connectionString = await secrets.GetSecretIfExistsAsync(SecretKeys.MaintenanceDbConnectionString);
        if (connectionString.IsNullOrWhiteSpace())
        {
            throw new NotImplementedException();
        }
        return await WebAppDbManager
            .Create(connectionString!, secrets)
            .CreateDbAsync(cts.Token)
            .ToObservable()
            .Select(_ => 0)
            .Catch((Exception ex) => OnError(ex));
    }


    async Task<int> DropAndRecreateAsync(InvocationContext context)
    {
        var secrets = context.ParseResult.GetValueForOption(_secretsOption)!;
        var timeoutCancellation = context.ParseResult.GetValueForOption(_timeoutOption);
        var externalCancellation = context.GetCancellationToken();
        var cts = CancellationTokenSource
            .CreateLinkedTokenSource(externalCancellation, timeoutCancellation);
        var connectionString = await secrets.GetSecretIfExistsAsync(SecretKeys.MaintenanceDbConnectionString);
        connectionString = ThrowIf.NullOrWhiteSpace(connectionString);

        var manager = WebAppDbManager
            .Create(connectionString, secrets);


        bool approved = ConsoleColor.Yellow.AsForegroundColor(() =>
        {
            Console.Write(FluentArray
                .Create($"You are about to drop the entire '{manager.Database}' database.",
                    $"This action cannot be undone.",
                    $"Are you sure you want to proceed? (Y/N):")
                .Join(Environment.NewLine));
            return ConsoleKeypressObservable
                .GetYesNoAsync(cts.Token)
                .GetAwaiter()
                .GetResult();
        });
        
        if (approved == false)
        {
            return 0;
        }
        Console.WriteLine();

        try
        {
            await manager
                .DropDbAsync(cts.Token);

            await manager
                .CreateDbAsync(cts.Token);

            return 0;
        }
        catch (Exception e)
        {
            return await OnError(e);
        }

    }

    private IObservable<int> OnError(Exception ex)
    {
        ConsoleColor.Red
            .AsForegroundColor(() => Console.WriteLine(ex.Message));
        return Observable.Return(1);
    }

    public void OnOperationRetry(string operation)
    {
        ConsoleColor.DarkGray
            .AsForegroundColor(() => Console.WriteLine($"Retrying: {operation.Quote()}"));
    }

    public void OnOperationFailed(string operation, Exception error)
    {
        ConsoleColor.Yellow
            .AsForegroundColor(() => Console.WriteLine($"{operation.Quote()}: {error.Message}"));
    }

    public void OnCreatingDatabase(string databaseName, PgServerInfo info)
    {
        Console.WriteLine($"Creating {databaseName}");
        Console.WriteLine($"\tHost: {info.Host}");
        Console.WriteLine($"\tPort: {info.Port}");
        Console.WriteLine($"\tMaintenance db: {info.Database}");
        Console.WriteLine($"\tUsername: {info.Username}");
    }

    public void OnValidatingUserPermissions(
        string commandText)
    {
        Console.WriteLine($"Validating user permissions.");
        ConsoleColor.DarkGreen
            .AsForegroundColor(() => Console.WriteLine(commandText));
    }
}