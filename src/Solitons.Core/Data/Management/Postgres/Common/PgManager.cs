using Solitons.Reactive;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Management.Postgres.Common;

/// <summary>
/// Provides base functionality for managing a Postgres database.
/// </summary>
public abstract partial class PgManager : IPgManager
{
    private readonly PgManagerConfig _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="PgManager"/> class.
    /// </summary>
    /// <param name="connectionString">The maintenance database connection string for the Postgres database.</param>
    /// <param name="config">The configuration for the Postgres database.</param>
    protected PgManager(
        string connectionString,
        PgManagerConfig config)
    {
        ConnectionString = connectionString;
        _config = config;
    }

    /// <summary>
    /// Gets the maintenance database connection string.
    /// </summary>
    protected string ConnectionString { get; }

    /// <summary>
    /// Gets the name of the target Postgres database.
    /// </summary>
    public string Database => _config.DatabaseName;

    /// <summary>
    /// Creates a new database if one with the specified name does not already exist.
    /// </summary>
    /// <param name="cancellation">The cancellation token to use.</param>
    private async Task CreateDbAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        var maintenanceDbInfo = ExtractConnectionInfo(ConnectionString);

        Debug.WriteLine($"Creating {Database} ...");
        await CreateDbIfNotExistsAsync(cancellation);

        foreach (var roleSecretKey in _config.RoleConnectionSecretKeys)
        {
            var (roleName, secretName) = (roleSecretKey.Key, roleSecretKey.Value);
            Debug.WriteLine($"Creating {roleName} role with login ...");
            await RegisterLoginIfNotExistsAsync(
                maintenanceDbInfo, 
                roleName, 
                secretName, 
                cancellation);
        }

