namespace Solitons.Security.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DbRoleAttribute : RoleAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        protected DbRoleAttribute(string name, string description) : base(name)
        {
            Description = description
                .ThrowIfNullOrWhiteSpaceArgument(nameof(description))
                .Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DbRoleAttribute(string name) 
            : this(name, name)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; }
    }
}
