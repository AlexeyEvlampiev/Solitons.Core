using System.Data.Common;
using System.Diagnostics;
using Npgsql;
using Solitons;
using Solitons.Data.Common.Postgres;
using Solitons.Data.Management.Postgres.Common;
using Solitons.Security;

namespace SampleSoft.SkyNet.Azure.Postgres;

public abstract class NpgsqlManager : PgManager
{
    private readonly ISecretsRepository _secrets;

    [DebuggerStepThrough]
    protected NpgsqlManager(
        string connectionString,
        ISecretsRepository secrets,
        PgManagerConfig config)
        : base(connectionString, config)
    {
        _secrets = secrets;
    }


    [DebuggerStepThrough]
    protected sealed override Task SaveSecretAsync(
        string secretKey,
        string secretValue,
        CancellationToken cancellation) => _secrets.SetSecretAsync(secretKey, secretValue);

    protected sealed override string ConstructConnectionString(
        string template,
        DatabaseConnectionOptions options)
    {
        var builder = new NpgsqlConnectionStringBuilder(template);
        builder.Database = options.Database.DefaultIfNullOrWhiteSpace(builder.Database ?? "");
        builder.Username = options.Username.DefaultIfNullOrWhiteSpace(builder.Username ?? "");
        builder.Password = options.Password.DefaultIfNullOrWhiteSpace(builder.Password ?? "");
        return builder.ConnectionString;
    }



    protected sealed override PgConnectionInfo ExtractConnectionInfo(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        return new PgConnectionInfo(
            builder.Host.DefaultIfNullOrWhiteSpace("localhost"),
            builder.Port,
            builder.Database.DefaultIfNullOrWhiteSpace("postgres"),
            builder.Username.DefaultIfNullOrWhiteSpace("postgres"));
    }


    [DebuggerNonUserCode]
    protected sealed override DbConnection CreateDbConnection(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Timeout = 30,
            CommandTimeout = 600
        };
        return new NpgsqlConnection(builder.ConnectionString);
    }


    [DebuggerStepThrough]
    protected sealed override Task<string?> GetSecretIfExistsAsync(string secretKey, CancellationToken _) => _secrets.GetSecretIfExistsAsync(secretKey);


    protected override async Task OnUpgradingAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        var connectionString = await GetSecretIfExistsAsync(
            KeyVaultSecretNames.SkyNetDbAdminConnectionString, 
            cancellation);

        if (connectionString.IsNullOrWhiteSpace())
        {
            throw new InvalidOperationException();
        }

        await using var connection = new NpgsqlConnection(connectionString);
        await using var command = connection.CreateCommand();
        command.CommandText = @"
        CREATE TABLE IF NOT EXISTS system.migration_log
        (
            sequence_number BIGINT GENERATED ALWAYS AS IDENTITY,           
            script_id text NOT NULL PRIMARY KEY,
            created_utc TIMESTAMP NOT NULL DEFAULT(now())
        );";
        await connection.OpenAsync(cancellation);
        await command.ExecuteNonQueryAsync(cancellation);
    }
}