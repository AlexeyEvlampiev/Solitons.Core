using System;
using System.Data;
using System.Diagnostics;

namespace Solitons.Security.Postgres
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PgSecurityManagementProvider : IDisposable
    {
        private readonly IDbConnection _connection;

        [DebuggerNonUserCode]
        private PgSecurityManagementProvider(IDbConnection connection)
        {
            _connection = connection;
        }

        [DebuggerStepThrough]
        public static PgSecurityManagementProvider Create(Func<IDbConnection> createConnection)
        {
            var connection = createConnection
                .ThrowIfNullArgument(nameof(createConnection))
                .Invoke()
                .ThrowIfNull(
                    () => new ArgumentException($"Connection factory returned null.", nameof(createConnection)));
            connection.Open();
            return new PgSecurityManagementProvider(connection);
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
            using var command = _connection.CreateCommand();
            command.CommandText = $"ALTER ROLE {roleName} WITH PASSWORD '{newPassword}'";
            command.ExecuteNonQuery();
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
            _connection.Dispose();
        }
    }
}
