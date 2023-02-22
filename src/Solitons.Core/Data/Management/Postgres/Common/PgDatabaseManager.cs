using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Solitons.Collections;

namespace Solitons.Data.Management.Postgres.Common;

/// <summary>
/// 
/// </summary>
public abstract class PgDatabaseManager
{

    private readonly HashSet<string> _loginRoles;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="databaseName"></param>
    /// <param name="databaseOwner"></param>
    /// <param name="loginRoles"></param>
    protected PgDatabaseManager(
        string databaseName,
        string databaseOwner,
        IEnumerable<string> loginRoles)
    { ;
        DatabaseName = ThrowIf
            .ArgumentNullOrWhiteSpace(databaseName, nameof(databaseName))
            .Trim();
        DatabaseOwner = ThrowIf
            .ArgumentNullOrWhiteSpace(databaseOwner, nameof(databaseOwner))
            .Trim();
        _loginRoles = loginRoles
            .Do(role => ThrowIf.NullOrEmpty(role, $"Database role name is missing"))
            .Select(role => role.Trim())
            .ToHashSet(StringComparer.Ordinal);
        _loginRoles.Remove(DatabaseOwner);
    }

    /// <summary>
    /// 
    /// </summary>
    public string DatabaseName { get; }

    /// <summary>
    /// 
    /// </summary>
    public string DatabaseOwner { get; }

    /// <summary>
    /// 
    /// </summary>
    public abstract void Upgrade();
    
    protected abstract string? GetRoleConnectionStringIfExists(string loginRole);
    protected abstract void SetRoleConnectionString(string loginRole, string loginConnectionString);

    protected virtual string GeneratePassword() => Guid.NewGuid().ToString("N");

    protected virtual string GetRoleConnectionString(string loginRole)
    {
        return GetRoleConnectionStringIfExists(loginRole)
            .ThrowIfNullOrWhiteSpace($"{loginRole} connection string not found.");
    }

    public virtual void Provision(PgConnectionFactory serverAdmin)
    {
        ThrowIfNotServerAdmin(serverAdmin);
        CreateDatabaseIfNotExists(serverAdmin);
        FluentList
            .Create(DatabaseOwner)
            .AddRange(_loginRoles)
            .ForEach(role => CreateRoleIfNotExists(role, serverAdmin));
        OnProvisioned(serverAdmin);
    }

    public virtual void Deprovision(PgConnectionFactory serverAdmin)
    {
        ThrowIfNotServerAdmin(serverAdmin);
        serverAdmin.Do(command =>
        {
            command.CommandTimeout = TimeSpan
                .FromMinutes(3)
                .Convert(ts => (int)ts.TotalSeconds);

            var dropLoginRolesSql = FluentList
                .Create(DatabaseOwner)
                .AddRange(_loginRoles)
                .Select(role => $"DROP ROLE IF EXISTS {role};")
                .Join(Environment.NewLine);
            command.CommandText = $@"
            SELECT *, pg_terminate_backend(pid)
            FROM pg_stat_activity 
            WHERE pid <> pg_backend_pid()
            AND datname = '{DatabaseName}';

            
            DROP DATABASE IF EXISTS {DatabaseName} WITH (FORCE);
            {dropLoginRolesSql}";
            command.ExecuteNonQuery();
        });
        OnDeprovisioned();
    }

    protected virtual void OnDeprovisioned(){}

    protected virtual void OnProvisioned(PgConnectionFactory serverAdmin)
    {
        ThrowIfNotServerAdmin(serverAdmin);
        serverAdmin.Do(command =>
        {
            command.CommandText = $@"
                GRANT {DatabaseOwner} TO current_user;
                ALTER DATABASE {DatabaseName} OWNER TO {DatabaseOwner};";
            command.ExecuteNonQuery();

            command.CommandText = _loginRoles
                .Select(role => $"GRANT CONNECT ON DATABASE {DatabaseName} TO {role};")
                .Join("; ");
            command.ExecuteNonQuery();
        });
    }

    protected virtual void ThrowIfNotServerAdmin(PgConnectionFactory serverAdmin)
    {
        var authorized = serverAdmin.Do(command =>
        {
            command.CommandText = $@"
                    SELECT EXISTS(
                        SELECT 1
                        FROM pg_roles 
                        WHERE rolname = current_user AND rolcreaterole AND rolcreatedb)";
            var result = command.ExecuteScalar() ?? false;
            return result.Equals(true);
        });

        if (authorized == false)
        {
            throw new InvalidOperationException(
                $"The {serverAdmin.Username} does not have permissions to perform the atabase provisioning operation.");
        }
    }


    protected virtual void CreateRoleIfNotExists(string loginRole, PgConnectionFactory factory)
    {
        var connectionString = GetRoleConnectionStringIfExists(loginRole);
        if (connectionString.IsPrintable())
        {
            try
            {
                var valid = factory
                    .WithConnectionString(connectionString!)
                    .TestConnection(factory.Host, DatabaseName, loginRole);
                if (valid)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }

        var login = factory
            .WithDatabase(DatabaseName)
            .WithUsername(loginRole)
            .WithPassword(GeneratePassword());
        SetRoleConnectionString(loginRole, login.ConnectionString);

        factory.Do(command =>
        {
            command.CommandText = $@"
                DO
                $DO$
                BEGIN
                    IF EXISTS(SELECT 1 FROM pg_catalog.pg_roles WHERE  rolname = '{loginRole}') THEN
                        ALTER ROLE {loginRole} WITH LOGIN PASSWORD $${login.Password}$$;
                    ELSE
                        CREATE ROLE {loginRole} WITH LOGIN PASSWORD $${login.Password}$$;
                    END IF;
                END;
                $DO$;
                GRANT CONNECT ON DATABASE {DatabaseName} TO {loginRole}; ";
            command.ExecuteNonQuery();
        });
    }

    protected virtual bool CreateDatabaseIfNotExists(PgConnectionFactory serverAdmin)
    {
        ThrowIfNotServerAdmin(serverAdmin);
        var exists = serverAdmin
            .Do(command =>
            {
                command.CommandText = $@"SELECT EXISTS(SELECT 1 FROM pg_database WHERE datname = '{DatabaseName}')";
                return (command.ExecuteScalar() ?? false).Equals(true);
            });
        if (exists)
        {
            return false;
        }

        serverAdmin
            .Do(command =>
            {
                command.CommandText = $@"CREATE DATABASE {DatabaseName};";
                command.ExecuteNonQuery();
            });
        
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static bool IsValidPassword(string password)
    {
        if (password.IsNullOrWhiteSpace() ||
            password.Length > 99)
            return false;
        return true;
    }
}