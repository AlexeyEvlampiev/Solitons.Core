namespace Solitons.Security.Postgres
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PgLoginRole : PgRole
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public PgLoginRole(string name) : base(name)
        {
            ConnectionLimit = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ConnectionLimit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PgLoginRole WithConnectionLimit(int value)
        {
            ConnectionLimit = value.ThrowIfArgumentLessThan(-1, nameof(value));
            return this;
        }
    }
}
