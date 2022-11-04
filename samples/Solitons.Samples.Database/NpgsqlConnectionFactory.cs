using System.Data;
using System.Diagnostics;
using Npgsql;
using Solitons.Data.Management.Postgres;

namespace Solitons.Samples.Database
{
    public sealed class NpgsqlConnectionFactory : PgConnectionFactory
    {
        private readonly NpgsqlConnectionStringBuilder _builder;

        public NpgsqlConnectionFactory(string connectionString)
        {
            _builder = new NpgsqlConnectionStringBuilder(connectionString);
        }
        protected override PgConnectionFactory Clone() => new NpgsqlConnectionFactory(_builder.ConnectionString);

        public override string ConnectionString => _builder.ConnectionString;

        public override IDbConnection CreateConnection() => new NpgsqlConnection(ConnectionString);

        public override string Host
        {
            get => _builder.Host.DefaultIfNullOrWhiteSpace("localhost");
            protected set => _builder.Host = value;
        }

        public override string Database
        {
            get => _builder.Database.DefaultIfNullOrWhiteSpace("postgres");
            protected set => _builder.Database = value;
        }

        public override string Username
        {
            get => _builder.Username.DefaultIfNullOrWhiteSpace("postgres");
            protected set => _builder.Username = value;
        }

        public override string Password
        {
            get => _builder.Password.DefaultIfNullOrWhiteSpace(String.Empty);
            protected set => _builder.Password = value;
        }


        public override bool IsValidConnectionString(string? connectionString)
        {
            if (connectionString.IsNullOrWhiteSpace())
            {
                return false;
            }
            try
            {
                var _ = new NpgsqlConnectionStringBuilder(connectionString).ConnectionString;
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public override PgConnectionFactory WithConnectionString(string connectionString) => new NpgsqlConnectionFactory(connectionString);
    }
}
