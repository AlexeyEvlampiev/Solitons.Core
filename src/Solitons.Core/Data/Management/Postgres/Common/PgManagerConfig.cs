using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Solitons.Data.Management.Postgres.Common;

/// <summary>
/// Provides the necessary configuration parameters for the Postgres manager.
/// </summary>
/// <remarks>
/// The <see cref="PgManagerConfig"/> record encapsulates the key configuration settings for the Postgres database, including the database name, an optional shared password for all roles, and a mapping of roles to their connection string secret keys.
/// </remarks>
public record PgManagerConfig
{
    private static readonly Regex DatabaseNameRegex = new Regex(@"^(?i)[a-z_]\w{0,62}$");

    /// <summary>
    /// Initializes a new instance of the <see cref="PgManagerConfig"/> class.
    /// </summary>
    /// <param name="databaseName">The name of the Postgres database to manage.</param>
    /// <exception cref="ArgumentException">Thrown if the provided database name is not valid.</exception>
    public PgManagerConfig(string databaseName)
    {
        DatabaseName = IsValidDatabaseName(databaseName) 
            ? databaseName 
            : throw new ArgumentException($"The provided database name '{databaseName}' is invalid. A valid database name must start with a letter or underscore, can contain letters, digits, and underscores, and must not be longer than 63 characters.");

        RoleConnectionSecretKeys =
            ImmutableDictionary.Create<string, string>(StringComparer.Ordinal);
    }

    /// <summary>
    /// Determines whether the provided string is a valid Postgres database name.
    /// </summary>
    /// <param name="databaseName">The name of the database to validate.</param>
    /// <returns>
    /// <see langword="true"/> if the provided database name is valid; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// A valid Postgres database name must meet the following criteria:
    /// - It must start with a letter or an underscore.
    /// - It can contain letters, digits, and underscores.
    /// - It cannot be longer than 63 characters.
    /// This method uses case-insensitive matching (that is, "myDatabase" and "MYDATABASE" are considered the same).
    /// </remarks>
    [DebuggerNonUserCode]
    public static bool IsValidDatabaseName(string databaseName) => DatabaseNameRegex.IsMatch(databaseName);

    /// <summary>
    /// Gets the name of the target Postgres database.
    /// </summary>
    public string DatabaseName { get; }

    /// <summary>
    /// Optionally gets a shared password to be used by all roles.
    /// </summary>
    /// <remarks>
    /// This property is handy in development environments where all database roles with login privileges can use the same password. 
    /// However, it is strongly recommended not to use this feature in production environments, especially those exposed to the Internet or other external parties.
    /// </remarks>
    public string? SharedPassword { get; init; }

    /// <summary>
    /// Gets the mapping of Postgres roles to their associated connection string secret keys.
    /// </summary>
    /// <remarks>
    /// This property facilitates secure role management by mapping each role to its corresponding secret key. This collection is used by the <see cref="PgManager"/> class 
    /// to set and retrieve the server roles needed for managing the database. The connection strings referenced by these secret keys should be stored securely in a suitable secrets repository.
    /// </remarks>
    public ImmutableDictionary<string, string> RoleConnectionSecretKeys { get; init; } = ImmutableDictionary<string, string>.Empty;
}