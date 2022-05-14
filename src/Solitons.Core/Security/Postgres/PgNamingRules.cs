namespace Solitons.Security.Postgres
{
    /// <summary>
    /// 
    /// </summary>
    public class PgNamingRules
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public virtual string BuildRoleFullName(string databaseName, string roleName) => $"{databaseName}_{roleName}";
    }
}
