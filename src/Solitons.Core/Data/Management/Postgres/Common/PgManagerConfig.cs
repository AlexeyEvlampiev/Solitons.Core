using System;
using System.Collections.Immutable;

namespace Solitons.Data.Management.Postgres.Common;

/// <summary>
/// Configuration options for the Postgres manager.
/// </summary>
public record PgManagerConfig
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PgManagerConfig"/> class with the specified database name.
    /// </summary>
    /// <param name="databaseName">The name of the Postgres database.</param>
    public PgManagerConfig(string databaseName)
    {
        DatabaseName = ThrowIf.ArgumentNullOrWhiteSpace(databaseName).Trim();
        RoleConnectionStringSecretKeys =
            ImmutableDictionary.Create<string, string>(StringComparer.Ordinal);
    }

    /// <summary>
    /// Gets the name of the target Postgres database.
    /// </summary>
    public string DatabaseName { get; init; }

    /// <summary>
    /// Gets a shared password to be used by all roles.
    /// </summary>
    public string? SharedPassword { get; init; }

    /// <summary>
    /// Gets the mapping of secret keys to connection strings for roles.
    /// </summary>
    public ImmutableDictionary<string, string> RoleConnectionStringSecretKeys { get; init; } = ImmutableDictionary<string, string>.Empty;
}