using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using Npgsql;
using SampleSoft.SkyNet.Azure;
using SampleSoft.SkyNet.Azure.Postgres;
using Solitons;
using Solitons.Data.Management;
using Solitons.Data.Management.Postgres;
using Solitons.Data.Management.Postgres.Common;
using Solitons.Diagnostics;
using Solitons.Security;

namespace SampleSoft.SkyNet.Control.SkyNetDb;

public sealed class SkyNetDbManager : NpgsqlManager
{
    private readonly ISecretsRepository _secrets;

    [DebuggerStepThrough]
    private SkyNetDbManager(
        string connectionString,
        ISecretsRepository secrets,
        SkyNetDbManagerConfig config) 
        : base(connectionString, secrets, config, new SkyNetDbScriptPriorityComparer())
    {
        _secrets = secrets;
    }

    [DebuggerStepThrough]
    public static async Task<IPgManager> CreateAsync(
        string connectionString,
        ISecretsRepository secrets,
        SkyNetDbManagerConfig config)
    {
        await using (var connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();
            var authorized = await PgManager.IsAuthorizedAsync(connection);
            if (authorized == false)
            {
                throw new InvalidOperationException(
                    $"The user '{connection.UserName}' lacks necessary permissions to manage database deployments and roles. Please ensure the user has the appropriate permissions, or use a different user that has the necessary privileges.");
            }
        }
        
        return new SkyNetDbManager(connectionString, secrets, config);
    }

    [DebuggerStepThrough]
    public static async Task<IPgManager> CreateAsync(ISecretsRepository secrets)
    {
        var connectionString = await secrets.GetSecretAsync(KeyVaultSecretNames.SkyNetPgServerConnectionString);
        return new SkyNetDbManager(connectionString, secrets, new SkyNetDbManagerConfig());
    }

    [DebuggerStepThrough]
    public static async Task<IPgManager> CreateAsync(ISecretsRepository secrets, SkyNetDbManagerConfig config)
    {
        var connectionString = await secrets.GetSecretAsync(KeyVaultSecretNames.SkyNetPgServerConnectionString);
        return new SkyNetDbManager(connectionString, secrets, config);
    }

    [DebuggerStepThrough]
    public static IPgManager Create(
        string connectionString,
        ISecretsRepository secrets)
    {
        return new SkyNetDbManager(connectionString, secrets, new SkyNetDbManagerConfig());
    }

    protected override async Task CreateDbIfNotExistsAsync(CancellationToken cancellation)
    {
        await base.CreateDbIfNotExistsAsync(cancellation);
        await using var connection = new NpgsqlConnection(
            new NpgsqlConnectionStringBuilder(ConnectionString)
            {
                Database = Database
            }.ConnectionString);
        await using var command = connection.CreateCommand();
        await connection.OpenAsync(cancellation);
        command.CommandText = @"
                CREATE SCHEMA IF NOT EXISTS system AUTHORIZATION skynetdb_admin;
                CREATE SCHEMA IF NOT EXISTS api AUTHORIZATION skynetdb_admin;
                CREATE SCHEMA IF NOT EXISTS data AUTHORIZATION skynetdb_admin;
                CREATE SCHEMA IF NOT EXISTS extensions AUTHORIZATION skynetdb_admin;
                CREATE EXTENSION IF NOT EXISTS hstore WITH SCHEMA extensions;
                CREATE EXTENSION IF NOT EXISTS pgcrypto WITH SCHEMA extensions;
                CREATE EXTENSION IF NOT EXISTS bloom WITH SCHEMA extensions;
                CREATE EXTENSION IF NOT EXISTS pg_trgm WITH SCHEMA extensions;

                ALTER DATABASE skynetdb SET search_path = data, extensions, public;
                ALTER DATABASE skynetdb OWNER TO skynetdb_admin;

                GRANT skynetdb_api TO skynetdb_admin WITH ADMIN OPTION;";

        await command.ExecuteNonQueryAsync(cancellation);
    }

    protected override async Task<bool> ExecuteScriptIfApplicableAsync(DbConnection connection, Script script, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        await using var command = connection.CreateCommand();
        command.CommandText = $@"
            SELECT sequence_number, created_utc
            FROM system.migration_log 
            WHERE script_id = '{script.Id}'";
        await using (var reader = await command.ExecuteReaderAsync(
                         CommandBehavior.SingleRow, 
                         cancellation))
        {
            if (await reader.ReadAsync(cancellation))
            {
                var sequenceNumber = reader.GetInt64("sequence_number");
                var deployedUtc = reader.GetDateTime("created_utc");
                Console.WriteLine($"{script} deployed on {deployedUtc} (sn: {sequenceNumber});");
                return false;
            }
        }


        Console.WriteLine($"Executing {script.Id}");

        command.CommandText = await script.LoadAsync(cancellation);
        if (command.CommandText.IsNullOrWhiteSpace())
        {
            Trace.TraceWarning($"{script.Path} is empty.");
            return false;
        }

        await command.ExecuteNonQueryAsync(cancellation);

        if (SkyNetDbScriptPriorityComparer.IsMigrationScript(script))
        {
            command.CommandText = @$"INSERT INTO system.migration_log(script_id) VALUES('{script.Id}')";
            await command.ExecuteNonQueryAsync(cancellation);
        }

        return true;
    }

    protected override async Task<DbTransaction> BeginUpgradeTransactionAsync(CancellationToken cancellation)
    {
        var connectionString = await GetSecretIfExistsAsync(
            KeyVaultSecretNames.SkyNetDbAdminConnectionString, 
            cancellation);

        if (connectionString.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException();
        }

        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Timeout = 30,
            CommandTimeout = (int)TimeSpan.FromMinutes(30).TotalSeconds
        };

        connectionString = builder.ConnectionString;
        var connection = new NpgsqlConnection(connectionString);
        try
        {
            await connection.OpenAsync(cancellation);
            return await connection.BeginTransactionAsync(cancellation);
        }
        catch (Exception e)
        {
            await connection.DisposeAsync();
            throw;
        }
    }


    protected override IEnumerable<Script> GetUpgradeScripts()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var scripts = assembly
            .GetManifestResourceNames()
            .Where(SkyNetDbEmbeddedScript.IsScript)
            .Select(path => SkyNetDbEmbeddedScript.Create(path, assembly))
            .ToArray();
        return scripts;
    }


    [DebuggerStepThrough]
    protected override Task PerformPostUpgradeTestsAsync(CancellationToken cancellation) => IntegrationTest
        .RunAllAsync(
            GetType().Assembly, 
            _secrets.ReadThroughCache(Observable.Empty<Unit>()), 
            cancellation: cancellation);
}