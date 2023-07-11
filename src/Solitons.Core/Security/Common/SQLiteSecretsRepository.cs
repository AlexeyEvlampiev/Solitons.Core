using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Reactive;

namespace Solitons.Security.Common;

/// <summary>
/// Provides an SQLite-based implementation of a secrets repository.
/// </summary>
// ReSharper disable once InconsistentNaming
public abstract class SQLiteSecretsRepository : SecretsRepository
{
    private readonly string _connectionString;
    private readonly string _scopeName;
    private readonly ISQLIteProvider _provider;
    private static readonly Regex ScopeRegex = new Regex(@"^(?<path>.+?\.db)[|](?:scope=)?(?<scope>.+)");

    sealed class SecretNotFoundException : KeyNotFoundException
    {
        
    }

    /// <summary>
    /// Defines a contract for creating and managing SQLite connections and connection strings.
    /// </summary>
    protected interface ISQLIteProvider
    {
        /// <summary>
        /// Creates a new SQLite database connection using the provided connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to be used for the SQLite database connection.</param>
        /// <returns>A new SQLite database connection.</returns>
        DbConnection CreateConnection(string connectionString);

        /// <summary>
        /// Creates a new SQLite database connection string using the provided file path.
        /// </summary>
        /// <param name="filePath">The file path to be used for the SQLite database connection string.</param>
        /// <returns>A new SQLite database connection string.</returns>
        string CreateConnectionString(string filePath);
    }


