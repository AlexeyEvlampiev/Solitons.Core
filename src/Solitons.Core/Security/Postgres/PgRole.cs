namespace Solitons.Security.Postgres
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PgRole
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        protected PgRole(string name)
        {
            Name = name
                .ThrowIfNullOrWhiteSpaceArgument(nameof(name))
                .Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }
    }
}
