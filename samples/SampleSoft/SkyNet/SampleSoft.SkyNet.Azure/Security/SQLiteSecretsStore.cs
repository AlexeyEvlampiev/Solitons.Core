using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Solitons;
using Solitons.Security;
using Solitons.Security.Common;

namespace SampleSoft.SkyNet.Azure.Security;

/// <summary>
/// Provides a .NET Core specific implementation of an SQLite-based secrets repository.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public sealed class SQLiteSecretsStore : SQLiteSecretsRepository
{
    /// <summary>
    /// Provides .NET Core specific implementation for creating and managing SQLite connections and connection strings.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    sealed class DotNetCoreSQLiteProvider : ISQLIteProvider
    {
        /// <summary>
        /// Creates a new SQLite database connection using the provided connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to be used for the SQLite database connection.</param>
        /// <returns>A new SQLite database connection.</returns>
        [DebuggerNonUserCode]
        public DbConnection CreateConnection(string connectionString) => new SQLiteConnection(connectionString);

        /// <summary>
        /// Creates a new SQLite database connection string using the provided file path.
        /// </summary>
        /// <param name="filePath">The file path to be used for the SQLite database connection string.</param>
        /// <returns>A new SQLite database connection string.</returns>
        [DebuggerNonUserCode]
        public string CreateConnectionString(string filePath) => $"Data Source={filePath};Version=3;";
    }

    /// <summary>
    /// Initializes a new instance of the DotNetCoreSQLiteSecretsRepository class with the specified file path and scope name.
    /// </summary>
    /// <param name="filePath">The file path of the SQLite database.</param>
    /// <param name="scopeName">The scope name for secrets.</param>
    [DebuggerStepThrough]
    public SQLiteSecretsStore(string filePath, string scopeName)
        : base(filePath, scopeName, new DotNetCoreSQLiteProvider())
    {
    }

    /// <summary>
    /// Creates a new instance of the DotNetCoreSQLiteSecretsRepository class based on a scope connection string.
    /// </summary>
    /// <param name="scopeConnectionString">The scope connection string.</param>
    /// <returns>A new DotNetCoreSQLiteSecretsRepository instance.</returns>
    /// <exception cref="FormatException">Throws when the provided scopeConnectionString does not match the expected format.</exception>
    public static ISecretsRepository Create(string scopeConnectionString)
    {
        ThrowIf.ArgumentNullOrWhiteSpace(scopeConnectionString);
        if (IsScopeConnectionString(scopeConnectionString, out var filePath, out var scopeName))
        {
            return new SQLiteSecretsStore(filePath, scopeName);
        }

        throw new FormatException("Invalid secret scope connection string");
    }
}