        Debug.WriteLine($"Upgrading the {Database} database ...");
        await UpgradeAsync(cancellation);
    }

    [DebuggerStepThrough]
    protected virtual Task OnUpgradingAsync(CancellationToken cancellation) => Task.CompletedTask;

    protected virtual async Task<bool> RegisterLoginIfNotExistsAsync(
        PgConnectionInfo maintenanceDbConnectionParams,
        string username, 
        string secretName,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var requiredConnectionParams = maintenanceDbConnectionParams with
        {
            Database = Database,
            Username = username
        };

        var newSecretGenerated = await GetSecret(secretName, cancellation)
            .Do(_ => Debug.WriteLine($"{secretName} secret found."))
            .Where(secretValue =>
            {
                try
                {
                    using (CreateDbConnection(secretValue))
                    {
                        return requiredConnectionParams
                            .Equals(ExtractConnectionInfo(secretValue));
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceWarning(e.Message);
                    return false;
                }
            })
            .Do(_ => Debug.WriteLine($"{secretName} secret is valid."))
            .WhenAnyIs(false)
            .Do(_ => Debug.WriteLine($"Generating new {secretName} secret."))
            .SelectMany(async generateNewSecretTrigger =>
            {
                var password = _config.SharedPassword
                    .DefaultIfNullOrWhiteSpace(GenRandomPassword())
                    .DefaultIfNullOrWhiteSpace(Guid
                        .NewGuid()
                        .ToString("N"))
                    .Trim();
                await using var connection = CreateDbConnection(ConnectionString);
                await connection.OpenAsync(cancellation);
                await using var transaction = await connection.BeginTransactionAsync(cancellation);
                await connection.DoAsync(async cmd =>
                {
                    cmd.CommandText = $@"
                        DO
                        $DO$
                        BEGIN
                            IF EXISTS(SELECT 1 FROM pg_catalog.pg_roles WHERE  rolname = '{username}') THEN
                                ALTER ROLE {username} WITH LOGIN PASSWORD $${password}$$;
                            ELSE
                                CREATE ROLE {username} WITH LOGIN PASSWORD $${password}$$;
                            END IF;
                        END;
                        $DO$;

                        GRANT {username} TO current_user;
                        GRANT CONNECT ON DATABASE {_config.DatabaseName} TO {username}";
                    await cmd.ExecuteNonQueryAsync(cancellation);
                });
                var secretValue = ConstructConnectionString(
                    ConnectionString,
                    new DatabaseConnectionOptions
                    {
                        Database = _config.DatabaseName,
                        Username = username,
                        Password = password
                    });
                await SaveSecretAsync(secretName, secretValue, cancellation);
                await transaction.CommitAsync(CancellationToken.None);
                return Unit.Default;
            })
            .WithRetryPolicy(args => CreateRoleRetryPolicy(args, cancellation))
            .Any();
        return newSecretGenerated;
    }

    protected virtual async Task CreateDbIfNotExistsAsync(CancellationToken cancellation)
    {
        await Observable
            .FromAsync(async () =>
            {
                await using var connection = CreateDbConnection(ConnectionString);
                await connection.OpenAsync(cancellation);
                await connection.DoAsync(async cmd =>
                {
                    cmd.CommandText =
                        $@"SELECT EXISTS(SELECT 1 FROM pg_database WHERE datname = '{Database}')";
                    if (!true.Equals(await cmd.ExecuteScalarAsync(cancellation)))
                    {
                        cmd.CommandText = $@"CREATE DATABASE {Database};";
                        await cmd.ExecuteNonQueryAsync(cancellation);
                    }
                });
            });
    }


    /// <summary>
    /// Defines the retry policy for creating a database role.
    /// </summary>
    /// <param name="args">The arguments for the retry policy, including information about the current exception and attempt number.</param>
    /// <param name="cancellation">A token that can be used to request the operation to be cancelled.</param>
    /// <returns>An observable stream of <see cref="RetryPolicyArgs"/> that signals the next attempt should be made if the exception is transient and the attempt number is less than 10.</returns>
    /// <remarks>
    /// This method will delay the next attempt by a number of milliseconds equal to the current attempt number times 300, up to a maximum of 3000 milliseconds (3 seconds).
    /// </remarks>
    protected virtual IObservable<RetryPolicyArgs> CreateRoleRetryPolicy(
        RetryPolicyArgs args, 
        CancellationToken cancellation)
    {
        return args
            .SignalNextAttempt(args is { Exception: DbException { IsTransient: true }, AttemptNumber: < 10 })
            .Delay(300 * args.AttemptNumber.Max(10));
    }

    /// <summary>
    /// Defines the retry policy for testing a database connection.
    /// </summary>
    /// <param name="args">The arguments for the retry policy, including information about the current exception and attempt number.</param>
    /// <param name="cancellation">A token that can be used to request the operation to be cancelled.</param>
    /// <returns>An observable stream of <see cref="RetryPolicyArgs"/> that signals the next attempt should be made if the exception is transient and the attempt number is less than 10.</returns>
    /// <remarks>
    /// This method will delay the next attempt by a number of milliseconds equal to the current attempt number times 100, up to a maximum of 1000 milliseconds (1 second).
    /// </remarks>
    protected virtual IObservable<RetryPolicyArgs> ConnectionTestRetryPolicy(
        RetryPolicyArgs args,
        CancellationToken cancellation)
    {
        return args
            .SignalNextAttempt(args is
            {
                Exception: DbException { IsTransient: true },
                AttemptNumber: < 10
            })
            .Delay(100 * args.AttemptNumber.Max(10), cancellation);
    }

    /// <summary>
    /// Drops the Postgres database with the specified name.
    /// </summary>
    /// <param name="cancellation">The cancellation token to use.</param>
    protected virtual async Task DropDbAsync(CancellationToken cancellation)
    {
        await using var connection = CreateDbConnection(ConnectionString);
        await using var command = connection.CreateCommand();
        await connection.OpenAsync(cancellation);

        command.CommandText = $@"
                SELECT *, pg_terminate_backend(pid)
                FROM pg_stat_activity 
                WHERE pid <> pg_backend_pid()
                AND datname = '{Database}';";
        await command.ExecuteNonQueryAsync(cancellation);

        command.CommandText = $@"DROP DATABASE IF EXISTS {Database} WITH (FORCE);";
        await command.ExecuteNonQueryAsync(cancellation);
    }



    /// <summary>
    /// Saves the specified secret value with the specified key.
    /// </summary>
    /// <param name="secretKey">The key for the secret value.</param>
    /// <param name="secretValue">The secret value to save.</param>
    /// <param name="cancellation">The cancellation token to use.</param>
    protected abstract Task SaveSecretAsync(string secretKey, string secretValue, CancellationToken cancellation);



    protected virtual Task RunTestAsync(CancellationToken cancellation) => Task.CompletedTask;

    /// <summary>
    /// Generates a random password.
    /// </summary>
    /// <returns>A random password.</returns>
    protected virtual string GenRandomPassword() => Guid.NewGuid().ToString("N");

    /// <summary>
    /// Handles the case when an invalid connection string is encountered.
    /// </summary>
    /// <param name="error">The error message.</param>
    /// <param name="role">The role associated with the connection string.</param>
    /// <param name="cancellation">The cancellation token to use.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected virtual Task OnInvalidConnectionStringAsync(
        string error,
        string role,
        CancellationToken cancellation)
    {
        throw new InvalidOperationException(error);
    }

    /// <summary>
    /// Gets the connection string for the specified role if it exists.
    /// </summary>
    /// <param name="secretKey">The secret key.</param>
    /// <param name="cancellation">The cancellation token to use.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected abstract Task<string?> GetSecretIfExistsAsync(
        string secretKey, 
        CancellationToken cancellation);

    protected IObservable<string> GetSecret(
        string secretKey,
        CancellationToken cancellation) => Observable
        .FromAsync(() => GetSecretIfExistsAsync(secretKey, cancellation))
        .Where(_ => _.IsPrintable())
        .Select(_ => _!);

    /// <summary>
    /// Defines the retry policy for creating a database.
    /// </summary>
    /// <param name="args">The arguments for the retry policy, including information about the current exception and elapsed time since the first exception.</param>
    /// <param name="cancellation">A token that can be used to request the operation to be cancelled.</param>
    /// <returns>An observable stream of <see cref="RetryPolicyArgs"/> that signals the next attempt should be made if the exception is transient and the elapsed time since the first exception is less than 5 minutes.</returns>
    /// <remarks>
    /// This method will delay the next attempt by a number of milliseconds equal to the current attempt number times 1000, up to a maximum of 180,000 milliseconds (3 minutes).
    /// </remarks>
    protected virtual IObservable<RetryPolicyArgs> CreateDbRetryPolicy(
        RetryPolicyArgs args,
        CancellationToken cancellation)
    {
        return args
            .SignalNextAttempt(
                args is { Exception: DbException { IsTransient: true } } &&
                args.ElapsedTimeSinceFirstException < TimeSpan.FromMinutes(5))
            .Delay(1000 * args.AttemptNumber.Max(180), cancellation);
    }



    [DebuggerStepThrough]
    Task IPgManager.CreateDbAsync(CancellationToken cancellation) => Observable
        .FromAsync([DebuggerStepThrough] () => this.CreateDbAsync(cancellation))
        .WithRetryPolicy(args => CreateDbRetryPolicy(args, cancellation))
        .ToTask(cancellation);



    [DebuggerStepThrough]
    Task IPgManager.DropDbAsync(CancellationToken cancellation) =>
        Observable
            .FromAsync([DebuggerStepThrough] () => this.DropDbAsync(cancellation))
            .WithRetryPolicy(args => DropDbRetryPolicy(args, cancellation))
            .ToTask(cancellation);

    [DebuggerStepThrough]
    async Task IPgManager.RunTestAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        await Observable
            .FromAsync(()=>RunTestAsync(cancellation))
            .WithRetryPolicy(args => TestRetryPolicy(args, cancellation))
            .ToTask(cancellation);
    }



    /// <summary>
    /// Defines the retry policy for dropping a database.
    /// </summary>
    /// <param name="arg">The arguments for the retry policy, including information about the current exception and attempt number.</param>
    /// <param name="cancellation">A token that can be used to request the operation to be cancelled.</param>
    /// <returns>An observable stream of <see cref="RetryPolicyArgs"/> that signals the next attempt should be made if the exception is transient and the attempt number is less than 10.</returns>
    /// <remarks>
    /// This method will delay the next attempt by a number of seconds equal to the current attempt number, up to a maximum of 10 seconds.
    /// </remarks>
    protected virtual IObservable<RetryPolicyArgs> DropDbRetryPolicy(RetryPolicyArgs arg, CancellationToken cancellation)
    {
        return arg
            .SignalNextAttempt(arg is
            {
                Exception: DbException { IsTransient: true },
                AttemptNumber: < 10
            })
            .Delay(1000 * arg.AttemptNumber.Max(10));
    }

    protected virtual IObservable<RetryPolicyArgs> TestRetryPolicy(
        RetryPolicyArgs arg, 
        CancellationToken cancellation)
    {
        return arg
            .SignalNextAttempt(arg is
            {
                Exception: DbException { IsTransient: true },
                AttemptNumber: < 10
            })
            .Delay(1000 * arg.AttemptNumber.Max(10));
    }

    /// <summary>
    /// Creates a new <see cref="DbConnection"/> object with the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to use for the new <see cref="DbConnection"/> object.</param>
    /// <returns>A new <see cref="DbConnection"/> object.</returns>
    protected abstract DbConnection CreateDbConnection(string connectionString);

    /// <summary>
    /// Extracts the connection information from the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string from which to extract the connection information.</param>
    /// <returns>The extracted connection information.</returns>
    protected abstract PgConnectionInfo ExtractConnectionInfo(string connectionString);

    /// <summary>
    /// Constructs a connection string using the specified template and options.
    /// </summary>
    /// <param name="template">A string representing the connection string template with placeholders for connection parameters.</param>
    /// <param name="options">A <see cref="DatabaseConnectionOptions"/> object representing the set of connection options to use when constructing the connection string.</param>
    /// <returns>A string representing the constructed connection string.</returns>
    protected abstract string ConstructConnectionString(string template, DatabaseConnectionOptions options);

    /// <summary>
    /// Represents a set of options for establishing a connection to a database.
    /// </summary>
    protected sealed record DatabaseConnectionOptions
    {
        /// <summary>
        /// Gets or sets the name of the database to connect to.
        /// </summary>
        public string? Database { get; init; }

        /// <summary>
        /// Gets or sets the username to use for authentication.
        /// </summary>
        public string? Username { get; init; }

        /// <summary>
        /// Gets or sets the password to use for authentication.
        /// </summary>
        public string? Password { get; init; }
    }

    /// <summary>
    /// Asynchronously checks whether the current Postgres user is authorized to create databases and roles.
    /// </summary>
    /// <param name="connection">The active database connection to use for the check.</param>
    /// <param name="cancellation">Optional cancellation token that can be used to cancel the operation. Defaults to none.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean 
    /// indicating whether the current user is authorized to create databases and roles.</returns>
    /// <exception cref="Exception">Throws an exception if the query operation fails.</exception>
    public static async Task<bool> IsAuthorizedAsync(
        DbConnection connection, 
        CancellationToken cancellation = default)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = @"
        SELECT EXISTS(SELECT rolcreatedb AND rolcreaterole 
        FROM pg_roles 
        WHERE rolname = CURRENT_USER);";
        var response = await command.ExecuteScalarAsync(cancellation);
        return (true.Equals(response));
    }


    private async Task UpgradeAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        await OnUpgradingAsync(cancellation);

        await using var transaction = await BeginUpgradeTransactionAsync(cancellation);
        var connection = ThrowIf.NullReference(transaction.Connection);

        foreach (var script in GetUpgradeScripts())
        {
            Debug.WriteLine($"Script: {script}");
            await ExecuteIfShouldAsync(connection, script, cancellation);
        }

        await OnUpgradedAsync(transaction, cancellation);
    }

    protected virtual Task OnUpgradedAsync(
        DbTransaction transaction, 
        CancellationToken cancellation)=> transaction.CommitAsync(cancellation);

    protected abstract Task<bool> ExecuteIfShouldAsync(DbConnection connection, Script script, CancellationToken cancellation);


    protected abstract Task<DbTransaction> BeginUpgradeTransactionAsync(CancellationToken cancellation);

    protected abstract IEnumerable<Script> GetUpgradeScripts();


}