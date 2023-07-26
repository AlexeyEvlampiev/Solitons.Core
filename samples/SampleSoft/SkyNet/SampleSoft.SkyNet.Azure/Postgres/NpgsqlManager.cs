using System.Data.Common;
using System.Diagnostics;
using Npgsql;
using Solitons;
using Solitons.Data.Common.Postgres;
using Solitons.Data.Management.Postgres.Common;
using Solitons.Security;

namespace SampleSoft.SkyNet.Azure.Postgres;

/// <summary>
/// An abstract base class that provides the core functionality for managing Postgres databases.
/// This includes tasks such as establishing connections, constructing connection strings, and performing database upgrades.
/// It uses a secrets repository to manage sensitive information.
/// </summary>
/// <remarks>
/// Specific functionality and behavior should be provided by subclasses, making this a flexible base for various types of Postgres database managers.
/// </remarks>
public abstract class NpgsqlManager : PgManager
{
    private readonly ISecretsRepository _secrets;

    /// <summary>
    /// Initializes a new instance of the <see cref="NpgsqlManager"/> class with the specified connection string, secrets repository and configuration.
    /// </summary>
    /// <param name="connectionString">The connection string for the database to be managed.</param>
    /// <param name="secrets">The repository where secrets are stored.</param>
    /// <param name="config">The configuration settings for the Postgres manager.</param>
    /// <param name="scriptPriorityComparer">Determines the order in which the upgrade scripts should be executed.</param>
    [DebuggerStepThrough]
    protected NpgsqlManager(
        string connectionString,
        ISecretsRepository secrets,
        PgManagerConfig config, 
        ScriptPriorityComparer scriptPriorityComparer)
        : base(connectionString, config, scriptPriorityComparer)
    {
        _secrets = secrets;
    }

    /// <summary>
    /// Saves a secret asynchronously in the secrets repository.
    /// </summary>
    /// <param name="secretKey">The key of the secret to be saved.</param>
    /// <param name="secretValue">The value of the secret to be saved.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A Task representing the operation.</returns>
    [DebuggerStepThrough]
    protected sealed override Task SaveSecretAsync(
        string secretKey,
        string secretValue,
        CancellationToken cancellation) => _secrets.SetSecretAsync(secretKey, secretValue, cancellation);


    /// <summary>
    /// Constructs a connection string using the provided template and database connection options.
    /// </summary>
    /// <param name="template">The template connection string.</param>
    /// <param name="options">The options for the database connection.</param>
    /// <returns>A constructed connection string.</returns>
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


    /// <summary>
    /// Extracts the connection information from the provided connection string.
    /// </summary>
    /// <param name="connectionString">The connection string from which to extract the information.</param>
    /// <returns>The extracted Postgres connection information.</returns>
    protected sealed override PgConnectionInfo ExtractConnectionInfo(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        return new PgConnectionInfo(
            builder.Host.DefaultIfNullOrWhiteSpace("localhost"),
            builder.Port,
            builder.Database.DefaultIfNullOrWhiteSpace("postgres"),
            builder.Username.DefaultIfNullOrWhiteSpace("postgres"));
    }


    /// <summary>
    /// Creates a new database connection.
    /// </summary>
    /// <param name="connectionString">The connection string for the database connection.</param>
    /// <returns>A new DbConnection object.</returns>
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

    /// <summary>
    /// Gets the value of a secret if it exists in the secrets repository.
    /// </summary>
    /// <param name="secretKey">The key of the secret.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The value of the secret if it exists, null otherwise.</returns>
    [DebuggerStepThrough]
    protected sealed override Task<string?> GetSecretIfExistsAsync(string secretKey, CancellationToken cancellation) => _secrets
        .GetSecretIfExistsAsync(secretKey, cancellation);

    /// <summary>
    /// Performs tasks required before a database upgrade.
    /// </summary>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A Task representing the operation.</returns>
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