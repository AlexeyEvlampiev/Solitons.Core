using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Solitons;
using Solitons.Security;
using Solitons.Security.Common;

namespace SampleSoft.SkyNet.Azure.SQLite;

public sealed class SQLiteSecretsRepository : SecretsRepository
{
    private readonly string _connectionString;
    private readonly string _scopeName;
    private static readonly Regex ScopeRegex;

    static SQLiteSecretsRepository()
    {
        ScopeRegex = new Regex(@"^(?<path>.+?\.db)[|](?:scope=)?(?<scope>.+)");
    }

    public SQLiteSecretsRepository(string filePath, string scopeName)
    {
        _scopeName = scopeName;
        var extension = Path.GetExtension(filePath);
        if (false == ".db".Equals(extension, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException();
        }
        _connectionString = $"Data Source={filePath};Version=3;";
        using var connection = new SQLiteConnection(_connectionString);
        using var command = new SQLiteCommand(connection);
        command.CommandText = $@"
        CREATE TABLE IF NOT EXISTS scope 
        (
            id VARCHAR(150) PRIMARY KEY,
            created_utc DATETIME DEFAULT (datetime('now','utc'))
        );

        CREATE TABLE IF NOT EXISTS secret (
            scope_id VARCHAR(150),
            key VARCHAR(150),
            value TEXT,
            created_utc DATETIME DEFAULT (datetime('now','utc')),
            updated_utc DATETIME DEFAULT (datetime('now','utc')),
            PRIMARY KEY (scope_id, key),
            FOREIGN KEY(scope_id) REFERENCES scope(id)
        );
        CREATE INDEX IF NOT EXISTS idx_secret_scope_id
            ON secret (scope_id, key);

        INSERT OR IGNORE INTO scope (id) VALUES ('{scopeName}');
        ";
        connection.Open();
        command.ExecuteNonQuery();
    }


    protected override async Task<string[]> ListSecretNamesAsync(CancellationToken cancellation)
    {
        await using var connection = new SQLiteConnection(_connectionString);
        await using var command = connection.CreateCommand();
        command.CommandText = @$"SELECT key FROM secret WHERE scope_id = '{_scopeName}';";
        await connection.OpenAsync(cancellation);
        await using var reader = await command.ExecuteReaderAsync(cancellation);
        var list = new List<string>();
        while (await reader.ReadAsync(cancellation))
        {
            list.Add(reader.GetString(0));
        }

        return list.ToArray();
    }

    protected override async Task<string> GetSecretAsync(string secretName)
    {
        var value = await GetSecretIfExistsAsync(secretName);
        return (value.IsPrintable() ? value : throw new KeyNotFoundException())!;
    }

    protected override async Task<string?> GetSecretIfExistsAsync(string secretName)
    {
        await using var connection = new SQLiteConnection(_connectionString);
        await using var command = connection.CreateCommand();
        command.CommandText = @$"SELECT value FROM secret WHERE scope_id = '{_scopeName}' AND key = '{secretName}';";
        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        if (result is DBNull || result == null)
        {
            return null;
        }

        return result.ToString() ?? "";
    }

    protected override async Task<string> GetOrSetSecretAsync(string secretName, string defaultValue)
    {
        await using var connection = new SQLiteConnection(_connectionString);
        await using var command = connection.CreateCommand();

        throw new NotImplementedException();
    }

    protected override async Task SetSecretAsync(string secretName, string secretValue)
    {
        await using var connection = new SQLiteConnection(_connectionString);
        await using var command = connection.CreateCommand();
        command.CommandText = @$"
        INSERT OR REPLACE INTO secret (scope_id, key, value, updated_utc) 
        VALUES ('{_scopeName}', '{secretName}', '{secretValue}', datetime('now','utc'));";
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    protected override bool IsSecretNotFoundError(Exception exception)
    {
        return exception is KeyNotFoundException;
    }

    public static bool IsScopeConnectionString(string scopeConnectionString)
    {
        return ScopeRegex.IsMatch(scopeConnectionString);
    }

    public static ISecretsRepository Create(string scopeConnectionString)
    {
        ThrowIf.ArgumentNullOrWhiteSpace(scopeConnectionString);
        var match = ScopeRegex.Match(scopeConnectionString);
        if (match.Success == false)
        {
            throw new FormatException("Invalid secret scope connection string");
        }
        var path = match.Groups["path"].Value;
        var scope = match.Groups["scope"].Value;
        return new SQLiteSecretsRepository(path, scope);
    }
}