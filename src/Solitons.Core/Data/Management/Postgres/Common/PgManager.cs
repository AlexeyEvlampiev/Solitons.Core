using Solitons.Reactive;
using System;
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
public abstract class PgManager : IPgManager
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
    protected virtual async Task CreateDbAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        await FluentObservable
            .Defer(async () =>
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
            })
            .RetryWhen(ShouldRetry(cancellation));


        var maintenanceDb = ExtractConnectionInfo(ConnectionString);
        
        foreach (var roleSecretKey in _config.RoleConnectionStringSecretKeys)
        {
            var (role, secretKey) = (roleSecretKey.Key, roleSecretKey.Value);
            var expected = maintenanceDb with
            {
                Database = Database,
                Username = role
            };

            var currentValueIsValid = await FluentObservable
                .Defer(() => GetSecretIfExistsAsync(secretKey, cancellation))
                .Where(cs => cs.IsPrintable())
                .SelectMany(async cs =>
                {
                    var actual = ExtractConnectionInfo(cs!);
                    if (actual.Equals(expected))
                    {
                        var valid = await FluentObservable
                            .Defer(async () =>
                            {
                                await using var conn = CreateDbConnection(cs!);
                                await conn.OpenAsync(cancellation);
                                return true;
                            })
                            .RetryWhen(ShouldRetry(cancellation));
                        if (valid)
                        {
                            return true;
                        }
                    }

                    await OnInvalidConnectionStringAsync(
                        $"The {secretKey} connection string is invalid." +
                        $" Expected: {expected}." +
                        $" Actual: {actual}.",
                        role,
                        cancellation);
                    return false;
                })
                .RetryWhen(ShouldRetry(cancellation))
                .OnErrorResumeNext(Observable.Return(false))
                .FirstOrDefaultAsync(_ => _);

            if (currentValueIsValid)
            {
                continue;
            }

            var password = _config.SharedPassword
                .DefaultIfNullOrWhiteSpace(GenRandomPassword())
                .DefaultIfNullOrWhiteSpace(Guid
                    .NewGuid()
                    .ToString("N"))
                .Trim();

            await FluentObservable
                .Defer(async () =>
                {
                    await using var connection = CreateDbConnection(ConnectionString);
                    await connection.OpenAsync(cancellation);
                    await using var transaction = await connection.BeginTransactionAsync(cancellation);
                    await connection.DoAsync(async cmd =>
                    {
                        cmd.CommandText = $@"
                        DO
                        $DO$
                        BEGIN
                            IF EXISTS(SELECT 1 FROM pg_catalog.pg_roles WHERE  rolname = '{role}') THEN
                                ALTER ROLE {role} WITH LOGIN PASSWORD $${password}$$;
                            ELSE
                                CREATE ROLE {role} WITH LOGIN PASSWORD $${password}$$;
                            END IF;
                        END;
                        $DO$;

                        GRANT {role} TO current_user;
                        GRANT CONNECT ON DATABASE {_config.DatabaseName} TO {role}";
                        await cmd.ExecuteNonQueryAsync(cancellation);
                    });
                    var secretValue = ConstructConnectionString(
                        ConnectionString,
                        new DatabaseConnectionOptions
                        {
                            Database = _config.DatabaseName,
                            Username = role,
                            Password = password
                        });
                    await SaveSecretAsync(secretKey, secretValue, cancellation);
                    await transaction.CommitAsync(CancellationToken.None);
                })
                .RetryWhen(ShouldRetry(cancellation));
        }
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
    /// <param name="role">The role for which to retrieve the connection string.</param>
    /// <param name="cancellation">The cancellation token to use.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected abstract Task<string?> GetSecretIfExistsAsync(
        string role, 
        CancellationToken cancellation);




    /// <summary>
    /// Returns a function that, given an observable sequence of exceptions, returns an observable sequence of signals indicating whether a retry attempt should be made.
    /// </summary>
    /// <param name="cancellation">The cancellation token to observe for cancellation requests.</param>
    /// <returns>A function that maps an observable sequence of exceptions to an observable sequence of retry signals.</returns>
    [DebuggerStepThrough]
    protected virtual Func<IObservable<Exception>, IObservable<Unit>> ShouldRetry(CancellationToken cancellation)
    {
        [DebuggerStepThrough]
        IObservable<Unit> ToSignals(IObservable<Exception> exceptions)
        {
            return exceptions
                .SelectMany([DebuggerStepThrough] (ex, attempt) =>
                {
                    if (ex is not DbException { IsTransient: true })
                    {
                        return Observable.Throw<Unit>(ex);
                    }

                    var delay = CalculateRetryDelay(attempt, ex);
                    return ToDelay(delay, cancellation);

                });
        }

        return ToSignals;
    }

    /// <summary>
    /// Calculates the delay between retry attempts for the specified operation,
    /// based on the number of attempts made and the exception that occurred.
    /// </summary>
    /// <param name="attempt">The number of attempts that have been made so far.</param>
    /// <param name="ex">The exception that occurred during the previous attempt.</param>
    /// <returns>A <see cref="TimeSpan"/> representing the delay before the next retry attempt.</returns>
    protected virtual TimeSpan CalculateRetryDelay(int attempt, Exception ex)
    {
        return TimeSpan
            .FromSeconds(attempt.Max(50) * 100);
    }

    protected IObservable<Unit> ToDelay(TimeSpan interval, CancellationToken cancellation) =>
        Task
            .Delay(interval, cancellation)
            .ToObservable();

    [DebuggerStepThrough]
    Task IPgManager.CreateDbAsync(CancellationToken cancellation) => FluentObservable
        .Defer([DebuggerStepThrough] () => this.CreateDbAsync(cancellation))
        .RetryWhen(ShouldRetry(cancellation))
        .ToTask(cancellation);


    [DebuggerStepThrough]
    Task IPgManager.DropDbAsync(CancellationToken cancellation) =>
        FluentObservable
            .Defer([DebuggerStepThrough] () => this.DropDbAsync(cancellation))
            .RetryWhen(ShouldRetry(cancellation))
            .ToTask(cancellation);

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

}