    /// <summary>
    /// Initializes a new instance of the SQLiteSecretsRepository class with the specified file path and scope name.
    /// </summary>
    /// <param name="filePath">The file path of the SQLite database.</param>
    /// <param name="scopeName">The scope name for secrets.</param>
    /// <param name="provider">The SQLite provider for database operations.</param>
    /// <exception cref="ArgumentException">Throws when the provided file path does not end with the .db extension.</exception>
    protected SQLiteSecretsRepository(string filePath, string scopeName, ISQLIteProvider provider)
    {
        _scopeName = scopeName;
        _provider = provider;
        var extension = Path.GetExtension(filePath);
        if (false == ".db".Equals(extension, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Provided file path '{filePath}' does not end with the .db extension.");
        }
        _connectionString = provider.CreateConnectionString(filePath);
        using var connection = provider.CreateConnection(_connectionString);
        using var command = connection.CreateCommand();
        command.CommandText = $@"

        CREATE TABLE IF NOT EXISTS secret (
            scope VARCHAR(150),
            key VARCHAR(150),
            value TEXT,
            created_utc DATETIME DEFAULT (datetime('now','utc')),
            updated_utc DATETIME DEFAULT (datetime('now','utc')),
            PRIMARY KEY (scope, key)
        );
        CREATE UNIQUE INDEX IF NOT EXISTS idx_secret_scope_key ON secret (scope, key);";
        connection.Open();
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Asynchronously gets a list of all secret names within the specified scope.
    /// </summary>
    /// <param name="cancellation">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A Task representing the asynchronous operation. The Task's result is an array of secret names.</returns>
    protected override async Task<string[]> ListSecretNamesAsync(CancellationToken cancellation)
    {
        await using var connection = _provider.CreateConnection(_connectionString);
        await using var command = connection.CreateCommand();
        command.CommandText = @"SELECT key FROM secret WHERE scope = @scope;";
        var scopeParameter = command.CreateParameter();
        scopeParameter.ParameterName = "@scope";
        scopeParameter.DbType = DbType.String;
        scopeParameter.Value = _scopeName;
        command.Parameters.Add(scopeParameter);

        await connection.OpenAsync(cancellation);
        await using var reader = await command.ExecuteReaderAsync(cancellation);
        var list = new List<string>();
        while (await reader.ReadAsync(cancellation))
        {
            list.Add(reader.GetString(0));
        }

        return list.ToArray();
    }

    /// <summary>
    /// Asynchronously gets the value of a secret with the specified name.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="cancellation">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A Task representing the asynchronous operation. The Task's result is the value of the secret.</returns>
    /// <exception cref="SecretNotFoundException">Throws when the secret does not exist.</exception>
    protected override async Task<string> GetSecretAsync(string secretName, CancellationToken cancellation)
    {
        var value = await GetSecretIfExistsAsync(secretName, cancellation);
        return (value.IsPrintable() ? value : throw new SecretNotFoundException())!;
    }

    /// <summary>
    /// Asynchronously gets the value of a secret if it exists.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="cancellation">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A Task representing the asynchronous operation. The Task's result is the value of the secret, or null if the secret does not exist.</returns>
    protected override async Task<string?> GetSecretIfExistsAsync(string secretName, CancellationToken cancellation)
    {
        await using var connection = _provider.CreateConnection(_connectionString);
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT value FROM secret WHERE scope = @scope AND key = @key;";
        var scopeParameter = command.CreateParameter();
        var keyParameter = command.CreateParameter();

        scopeParameter.ParameterName = "@scope";
        scopeParameter.DbType = DbType.String;
        scopeParameter.Value = _scopeName;

        keyParameter.ParameterName = "@key";
        keyParameter.DbType = DbType.String;
        keyParameter.Value = secretName;

        command.Parameters.Add(scopeParameter);
        command.Parameters.Add(keyParameter);

        await connection.OpenAsync(cancellation);
        var result = await command.ExecuteScalarAsync(cancellation);
        if (result is DBNull || result == null)
        {
            return null;
        }

        return result.ToString() ?? "";
    }

    /// <summary>
    /// Asynchronously gets the value of a secret with the specified name. If the secret does not exist, sets the secret with a provided default value.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="defaultValue">The default value to set if the secret does not exist.</param>
    /// <param name="cancellation">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A Task representing the asynchronous operation. The Task's result is the value of the secret, or the default value if the secret did not exist.</returns>
    protected override async Task<string> GetOrSetSecretAsync(string secretName, string defaultValue, CancellationToken cancellation)
    {
        await using var connection = _provider.CreateConnection(_connectionString);
        await connection.OpenAsync(cancellation);
        await using var transaction = await connection.BeginTransactionAsync(cancellation);

        // Check if secret already exists
        var command = connection.CreateCommand();
        command.CommandText = "SELECT value FROM secret WHERE scope = @scope AND key = @key;";
        var scopeParameter = command.CreateParameter();
        var keyParameter = command.CreateParameter();

        scopeParameter.ParameterName = "@scope";
        scopeParameter.DbType = DbType.String;
        scopeParameter.Value = _scopeName;

        keyParameter.ParameterName = "@key";
        keyParameter.DbType = DbType.String;
        keyParameter.Value = secretName;

        command.Parameters.Add(scopeParameter);
        command.Parameters.Add(keyParameter);

        command.Transaction = transaction;

        var result = await command.ExecuteScalarAsync(cancellation);
        if (result != null && !(result is DBNull))
        {
            // Secret exists, so return it
            await transaction.CommitAsync(cancellation);
            return result.ToString()!;
        }

        
        // Secret does not exist, so set it
        command.Parameters.Clear();
        command.CommandText = @"
        INSERT INTO secret (scope, key, value, updated_utc) 
        VALUES (@scope, @key, @defaultValue, datetime('now','utc'));";

        var defaultValueParameter = command.CreateParameter();

        defaultValueParameter.ParameterName = "@defaultValue";
        defaultValueParameter.DbType = DbType.String;
        defaultValueParameter.Value = defaultValue;

        command.Parameters.Add(scopeParameter);
        command.Parameters.Add(keyParameter);
        command.Parameters.Add(defaultValueParameter);

        command.Transaction = transaction;

        await command.ExecuteNonQueryAsync(cancellation);

        await transaction.CommitAsync(cancellation);

        return defaultValue;
    }

    /// <summary>
    /// Asynchronously sets the value of the secret with the specified name.
    /// </summary>
    /// <param name="secretName">The name of the secret to be set.</param>
    /// <param name="secretValue">The value to be set for the specified secret.</param>
    /// <param name="cancellation">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected override async Task SetSecretAsync(string secretName, string secretValue, CancellationToken cancellation)
    {
        await using var connection = _provider.CreateConnection(_connectionString);
        await using var command = connection.CreateCommand();
        command.CommandText = @"
        INSERT OR REPLACE INTO secret (scope, key, value, updated_utc) 
        VALUES (@scope, @key, @value, datetime('now','utc'));";

        var scopeIdParameter = command.CreateParameter();
        var keyParameter = command.CreateParameter();
        var valueParameter = command.CreateParameter();

        scopeIdParameter.ParameterName = "@scope";
        scopeIdParameter.DbType = DbType.String;
        scopeIdParameter.Value = _scopeName;

        keyParameter.ParameterName = "@key";
        keyParameter.DbType = DbType.String;
        keyParameter.Value = secretName;

        valueParameter.ParameterName = "@value";
        valueParameter.DbType = DbType.String;
        valueParameter.Value = secretValue;

        command.Parameters.Add(scopeIdParameter);
        command.Parameters.Add(keyParameter);
        command.Parameters.Add(valueParameter);

        await connection.OpenAsync(cancellation);
        await command.ExecuteNonQueryAsync(cancellation);
    }

    /// <summary>
    /// Checks whether the provided exception is a "secret not found" error.
    /// </summary>
    /// <param name="exception">The exception to check.</param>
    /// <returns>
    /// <c>true</c> if the exception is a "secret not found" error; otherwise, <c>false</c>.
    /// </returns>
    protected override bool IsSecretNotFoundError(Exception exception)
    {
        return exception is SecretNotFoundException;
    }


    /// <inheritdoc />
    protected override bool ShouldRetry(RetryPolicyArgs args)
    {
        return args.Exception is DbException {IsTransient: true};
    }

    /// <summary>
    /// Checks if the provided string is a valid scope connection string.
    /// </summary>
    /// <param name="scopeConnectionString">The string to be checked for scope connection string validity.</param>
    /// <param name="filePath">Outputs the file path from the connection string if it is valid, otherwise an empty string.</param>
    /// <param name="scopeName">Outputs the scope name from the connection string if it is valid, otherwise an empty string.</param>
    /// <returns>Returns true if the provided string is a valid scope connection string, otherwise false.</returns>
    public static bool IsScopeConnectionString(string scopeConnectionString, out string filePath, out string scopeName)
    {
        var match = ScopeRegex.Match(scopeConnectionString);
        if (match.Success == false)
        {
            filePath = string.Empty;
            scopeName = string.Empty;
            return false;
        }
        filePath = match.Groups["path"].Value;
        scopeName = match.Groups["scope"].Value;
        return true;
    }

}