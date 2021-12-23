using System;
using System.Data;

namespace Solitons.Security.Postgres.Scripts
{
    public sealed class DropDatabaseScriptRtt
    {
        private DropDatabaseScriptRtt(string databaseName)
        {
            DatabaseName = databaseName;
        }

        public string DatabaseName { get; }

        public static void Execute(IDbConnection connection, string databaseName)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (databaseName == null) throw new ArgumentNullException(nameof(databaseName));
            using var command = connection.CreateCommand();
            command.CommandText = @$"
                SELECT *, pg_terminate_backend(pid)
                FROM pg_stat_activity 
                WHERE pid <> pg_backend_pid()
                AND datname = '{databaseName}';

                DROP DATABASE IF EXISTS {databaseName};";
            command.ExecuteNonQuery();
        }
    }
}
