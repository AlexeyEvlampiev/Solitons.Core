using Solitons.Data.Common.Postgres;
using Solitons.Reactive;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Management.Postgres.Common;

/// <summary>
/// Provides an abstract base for managing Postgres databases, including operations for creating, dropping, and upgrading databases,
/// and handling secrets and roles. This class implements the <see cref="IPgManager"/> interface.
/// </summary>
/// <remarks>
/// Implementing classes must provide their own logic for database connection creation, extraction of connection information, 
/// construction of connection strings, execution of database upgrade scripts, managing database upgrade transactions,
/// retrieval of database upgrade scripts, and managing secret information. 
/// It also contains robust mechanisms for handling transient database errors via retry policies and leverages the Task Parallel Library 
/// and Reactive Extensions for managing asynchronous operations and event-based programs, respectively.
/// </remarks>

public abstract partial class PgManager : IPgManager
{
    private readonly PgManagerConfig _config;
    private readonly ScriptPriorityComparer _scriptPriorityComparer;

    /// <summary>
    /// Initializes a new instance of the <see cref="PgManager"/> class.
    /// </summary>
    /// <param name="connectionString">The maintenance database connection string for the Postgres database.</param>
    /// <param name="config">The configuration for the Postgres database.</param>
    /// <param name="scriptPriorityComparer">Determines the order in which the upgrade scripts should be executed.</param>
    protected PgManager(
        string connectionString,
        PgManagerConfig config,
        ScriptPriorityComparer scriptPriorityComparer)
    {
        ConnectionString = connectionString;
        _config = config;
        _scriptPriorityComparer = scriptPriorityComparer;
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
    /// Asynchronously creates a database and associated roles if they do not already exist, then upgrades the database using a series of scripts.
    /// </summary>
    /// <param name="cancellation">A CancellationToken that can be used to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method first creates the database if it does not already exist. It then iterates over the configured role keys, creating each role with 
    /// login if it does not already exist. Afterward, the method begins the upgrade process by calling the OnUpgradingAsync() method and 
    /// initializing a database transaction. 
    /// 
    /// The upgrade scripts to be executed are retrieved by calling the GetUpgradeScripts() method. Each script is then executed in turn, 
    /// assuming it should be executed based on the current state of the database.
    ///
    /// After all scripts have been executed, the method finalizes the upgrade process by calling the OnUpgradedAsync() method, which commits the transaction.
    /// </remarks>
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
        cancellation.ThrowIfCancellationRequested();
        await OnUpgradingAsync(cancellation);

        await using var transaction = await BeginUpgradeTransactionAsync(cancellation);
        var connection = ThrowIf.NullReference(transaction.Connection);

        var sortedScripts = GetUpgradeScripts()
            .OrderBy(script => script, _scriptPriorityComparer)
            .ToList();

        foreach (var script in sortedScripts)
        {
            Debug.WriteLine($"Script: {script}");
            await ExecuteScriptIfApplicableAsync(connection, script, cancellation);
        }

        await OnUpgradedAsync(transaction, cancellation);
    }



    /// <summary>
    /// Called during the upgrade process and can be overridden to implement custom upgrade logic.
    /// </summary>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that represents the completion of this method. This base implementation immediately returns a completed task.</returns>
    [DebuggerStepThrough]
    protected virtual Task OnUpgradingAsync(CancellationToken cancellation) => Task.CompletedTask;

    /// <summary>
    /// Asynchronously registers a new database login if it doesn't already exist.
    /// </summary>
    /// <remarks>
    /// This method checks the validity of the provided secret. If it is not valid or does not exist, it generates a new secret and creates a new role with the given username and new secret in the database. If the role already exists, it updates the login password of the role with the new secret. 
    ///
    /// The newly generated secret is then saved and the method returns whether a new secret was generated.
    ///
    /// Note: Exceptions during the secret validity check are caught and logged as a warning. In such cases, the method proceeds to generate a new secret.
    /// </remarks>
    /// <param name="maintenanceDbConnectionParams">The connection parameters for the database maintenance operations.</param>
    /// <param name="username">The username for the new login.</param>
    /// <param name="secretName">The name of the secret to be stored.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains a boolean indicating whether a new secret was generated.</returns>
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

    /// <summary>
    /// Asynchronously creates the database if it doesn't already exist.
    /// </summary>
    /// <remarks>
    /// This method opens a new database connection and checks if the database, as defined by the <see cref="Database"/> property, exists. If it doesn't exist, the method executes a command to create the database.
    /// 
    /// Subclasses may override this method to provide custom database creation logic. The override should call this base implementation to ensure that the database exists before executing any additional setup logic.
    ///
    /// This could be extended to create schemas, extensions, or to assign roles with specific privileges. Remember, any override should first call the base implementation to ensure the existence of the database.
    /// </remarks>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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


    /// <summary>
    /// Executes an optional set of tests against the database.
    /// </summary>
    /// <param name="cancellation">An instance of <see cref="CancellationToken"/> used to propagate notification that operations should be canceled.</param>
    /// <returns>An instance of <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method provides a hook to perform any necessary tests against the database once the upgrade process has been completed. It is empty by default, and can be overridden in a derived class to implement specific tests. Note that this method is run asynchronously, so any implemented tests should also be asynchronous. 
    /// If no tests are required, the method can be left as it is, and it will simply return a completed task when called.
    /// </remarks>
    protected virtual Task PerformPostUpgradeTestsAsync(CancellationToken cancellation) => Task.CompletedTask;

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

