using System;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Solitons.Data.Management.Postgres
{
    public abstract class PgConnectionFactory
    {
        protected abstract PgConnectionFactory Clone();

        public abstract string ConnectionString {get;}

        public abstract IDbConnection CreateConnection();

        /// <summary>
        /// The hostname or IP address of the PostgreSQL server to connect to.
        /// </summary>
        public abstract string Host { get; protected set; }

        /// <summary>
        /// The PostgreSQL database to connect to.
        /// </summary>
        public abstract string Database { get; protected set; }

        /// <summary>
        /// The username to connect with. Not required if using IntegratedSecurity.
        /// </summary>
        public abstract string Username { get; protected set; }

        /// <summary>
        /// The password to connect with. Not required if using IntegratedSecurity.
        /// </summary>
        public abstract string Password { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public abstract bool IsValidConnectionString(string? connectionString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public PgConnectionFactory WithHost(string host)
        {
            var clone = Clone();
            clone.Host = host
                .DefaultIfNullOrWhiteSpace("localhost")
                .Trim();
            return clone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public PgConnectionFactory WithDatabase(string database)
        {
            var clone = Clone();
            clone.Database = database
                .DefaultIfNullOrWhiteSpace("postgres")
                .Trim();
            return clone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public PgConnectionFactory WithUsername(string username)
        {
            var clone = Clone();
            clone.Username = username
                .DefaultIfNullOrWhiteSpace("postgres")
                .Trim();
            return clone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public PgConnectionFactory WithPassword(string password)
        {
            var clone = Clone();
            clone.Password = password;
            return clone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public abstract PgConnectionFactory WithConnectionString(string connectionString);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expectedHost"></param>
        /// <param name="expectedDatabase"></param>
        /// <param name="expectedUsername"></param>
        /// <returns></returns>
        public bool TestConnection(
            string? expectedHost = null, 
            string? expectedDatabase = null, 
            string? expectedUsername = null)
        {
            if (expectedHost.IsPrintable() && false == Host.Equals(expectedHost, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (expectedDatabase.IsPrintable() && false == Database.Equals(expectedDatabase, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (expectedUsername.IsPrintable() && false == Username.Equals(expectedUsername, StringComparison.Ordinal))
            {
                return false;
            }

            try
            {
                using var connection = CreateConnection();
                connection.Open();
                return true;
            }
            catch (Exception e) when(Regex.IsMatch(e.Message, @"(?xis)\b28P01"))
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onConnected"></param>
        [DebuggerStepThrough]
        public void Connect(Action<IDbConnection> onConnected)
        {
            using var connection = CreateConnection();
            connection.Open();
            onConnected.Invoke(connection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        [DebuggerStepThrough]
        public void Do(Action<IDbCommand> action)
        {
            using var connection = CreateConnection();
            using var command = connection.CreateCommand();
            connection.Open();
            action.Invoke(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <returns></returns>
        public T Do<T>(Func<IDbCommand, T> handler)
        {
            using var connection = CreateConnection();
            using var command = connection.CreateCommand();
            connection.Open();
            return handler.Invoke(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public T Do<T>(string commandText, Func<IDbCommand, T> handler)
        {
            using var connection = CreateConnection();
            using var command = connection.CreateCommand();
            command.CommandText = commandText;
            connection.Open();
            return handler.Invoke(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sealed override string ToString() => ConnectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static implicit operator string(PgConnectionFactory factory) => factory.ConnectionString;
    }
}
