using DbUp;
using DbUp.Helpers;
using Npgsql;
using Solitons.Data.Management.Postgres;
using Solitons.Data.Management.Postgres.Common;
using Solitons.Examples.WebApp.Azure;
using Solitons.Reactive;
using Solitons.Security;
using System.Data.Common;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Solitons.Examples.WebApp.Data;

public sealed class WebAppDbManager : PgManager
{
    private readonly ISecretsRepository _secrets;
    private readonly PgManagerConfig _config;

    private WebAppDbManager(
        string connectionString,
        ISecretsRepository secrets,
        PgManagerConfig config) : base(connectionString, config)
    {
        _secrets = secrets;
        _config = config;
    }

    private WebAppDbManager(
        string connectionString,
        ISecretsRepository secrets)
        : this(connectionString, secrets, new WebAppDbManageConfig())
    {

    }

    [DebuggerNonUserCode]
    public static IPgManager Create(
        string connectionString,
        ISecretsRepository repository)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Timeout = 300,
            CommandTimeout = 300
        };

        return new WebAppDbManager(connectionString, repository);
    }

    [DebuggerStepThrough]
    protected override Task SaveSecretAsync(
        string secretKey, 
        string secretValue, 
        CancellationToken cancellation) => _secrets.SetSecretAsync(secretKey, secretValue);


    protected override string ConstructConnectionString(
        string template, 
        DatabaseConnectionOptions options)
    {
        var builder = new NpgsqlConnectionStringBuilder(template);
        builder.Database = options.Database.DefaultIfNullOrWhiteSpace(builder.Database ?? "");
        builder.Username = options.Username.DefaultIfNullOrWhiteSpace(builder.Username ?? "");
        builder.Password = options.Password.DefaultIfNullOrWhiteSpace(builder.Password ?? "");
        return builder.ConnectionString;
    }


    [DebuggerStepThrough]
    protected override Task<string?> GetSecretIfExistsAsync(string secretKey, CancellationToken _) => _secrets.GetSecretIfExistsAsync(secretKey);


 

    protected override PgConnectionInfo ExtractConnectionInfo(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        return new PgConnectionInfo(
            builder.Host.DefaultIfNullOrWhiteSpace("localhost"), 
            builder.Port, 
            builder.Database.DefaultIfNullOrWhiteSpace("postgres"), 
            builder.Username.DefaultIfNullOrWhiteSpace("postgres"));
    }

    [DebuggerNonUserCode]
    protected override DbConnection CreateDbConnection(string connectionString) => new NpgsqlConnection(connectionString);


    protected override async Task CreateDbAsync(CancellationToken cancellation)
    {
        await base.CreateDbAsync(cancellation);
        await FluentObservable
            .Defer(async () =>
            {
                await using var connection = new NpgsqlConnection(
                    new NpgsqlConnectionStringBuilder(ConnectionString)
                    {
                        Database = Database
                    }.ConnectionString);
                await using var command = connection.CreateCommand();
                await connection.OpenAsync(cancellation);
                command.CommandText = @"
                CREATE SCHEMA IF NOT EXISTS system AUTHORIZATION webappdb_admin;
                ALTER SCHEMA system OWNER TO webappdb_admin;

                CREATE SCHEMA IF NOT EXISTS data AUTHORIZATION webappdb_admin;
                ALTER SCHEMA data OWNER TO webappdb_admin;

                CREATE SCHEMA IF NOT EXISTS api AUTHORIZATION webappdb_admin;
                ALTER SCHEMA api OWNER TO webappdb_admin;

                CREATE EXTENSION IF NOT EXISTS hstore WITH SCHEMA system;
                CREATE EXTENSION IF NOT EXISTS pgcrypto WITH SCHEMA system;
                CREATE EXTENSION IF NOT EXISTS pg_trgm WITH SCHEMA system;

                ALTER DATABASE webappdb SET search_path = data, api, system, public;
                ";
                await command.ExecuteNonQueryAsync(cancellation);
            })
            .RetryWhen(ShouldRetry(cancellation));
        await FluentObservable
            .Defer(async () =>
            {
                await using var connection = new NpgsqlConnection(ConnectionString);
                await using var command = connection.CreateCommand();
                await connection.OpenAsync(cancellation);
                command.CommandText = @"
                ALTER DATABASE webappdb OWNER TO webappdb_admin;
                ALTER DATABASE webappdb SET search_path = system, data, api, public;
                ";
                await command.ExecuteNonQueryAsync(cancellation);
            })
            .RetryWhen(ShouldRetry(cancellation));
        


        var connectionString = await _secrets.GetSecretIfExistsAsync(
            SecretKeys.DatabaseAdminConnectionString);
        

        var migration = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(
                Assembly.GetExecutingAssembly(),
                IsMigrationScript)
            .JournalToPostgresqlTable("system", "schemaversion")
            .LogToConsole()
            .Build();

        var setup = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithVariablesDisabled()
            .WithScriptsEmbeddedInAssembly(
                Assembly.GetExecutingAssembly(),
                IsSetupScript)
            .JournalTo(new NullJournal())
            .LogToConsole()
            .Build();

        var migrationResult = migration.PerformUpgrade();
        if (migrationResult.Successful == false)
        {
            throw new Exception(
                $"Migration failed. {migrationResult.Error.Message}");
        }

        var setupResult = setup.PerformUpgrade();
        if (setupResult.Successful == false)
        {
            throw new Exception(
                $"Setup failed. {setupResult.Error.Message}");
        }

        bool IsMigrationScript(string path)
        {
            var isMatch = Regex.IsMatch(path, @"(?i)\.scripts\.migrations\.[^\.]+\.sql$");
            return isMatch;
        }

        bool IsSetupScript(string path)
        {
            var isMatch = Regex.IsMatch(path, @"(?i)\.scripts\.setup\..+\.sql$");
            return isMatch;
        }
    }

}