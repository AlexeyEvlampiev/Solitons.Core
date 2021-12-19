using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Security.Postgres
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPgRoleAttribute
    {
        /// <summary>
        /// The role name.
        /// </summary> 
        string RoleName { get; } 

        /// <summary>
        /// Create role options
        /// </summary>
        CreateRoleOptions Options { get; } 

        /// <summary>
        /// If role can log in, this specifies how many concurrent connections the role can make. -1 (the default) means no limit. Note that only normal connections are counted towards this limit. Neither prepared transactions nor background worker connections are counted towards this limit.
        /// </summary>
        int ConnectionLimit { get; }

        /// <summary>
        /// The IN ROLE clause lists one or more existing roles to which this role will be immediately added as a new member. (Note that there is no option to add the new role as an administrator; use a separate GRANT command to do that.)
        /// </summary>
        string[] InRoles { get; }

        /// <summary>
        /// The ROLE clause lists one or more existing roles which are automatically added as members of the new role. (This in effect makes the new role a “group”.)
        /// </summary>
        string[] MemberRoles { get; }

        /// <summary>
        /// The ADMIN clause is like ROLE, but the named roles are added to the new role WITH ADMIN OPTION, giving them the right to grant membership in this role to others.
        /// </summary>
        string AdminRole { get; }
    }
}
