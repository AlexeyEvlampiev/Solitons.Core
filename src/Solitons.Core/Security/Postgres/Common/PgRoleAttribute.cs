using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Security.Postgres.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class PgRoleAttribute : Attribute, IPgRoleAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="options"></param>
        protected PgRoleAttribute(string roleName, CreateRoleOptions options)
        {
            RoleName = roleName.ThrowIfNullOrWhiteSpaceArgument(nameof(roleName));
            Options = options;
        }

        public string RoleName { get; }

        public CreateRoleOptions Options { get; }

        public int ConnectionLimit { get; set; }

        public string[] InRoles { get; set; }

        public string[] MemberRoles { get; set; }

        public string AdminRole { get; set; }
    }
}
