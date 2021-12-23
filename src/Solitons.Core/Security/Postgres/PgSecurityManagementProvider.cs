using System;
using System.Data;
using System.Diagnostics;
using Solitons.Data;
using Solitons.Security.Postgres.Scripts;

namespace Solitons.Security.Postgres
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PgSecurityManagementProvider : IDisposable
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDbConnection _postgresDbConnection;

        [DebuggerNonUserCode]
        public PgSecurityManagementProvider(IDbConnectionFactory connectionFactory)
        {
            connectionFactory.ThrowIfNullArgument(nameof(connectionFactory));
            _connectionFactory = connectionFactory;
            _postgresDbConnection = _connectionFactory.WithDatabase("postgres").CreateConnection();
            _postgresDbConnection.Open();
        }



        public static bool IsValidPassword(string password)
        {
            if (password.IsNullOrWhiteSpace() || 
                password.Length > 99)
                return false;
            return true;
        }

        public void ChangeRolePassword(string databaseName, string roleName, string newPassword)
        {
            roleName = GetRoleFullName(databaseName, roleName
                .ThrowIfNullOrWhiteSpaceArgument(nameof(roleName)));
            using var command = _postgresDbConnection.CreateCommand();
            command.CommandText = $"ALTER ROLE {roleName} WITH PASSWORD '{newPassword}'";
            command.ExecuteNonQuery();
        }



        public void ProvisionDatabase(
            string databaseName, 
            Action<IPgRoleBuilder> configRoles = null,
            Action<IPgExtensionListBuilder> configExtensions = null)
        {
            var roles = new PgRoleBuilder();
            var extensions = new PgExtensionListBuilder();

            configRoles?.Invoke(roles);
            roles.Assert();

            configExtensions?.Invoke(extensions);

            CreatePgRolesScriptRtt.Execute(_postgresDbConnection, databaseName, roles);
            CreatePgDatabaseScriptRtt.Execute(_postgresDbConnection, databaseName);

            using var dbConnection = _connectionFactory.WithDatabase(databaseName).CreateConnection();
            dbConnection.Open();
            CreateExtensionsScriptRtt.Execute(dbConnection, databaseName, extensions);
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
            DropRolesByPrefixScriptRtt.Execute(_postgresDbConnection, prefix);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseName"></param>
        public void DropDatabase(string databaseName)
        {
            databaseName.ThrowIfNullOrWhiteSpaceArgument(nameof(databaseName));
            DropDatabaseScriptRtt.Execute(_postgresDbConnection, databaseName);
        }

        private string GetRoleFullName(string databaseName, string roleName)
        {
            return roleName
                .StartsWith($"{databaseName}_")
                ? roleName
                : $"{databaseName}_{roleName}";
        }


        void IDisposable.Dispose()
        {
            _postgresDbConnection.Dispose();
        }
    }
}
