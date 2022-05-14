using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Security.Postgres
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PgRolesBuilder
    {
        internal sealed record Membership(PgLoginRole LoginRole, PgGroupRole GroupRole);

        private readonly HashSet<PgRole> _roles = new();
        private readonly HashSet<Membership> _membership = new();
        private readonly HashSet<string> _roleNames = new(StringComparer.OrdinalIgnoreCase);

        internal PgRolesBuilder()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="groupRoles">Membership groups</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [DebuggerNonUserCode]
        public PgLoginRole AddLoginRole(string roleName, params PgGroupRole[] groupRoles)
        {
            var loginRole = new PgLoginRole(roleName);
            Register(loginRole);
            foreach (var groupRole in groupRoles)
            {
                if (false == _roles.Contains(groupRole))
                {
                    throw new InvalidOperationException($"The specified {groupRole.Name} entry is originated from a different {typeof(PgRolesBuilder)} instance");
                }
                _membership.Add(new Membership(loginRole, groupRole));
            }
            return loginRole;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public PgGroupRole AddGroupRole(string roleName)
        {
            var role = new PgGroupRole(roleName);
            Register(role);
            return role;
        }

        private void Register(PgRole role)
        {
            if (_roleNames.Add(role.Name))
            {
                _roles.Add(role);
            }
            else
            {
                throw new InvalidOperationException($"'{role.Name}' role is registered already.");
            }
        }




        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        public IEnumerable<PgRole> Roles => _roles.AsEnumerable();

        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        public IEnumerable<PgLoginRole>  LoginRoles => _roles.OfType<PgLoginRole>();

        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        public IEnumerable<PgGroupRole> GroupRoles => _roles.OfType<PgGroupRole>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupRole"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public IEnumerable<PgLoginRole> GetLoginMembers(PgGroupRole groupRole) => _membership
            .Where(m => m.GroupRole == groupRole)
            .Select(m => m.LoginRole);
    }
}
