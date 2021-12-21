using System;
using System.Data;

namespace Solitons.Data.Postgres
{
    public partial class CreateDbScriptRtt
    {
        private CreateDbScriptRtt(string databaseName, bool exists)
        {
            DatabaseName = databaseName;
            DatabaseExists = exists;
        }

        public string DatabaseName { get; }
        public bool DatabaseExists { get; }
        public object DatabaseAdmin => $"{DatabaseName}_admin";


        public static bool Execute(IDbConnection connection, string databaseName)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (databaseName == null) throw new ArgumentNullException(nameof(databaseName));
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT EXISTS(SELECT true FROM pg_database WHERE datname='{databaseName}');";
            var exists = (command.ExecuteScalar() ?? false).Equals(true);
            command.CommandText = new CreateDbScriptRtt(databaseName, exists);
            command.ExecuteNonQuery();
            return (!exists);
        }
    }
}
