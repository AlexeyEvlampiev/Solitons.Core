using System.Data;
using System.Data.Common;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IDbConnectionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        IDbConnectionFactory WithDatabase(string databaseName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        IDbConnectionFactory WithUsername(string username);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        IDbConnectionFactory WithPassword(string password);

        IDbConnection CreateConnection();

        /// <summary>
        /// 
        /// </summary>
        string ConnectionString { get; }
    }

    public partial interface IDbConnectionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDbConnectionFactory CreateGeneric(DbConnectionStringBuilder builder) => GenericDbConnectionFactory
            .Create(builder.ThrowIfNullArgument(nameof(builder)));
    }
}
