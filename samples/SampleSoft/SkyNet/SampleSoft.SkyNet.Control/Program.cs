using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using Npgsql;
using SampleSoft.SkyNet.Azure;
using SampleSoft.SkyNet.Azure.CommandLine;
using SampleSoft.SkyNet.Azure.Manifest;
using SampleSoft.SkyNet.Control.Options;
using SampleSoft.SkyNet.Control.SkyNetDb;
using Solitons;
using Solitons.Common;
using Solitons.IO;

namespace SampleSoft.SkyNet.Control;

sealed class Program : ProgramBase
{
    private readonly TimeoutOption _timeoutOption = new TimeoutOption();
    private readonly ManifestArgument _manifestArgument = new ManifestArgument();

    private readonly Option<bool> _recreateSkyNetDbOption = new Option<bool>(
        "--recreate",
        getDefaultValue: () => false,
        "");

    private readonly RootCommand _rootCommand = new RootCommand();

    [DebuggerStepThrough]
    public static Task<int> Main(string[] args) => new Program(args).RunAsync();

    public Program(string[] args) : base(args)
    {
        var skynetDbCreateCommand = new Command(
            "create", 
            "Creates or updates the SkyNet Postgres database using the provided manifest.");
        skynetDbCreateCommand.AddArgument(_manifestArgument);
        skynetDbCreateCommand.SetHandler(CreateSkyNetDb);


        var skynetDbDropCommand = new Command("drop", "");
        skynetDbDropCommand.AddArgument(_manifestArgument);
        skynetDbDropCommand.AddOption(_recreateSkyNetDbOption);
        skynetDbDropCommand.Description = "Drops or re-creates the SkyNet postgres database.";
        skynetDbDropCommand.SetHandler(DropSkyNetDb);

        var skynetDbCommand = new Command("skynetdb", "");
        skynetDbCommand.AddCommand(skynetDbCreateCommand);
        skynetDbCommand.AddCommand(skynetDbDropCommand);

        _rootCommand.AddCommand(skynetDbCommand);
        _rootCommand.Description = "SkyNet CLI";
    }

    private async Task<int> CreateSkyNetDb(InvocationContext context)
    {
        var cancellation = context.ParseResult
            .GetValueForOption(_timeoutOption)
            .Join(context.GetCancellationToken());
        IManifest manifest = context.ParseResult.GetValueForArgument(_manifestArgument)!;
        
        var secrets = manifest.GetSecretsRepository();
        if (manifest.HasMaintenanceDbConnectionString(out var connectionString))
        {
            await secrets.SetSecretAsync(
                KeyVaultSecretNames.SkyNetPgServerConnectionString,
                connectionString);
        }

        var config = new SkyNetDbManagerConfig
        {
            SharedPassword = manifest.SharedPassword
        };
        var manager = await SkyNetDbManager.CreateAsync(secrets, config);
        await manager.CreateDbAsync(cancellation);
        return 0;
    }


    private async Task<int> DropSkyNetDb(InvocationContext context)
    {
        var cancellation = context.ParseResult
            .GetValueForOption(_timeoutOption)
            .Join(context.GetCancellationToken());
        var recreate = context.ParseResult.GetValueForOption(_recreateSkyNetDbOption);
        IManifest manifest = context.ParseResult.GetValueForArgument(_manifestArgument)!;

        var secrets = manifest.GetSecretsRepository();
        if (manifest.HasMaintenanceDbConnectionString(out var connectionString))
        {
            await secrets.SetSecretAsync(
                KeyVaultSecretNames.SkyNetPgServerConnectionString,
                connectionString);
        }
        else
        {
            connectionString = await secrets
                .GetSecretIfExistsAsync(KeyVaultSecretNames.SkyNetPgServerConnectionString);
        }

        var config = new SkyNetDbManagerConfig
        {
            SharedPassword = manifest.SharedPassword
        };
        var manager = await SkyNetDbManager.CreateAsync(secrets, config);
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var server = $"{builder.Host}:{builder.Port}";

        Console.WriteLine($"server: {server}");
        Console.WriteLine($"database: {manager.Database}");
        Console.WriteLine();

        bool approval = ConsoleColorScope.Invoke(ConsoleColor.Yellow, () =>
        {
            if (recreate)
            {
                Console.WriteLine($"You are about to delete and then recreate the {manager.Database} database.");
                Console.WriteLine("Please note that all data will be lost.");
            }
            else
            {
                Console.WriteLine($"You are about to permanently delete the {manager.Database} database from the {server} server. This operation cannot be undone.");
            }
            Console.WriteLine();
            return Prompt.GetYesNoAnswer("Are you sure you want to proceed?");

        });

        if (recreate)
        {
            await manager.DropAndRecreateAsync(cancellation);
        }
        else
        {
            await manager.DropDbAsync(cancellation);
        }
        
        return 0;
    }
    public override Task<int> RunAsync(CancellationToken cancellation = default)
    {
        return _rootCommand.InvokeAsync(this.Arguments.ToArray());
    }
}