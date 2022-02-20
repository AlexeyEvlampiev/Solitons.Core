using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;

namespace Solitons.Data
{
    sealed class GenericDbConnectionFactory : IDbConnectionFactory
    {
        sealed record State(
            Func<DbConnectionStringBuilder> ConnectionStringBuilderFactory, 
            Func<string, IDbConnection> ConnectionFactory);

        sealed record Ctors(
            Func<string, DbConnectionStringBuilder> ConnectionStringBuilderFactory,
            Func<string, IDbConnection> ConnectionFactory);

        private static readonly ConcurrentDictionary<Assembly, Ctors> ProvidorFactories = new();
        private readonly State _state;
        private Action<DbConnectionStringBuilder> _transform;


        private GenericDbConnectionFactory(
            State state,
            Action<DbConnectionStringBuilder>? transform = null)
        {
            _state = state;
            _transform = transform;
        }

        public static IDbConnectionFactory Create(DbConnectionStringBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            var factories = ProvidorFactories.GetOrAdd(builder.GetType().Assembly, Discover);
            var metadata = new State(
                () => factories.ConnectionStringBuilderFactory(builder.ConnectionString),
                factories.ConnectionFactory);
            return new GenericDbConnectionFactory(metadata);
        }


        public static IDbConnectionFactory Create<T>(string connectionString) where T : DbConnection
        {
            var factories = ProvidorFactories.GetOrAdd(typeof(T).Assembly, Discover);
            var metadata = new State(
                () => factories.ConnectionStringBuilderFactory(connectionString),
                factories.ConnectionFactory);
            return new GenericDbConnectionFactory(metadata);
        }



        private static Ctors Discover(Assembly assembly)
        {
            var types = assembly
                .GetTypes()
                .Where(type => typeof(DbConnection).IsAssignableFrom(type) ||
                               typeof(DbConnectionStringBuilder).IsAssignableFrom(type))
                .ToList();
            var connectionCtor =
                (from type in types
                 where typeof(DbConnection).IsAssignableFrom(type)
                    let ctor = type.GetConstructor(new[] { typeof(string) })
                    where ctor != null
                    select ctor)
                .FirstOrDefault()
                .ThrowIfNull(() => new NotSupportedException($"{assembly} is not supported."));
            var connectionStringBuilderCtor =
                (from type in types
                 where typeof(DbConnectionStringBuilder).IsAssignableFrom(type)
                    let ctor = type.GetConstructor(new[] { typeof(string) })
                    where ctor is not null
                    select ctor)
                .FirstOrDefault()
                .ThrowIfNull(() => new NotSupportedException($"{assembly} is not supported."));

            DbConnectionStringBuilder CreateBuilder(string connectionString)
            {
                var instance = connectionStringBuilderCtor.Invoke(new object[] { connectionString });
                return (DbConnectionStringBuilder)instance;
            }

            DbConnection CreateConnection(string connectionString)
            {
                var instance = connectionCtor.Invoke(new object[] { connectionString });
                return (DbConnection)instance;
            }

            return new Ctors(CreateBuilder, CreateConnection);
        }

        private static DbConnectionStringBuilder CreateBuilder(string template, ConstructorInfo ctor)
        {
            var instance = ctor.Invoke(new object[] { template });
            return (DbConnectionStringBuilder)instance;
        }

        private static IDbConnection CreateConnection(string connectionString, ConstructorInfo ctor)
        {
            var instance = ctor.Invoke(new object[] { connectionString });
            return (IDbConnection)instance;
        }

        [DebuggerStepThrough]
        public IDbConnectionFactory WithDatabase(string databaseName)
        {
            return Create(builder =>
            {
                try
                {
                    dynamic obj = builder;
                    obj.Database = databaseName;
                }
                catch (RuntimeBinderException)
                {
                    throw new NotSupportedException();
                }
            });
        }

        [DebuggerStepThrough]
        public IDbConnectionFactory WithUsername(string username)
        {
            return Create(builder =>
            {
                try
                {
                    dynamic obj = builder;
                    obj.Username = username;
                }
                catch (RuntimeBinderException)
                {
                    throw new NotSupportedException();
                }
            });
        }

        [DebuggerStepThrough]
        public IDbConnectionFactory WithPassword(string password)
        {
            return Create(builder =>
            {
                try
                {
                    dynamic obj = builder;
                    obj.Password = password;
                }
                catch (RuntimeBinderException)
                {
                    throw new NotSupportedException();
                }
            });
        }

        public IDbConnection CreateConnection()
        {
            var builder = _state.ConnectionStringBuilderFactory.Invoke();
            _transform?.Invoke(builder);
            var connectionString = builder.ConnectionString;
            return _state.ConnectionFactory.Invoke(connectionString);
        }

        public string ConnectionString
        {
            [DebuggerStepThrough]
            get => ToString();
        }


        private GenericDbConnectionFactory Create(Action<DbConnectionStringBuilder> transform)
        {
            transform = _transform is null
                ? transform
                : _transform + transform;
            return new GenericDbConnectionFactory(_state, transform);
        }

        public override string ToString()
        {
            var builder = _state.ConnectionStringBuilderFactory.Invoke();
            _transform?.Invoke(builder);
            return builder.ConnectionString;
        }
    }
}
