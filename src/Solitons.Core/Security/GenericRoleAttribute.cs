using Solitons.Security.Common;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GenericRoleAttribute : RoleAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public GenericRoleAttribute(string name) : base(name)
        {
        }
    }
}