    /// <summary>
    /// Fetches the value of a secret, such as a password or a connection string, from a secure storage location.
    /// </summary>
    /// <param name="secretKey">The unique identifier of the secret to be retrieved.</param>
    /// <param name="cancellation">A CancellationToken that can be used to cancel the operation.</param>
    /// <returns>An observable sequence that contains the value of the secret.</returns>
    /// <remarks>
    /// This method uses an asynchronous design pattern based on the IObservable&lt;T&gt; interface. It returns immediately with an IObservable&lt;T&gt; 
    /// that can be used to track the completion of the operation and handle any exceptions that might occur.
    /// The secret storage location and the mechanism to retrieve the secret are implementation-dependent.
    /// </remarks>

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

    /// <summary>
    /// Defines the retry policy for running post-upgrade tests on the database.
    /// </summary>
    /// <param name="arg">The retry policy arguments, which include details about the current retry attempt, such as the current retry count, the exception that caused the retry, and the delay before the next retry.</param>
    /// <param name="cancellation">A CancellationToken that can be used to cancel the operation.</param>
    /// <returns>An observable sequence that notifies observers about the retry policy parameters for each new retry attempt.</returns>
    /// <remarks>
    /// This method is typically used in conjunction with the <see cref="PerformPostUpgradeTestsAsync"/> method. 
    /// If <see cref="PerformPostUpgradeTestsAsync"/> encounters an exception, this method determines whether to retry the test, 
    /// and if so, the delay before the next attempt. 
    /// By overriding this method in a derived class, you can customize the retry behavior for post-upgrade testing.
    /// </remarks>
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



    /// <summary>
    /// Asynchronously handles the post-upgrade actions within a transaction scope.
    /// </summary>
    /// <param name="transaction">The <see cref="DbTransaction"/> in which the upgrade has been performed.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> used to propagate notification that operations should be canceled.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result indicates that the transaction has been committed.</returns>
    /// <remarks>
    /// This method gets called after the database upgrade is done. By default, it commits the database transaction, finalizing the changes. Override this method to add additional steps after the upgrade process or to alter the default transaction behavior.
    /// </remarks>
    protected virtual Task OnUpgradedAsync(
        DbTransaction transaction, 
        CancellationToken cancellation)=> transaction.CommitAsync(cancellation);

    /// <summary>
    /// Executes the provided script against the given database connection if certain conditions are met.
    /// </summary>
    /// <remarks>
    /// This method is designed to ensure the idempotency of setup scripts and one-time execution of migration scripts.
    /// Setup scripts are idempotent, meaning they can be run multiple times without causing negative effects or changing the result beyond the initial application.
    /// Migration scripts, on the other hand, are designed to be executed only once.
    /// The decision of whether or not to execute the script should be implemented in this method.
    /// </remarks>
    /// <param name="connection">The database connection on which the script will be executed.</param>
    /// <param name="script">The script to potentially be executed.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The result of the task indicates whether the script was executed.</returns>
    protected abstract Task<bool> ExecuteScriptIfApplicableAsync(DbConnection connection, Script script, CancellationToken cancellation);

    /// <summary>
    /// Begins a new database transaction asynchronously as part of the upgrade process.
    /// </summary>
    /// <param name="cancellation">A <see cref="CancellationToken"/> used to propagate notification that operations should be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains a new instance of <see cref="DbTransaction"/> that is used to perform the upgrade.</returns>
    /// <remarks>
    /// This abstract method must be implemented in a derived class to define the exact transaction behavior during the upgrade process. A transaction represents a unit of work, and in this case, is used for the database upgrade operation to ensure data consistency and recoverability.
    /// </remarks>
    protected abstract Task<DbTransaction> BeginUpgradeTransactionAsync(CancellationToken cancellation);

    /// <summary>
    /// Gets the collection of scripts that should be executed for the database upgrade process.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Script"/> that represents the set of upgrade scripts.</returns>
    /// <remarks>
    /// This abstract method should be implemented in a derived class to provide the specific upgrade scripts needed for the database. These scripts are executed during the upgrade process and could include operations such as data transformations, table alterations, or new data additions. The order of scripts in the returned sequence may influence the upgrade process, thus implementors should ensure their correct order.
    /// </remarks>
    protected abstract IEnumerable<Script> GetUpgradeScripts();


    [DebuggerStepThrough]
    Task IPgManager.CreateDbAsync(CancellationToken cancellation) => Observable
        .FromAsync([DebuggerStepThrough] () => this.CreateDbAsync(cancellation))
        .WithRetryPolicy([DebuggerStepThrough] (args) => CreateDbRetryPolicy(args, cancellation))
        .ToTask(cancellation);



    [DebuggerStepThrough]
    Task IPgManager.DropDbAsync(CancellationToken cancellation) =>
        Observable
            .FromAsync([DebuggerStepThrough] () => this.DropDbAsync(cancellation))
            .WithRetryPolicy([DebuggerStepThrough] (args) => DropDbRetryPolicy(args, cancellation))
            .ToTask(cancellation);

    [DebuggerStepThrough]
    async Task IPgManager.PerformPostUpgradeTestsAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        await Observable
            .FromAsync(() => PerformPostUpgradeTestsAsync(cancellation))
            .WithRetryPolicy(args => TestRetryPolicy(args, cancellation))
            .ToTask(cancellation);
    }
}