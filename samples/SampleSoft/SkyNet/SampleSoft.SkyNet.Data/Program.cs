using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using Microsoft.Identity.Client;
using Npgsql;
using SampleSoft.SkyNet.Azure;
using SampleSoft.SkyNet.Azure.CommandLine;
using SampleSoft.SkyNet.Azure.SQLite;
using Solitons;
using Solitons.Collections;
using Solitons.Common;
using Solitons.Diagnostics;
using Solitons.IO;
using Solitons.Security;
using Prompt = Solitons.IO.Prompt;

namespace SampleSoft.SkyNet.Data;

sealed class Program : ProgramBase
{
    private readonly TimeoutOption _timeoutOption = new TimeoutOption();
    private readonly SecretsRepositoryOption _secretsRepositoryOption = new SecretsRepositoryOption();
    private readonly ServerConnectionOption _serverConnectionOption = new ServerConnectionOption();
    private readonly DatabaseConnectionOption _databaseConnectionOption = new DatabaseConnectionOption();


    private Program(string[] args) 
        : base(args)
    {
        var recreateCommand = new Command("create-or-update", "");
        recreateCommand.SetHandler(CreateOrUpdateAsync);
    }

    //[DebuggerStepThrough]
    public static Task<int> Main()
    {
        var cmd = Environment.CommandLine;
        cmd = Regex.Replace(cmd, @"^(""[^""]+""|\S+)\s+", String.Empty);
        var options = FluentDictionary.Create<string, string>();
        var valPattern = @"(?:""[^""]""|\S+)";

        cmd = Regex.Replace(cmd,
            @"(?i)--(?:server|host|data\W?source|addr(?:ess)?|network\W?address)\b", "--host");
        cmd = Regex.Replace(cmd, @$"--(?<key>host|connection|port|username|password|secrets)\s+(?<value>{valPattern})", match =>
        {
            var key = match.Groups["key"].Value;
            var value = match.Groups["value"].Value;
            if (options.ContainsKey(key))
            {
                throw new NotImplementedException();
            }
            options.Add(key, value);
            return string.Empty;
        });

        if (options.TryGetValue("secrets", out var secretsId))
        {

        }

        var connection = new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",
            Port = 5432,
            Username = "postgres",
            Password = "postgres"
        };
        if (options.TryGetValue("connection", out var connectionString))
        {
            connection.ConnectionString = connectionString;
        }

        if (options.TryGetValue("host", out var host))
        {
            connection.Host = host;
        }

        if (options.TryGetValue("port", out var port))
        {
            if (int.TryParse(port, out var value))
            {
                connection.Port = value;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        if (options.TryGetValue("username", out var username))
        {
            connection.Username = username;
        }

        if (options.TryGetValue("password", out var password))
        {
            connection.Password = password;
        }

        cmd = cmd + $" --connection={connection}";
        Console.WriteLine(cmd);
        return Task.FromResult(0);
    }

    public override async Task<int> RunAsync(CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        var traceFileName = $"trace-{CreatedAt.UtcDateTime.Ticks}.txt";
        using var trace = TraceManager.AttachTextFileListener(traceFileName);




        return 0;
    }


    async Task<int> CreateOrUpdateAsync(InvocationContext context)
    {
        var timeoutCancellation = context.ParseResult.GetValueForOption(_timeoutOption);
        var externalCancellation = context.GetCancellationToken();
        var cts = CancellationTokenSource
            .CreateLinkedTokenSource(externalCancellation, timeoutCancellation);
        var cancellation = cts.Token;
        var secrets = context.ParseResult.GetValueForOption(_secretsRepositoryOption)!;

        var serverConnectionString = context.ParseResult
            .GetValueForOption(_serverConnectionOption);
        if (serverConnectionString.IsNullOrWhiteSpace())
        {
            serverConnectionString = await secrets
                .GetSecretAsync(KeyVaultSecretNames.SkyNetPgServerConnectionString);
        }

        var builder = new NpgsqlConnectionStringBuilder(serverConnectionString)
        {
            Database = SkyNetDbManagerConfig.DatabaseName,
            Timeout = 300
        };
        void WriteLine(string key, object? value) => Console.WriteLine(@"{0,-12}{1}", key, value);
        WriteLine(@"Server:", builder.Host);
        WriteLine(@"Database:", SkyNetDbManagerConfig.DatabaseName);
        WriteLine(@"Username:", builder.Username);

        using (ConsoleColorScope.Create(ConsoleColor.Yellow))
        {
            var proceed = Prompt.GetYesNoAnswer(
                $"Are you sure you want to drop" +
                $" the {SkyNetDbManagerConfig.DatabaseName} database?");
            if (proceed == false)
            {
                return 0;
            }
        }
        

        //var skyNetDb = SkyNetDbManager
        //    .Create(
        //        serverConnectionString!,
        //        secrets,
        //        Array.Empty<string>());


        return 0;
    }
}