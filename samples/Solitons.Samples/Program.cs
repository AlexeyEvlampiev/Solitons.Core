using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Net;
using System.Reactive.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Npgsql;
using Solitons.Data;
using Solitons.Options;

namespace Solitons;
sealed class Program
{
    private readonly TimeoutOption _timeoutOption = new TimeoutOption();
    private readonly ServerConnectionOption _serverConnectionOption = new ServerConnectionOption();
    private readonly DatabaseConnectionOption _databaseConnectionOption = new DatabaseConnectionOption();
    private readonly TestSelectorOption _testSelectorOption = new TestSelectorOption();


    private async Task<int> RunAsync(string[] args)
    {

        Observable
            .Return(10)
            .WithRetryPolicy(attempt => attempt
                .SignalNextAttempt(attempt.AttemptNumber < 10)
                .Delay(TimeSpan
                    .FromSeconds(1)
                    .ScaleByFactor(1.1, attempt.AttemptNumber)));

        var client = new PgHttpClient(
            "host=localhost;database=webappdb;port=5430;username=postgres;password=postgres");
        var response = await client.GetAsync("");


        await client.SendAsync(new DbHttpRequestMessage(HttpMethod.Get, "")
            .WithCommitApproval(async (response, token) =>
            {

                return true;
            }));

        Console.WriteLine(response.StatusCode);
        response = await client.PostAsync("/actions?v=1.0", new StringContent("{}"));

        Console.WriteLine(response.StatusCode);


        //var recreateCommand = new Command("recreate", "");
        //recreateCommand.SetHandler(DropAndRecreateAsync);

        //var dropCommand = new Command("drop", "");
        //dropCommand.AddCommand(recreateCommand);
        //dropCommand.AddGlobalOption(_timeoutOption);
        //dropCommand.SetHandler(DropAsync);

        //var upCommand = new Command("up", "");
        //upCommand.AddOption(_timeoutOption);
        //upCommand.SetHandler(CreateDbAsync);

        //var testCommand = new Command("test", "");
        //testCommand.AddOption(_databaseConnectionOption);
        //testCommand.AddOption(_testSelectorOption);
        //testCommand.SetHandler(TestAsync);

        //var databaseCommand = new Command("database", "");
        //databaseCommand.AddCommand(upCommand);
        //databaseCommand.AddCommand(dropCommand);
        //databaseCommand.AddCommand(testCommand);
        //databaseCommand.AddGlobalOption(_secretsRepositoryOption);
        //databaseCommand.AddGlobalOption(_serverConnectionOption);


        //var workspaceUpgradeCommand = new Command("upgrade", "");
        //workspaceUpgradeCommand.SetHandler(UpgradeDatabricksWorkspaceAsync);
        //workspaceUpgradeCommand.AddOption(_timeoutOption);
        //var workspaceCommand = new Command("databricks", "");
        //workspaceCommand.AddAlias("spark");
        //workspaceCommand.AddAlias("workspace");
        //workspaceCommand.AddGlobalOption(_secretsRepositoryOption);
        //workspaceCommand.AddCommand(workspaceUpgradeCommand);



        var root = new RootCommand() {  };
        return await root.InvokeAsync(args);
    }





    [DebuggerNonUserCode]
    static async Task<int> Main(string[] args)
    {
        var regex = new Regex(@"%([^%]+?)%");
        var notRegistered = args
            .Select(arg => regex.Match(arg))
            .Where(match => match.Success)
            .Select(match => match.Result("$1"))
            .ToHashSet(StringComparer.Ordinal);

        if (notRegistered.Any())
        {
            var csv = notRegistered.Join(",");
            var msg = notRegistered.Count > 1
                ? @$"Environment variables '{csv}' are not defined."
                : @$"Environment variable '{csv}' is not defined.";
            ConsoleColor.Red.AsForegroundColor(() => Console.WriteLine(msg));
            return 1;
        }

        try
        {
            return await new Program().RunAsync(args);
        }
        catch (Exception ex)
        {
            ConsoleColor.Red.AsForegroundColor(() => Console.WriteLine(ex.Message));
            return 1;
        }
    }
}

//UsingSettingsGroupExample.Run();
//await UsingDefaultConsoleLoggerExample.RunAsync();
//await UsingCustomConsoleLoggerExample.RunAsync();
//await UsingCustomLoggerExample.RunAsync();


//await UsingHttpServerHandler.RunAsync();



//var client = new PgHttpClient(
//    "host=localhost;database=webappdb;port=5430;username=postgres;password=postgres");
//var response = await client.GetAsync("");


//Console.WriteLine(response.StatusCode);
//response = await client.PostAsync("/actions?v=1.0", new StringContent("{}"));

//Console.WriteLine(response.StatusCode);