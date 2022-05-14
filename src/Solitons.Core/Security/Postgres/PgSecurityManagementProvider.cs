using System;
using System.Diagnostics;
using Solitons.Data;
using Solitons.Security.Postgres.Scripts;

namespace Solitons.Security.Postgres
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PgSecurityManagementProvider
    {
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionFactory"></param>
        [DebuggerNonUserCode]
        public PgSecurityManagementProvider(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory
                .ThrowIfNullArgument(nameof(connectionFactory))
                .WithDatabase("postgres");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="roleName"></param>
        /// <param name="newPassword"></param>
        public void ChangeRolePassword(string databaseName, string roleName, string newPassword)
        {
            roleName = GetRoleFullName(databaseName, roleName
                .ThrowIfNullOrWhiteSpaceArgument(nameof(roleName)));
            using var connection = _connectionFactory.CreateConnection();
            using var command = connection.CreateCommand();
            connection.Open();
            command.CommandText = $"ALTER ROLE {roleName} WITH PASSWORD '{newPassword}'";
            command.ExecuteNonQuery();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="configRoles"></param>
        /// <param name="configExtensions"></param>
        /// <param name="namingRules"></param>
        public void ProvisionDatabase(
            string databaseName, 
            Action<PgRolesBuilder>? configRoles = null,
            Action<IPgExtensionListBuilder>? configExtensions = null,
            PgNamingRules? namingRules = null)
        {
            namingRules ??= new PgNamingRules();
            var roles = new PgRolesBuilder();
            var extensions = new PgExtensionListBuilder();

            configRoles?.Invoke(roles);
            configExtensions?.Invoke(extensions);

            var connection = _connectionFactory.CreateConnection();
            var command = connection.CreateCommand();

            try
            {
                connection.Open();
                command.CommandText = new CreatePgRolesScriptRtt(databaseName, roles, namingRules).ToString();
                command.ExecuteNonQuery();

                command.CommandText = $@"SELECT true FROM pg_database WHERE datname = '{databaseName}'";
                bool databaseExists = true.Equals(command.ExecuteScalar());
                if (false == databaseExists)
                {
                    command.CommandText = $@"CREATE DATABASE {databaseName} WITH OWNER {namingRules.BuildRoleFullName(databaseName,"admin")};";
                    command.ExecuteNonQuery();
                }

                command.CommandText = new CreatePgDatabaseScriptRtt(databaseName, namingRules).ToString();
                command.ExecuteNonQuery();
                connection.Close();

                connection.Dispose();
                command.Dispose();

                connection = _connectionFactory.WithDatabase(databaseName).CreateConnection();
                command = connection.CreateCommand();
                command.CommandText = new CreateExtensionsScriptRtt(databaseName, extensions, namingRules);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                connection.Dispose();
                command.Dispose();
            }
            catch(Exception)
            {
                connection.Dispose();
                command.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <exception cref="ArgumentException"></exception>
        public void DropRolesByPrefix(string prefix)
        {
            if (prefix
                .ThrowIfNullOrWhiteSpaceArgument(nameof(prefix))
                .StartsWith("pg_"))
            {
                throw new ArgumentException($"Specified roles cannot be deleted.");
            }

            using var connection = _connectionFactory.CreateConnection();
            using var command = connection.CreateCommand();
            connection.Open();
            command.CommandText = new DropRolesByPrefixScriptRtt(prefix).ToString();
            command.ExecuteNonQuery();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseName"></param>
        public void DropDatabase(string databaseName)
        {
            databaseName = databaseName
                .ThrowIfNullOrWhiteSpaceArgument(nameof(databaseName))
                .Trim();

            using var connection = _connectionFactory.CreateConnection();
            using var command = connection.CreateCommand();
            command.CommandText = @$"
                SELECT *, pg_terminate_backend(pid)
                FROM pg_stat_activity 
                WHERE pid <> pg_backend_pid()
                AND datname = '{databaseName}';

                DROP DATABASE IF EXISTS {databaseName};";
            connection.Open();
            command.ExecuteNonQuery();
        }

        private string GetRoleFullName(string databaseName, string roleName)
        {
            return roleName
                .StartsWith($"{databaseName}_")
                ? roleName
                : $"{databaseName}_{roleName}";
        }


    }
}
