using System;
using System.Collections.Generic;
using System.Linq;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct)]
    public abstract class RoleAuthorizationAttribute : Attribute
    {
        private readonly HashSet<string> _roles = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roles"></param>
        protected RoleAuthorizationAttribute(IEnumerable<string> roles)
        {
            _roles.AddRange(roles);
        }

        public IEnumerable<string> Roles => _roles.AsEnumerable();

        public override string ToString() => _roles.Join(",");
    }
}
