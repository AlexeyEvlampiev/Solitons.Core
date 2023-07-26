using System;

namespace Solitons.Data.Common.Postgres;

/// <summary>
/// Represents connection information for a Postgres database.
/// </summary>
public sealed record PgConnectionInfo
{

    /// <summary>
    /// Initializes a new instance of the <see cref="PgConnectionInfo"/> class
    /// with default values for all properties.
    /// </summary>
    public PgConnectionInfo()
    {
        Host = "localhost";
        Port = 5432;
        Database = "postgres";
        Username = "postgres";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PgConnectionInfo"/> class
    /// with the specified property values.
    /// </summary>
    /// <param name="host">The hostname or IP address of the Postgres server.</param>
    /// <param name="port">The port number on which the Postgres server listens.</param>
    /// <param name="database">The name of the Postgres database to connect to.</param>
    /// <param name="username">The username to use when connecting to the Postgres database.</param>
    public PgConnectionInfo(string host, int port, string database, string username)
    {
        Host = host;
        Port = port;
        Database = database;
        Username = username;
    }

    /// <summary>
    /// Gets or sets the hostname or IP address of the Postgres server.
    /// </summary>
    public string Host { get; init; }

    /// <summary>
    /// Gets or sets the port number on which the Postgres server listens.
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// Gets or sets the name of the Postgres database to connect to.
    /// </summary>
    public string Database { get; init; }

    /// <summary>
    /// Gets or sets the username to use when connecting to the Postgres database.
    /// </summary>
    public string Username { get; init; }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public bool Equals(PgConnectionInfo? other)
    {
        if (other is null)
        {
            return false;
        }

        return
            Host.Equals(other.Host, StringComparison.Ordinal) &&
            Port.Equals(other.Port) &&
            Database.Equals(other.Database, StringComparison.Ordinal) &&
            Username.Equals(other.Username, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Returns a hash code for this <see cref="PgConnectionInfo"/> object.
    /// </summary>
    /// <returns>A hash code for this <see cref="PgConnectionInfo"/> object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Host, Port, Database, Username);
    }

    /// <summary>
    /// Returns a string that represents the current <see cref="PgConnectionInfo"/> object.
    /// </summary>
    /// <returns>A string that represents the current <see cref="PgConnectionInfo"/> object.</returns>
    public override string ToString() => $"Host={Host};Port={Port};Database={Database};Username={Username}";
}