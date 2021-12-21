using Solitons.Security.Common;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GenericDbRoleAttribute : DbRoleAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public GenericDbRoleAttribute(string name) : base(name)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public GenericDbRoleAttribute(string name, string description) : base(name, description)
        {
        }
    }
}
