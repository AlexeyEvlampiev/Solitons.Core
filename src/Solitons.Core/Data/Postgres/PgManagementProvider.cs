using System;
using System.Data;

namespace Solitons.Data.Postgres
{
    public abstract class PgManagementProvider
    {
        protected abstract IDbConnection CreateConnection();


        public bool DatabaseExists(string databaseName)
        {
            using var connection = CreateConnection();
            using var command = connection.CreateCommand();
            command.CommandText = $@"SELECT true FROM pg_database WHERE datname = '{databaseName}';";
            connection.Open();
            var result = command.ExecuteScalar();
            return (result is bool exists && exists);
        }

        public bool CreateDatabaseIfNotExists(string databaseName, string ownerRole)
        {
            if (DatabaseExists(databaseName))
                return false;
            using var connection = CreateConnection();
            using var command = connection.CreateCommand();
            command.CommandText = $@"CREATE DATABASE {databaseName} WITH OWNER = {ownerRole};";
            command.CommandTimeout = Convert.ToInt32(TimeSpan.FromMinutes(5).TotalSeconds);
            connection.Open();
            command.ExecuteNonQuery();
            return true;
        }
    }
}
