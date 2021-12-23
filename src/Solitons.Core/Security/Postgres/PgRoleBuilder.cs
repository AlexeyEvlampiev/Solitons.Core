using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Security.Postgres
{

    internal class PgRoleBuilder : IPgRoleBuilder
    {
        internal sealed record LoginRole(string Name, int ConnectionLimit);
        internal sealed record GroupRole(string Name);

        internal sealed record Membership(LoginRole LoginRole, GroupRole GroupRole);

        private readonly List<LoginRole> _rolesWithLogin = new();
        private readonly List<GroupRole> _groupGroupRoles = new();
        private readonly HashSet<Membership> _membership = new();
        private readonly StringComparer _comparer = StringComparer.Ordinal;


        [DebuggerNonUserCode]
        public IPgRoleBuilder WithLoginRole(string roleName, int connectionLimit = -1)
        {
            _rolesWithLogin.Add(new LoginRole(roleName, connectionLimit));
            return this;
        }

        [DebuggerNonUserCode]
        public IPgRoleBuilder WithGroupRole(string roleName)
        {
            _groupGroupRoles.Add(new GroupRole(roleName));
            return this;
        }

        [DebuggerNonUserCode]
        public IPgRoleBuilder WithMembership(string loginRoleName, string groupRoleName)
        {
            var loginRole = _rolesWithLogin
                .SingleOrDefault(r => _comparer.Equals(r.Name, loginRoleName))
                .ThrowIfNull(() => new InvalidOperationException($"{loginRoleName} - role with login is not registered."));

            var groupRole = _groupGroupRoles
                .SingleOrDefault(r => _comparer.Equals(r.Name, groupRoleName))
                .ThrowIfNull(() => new InvalidOperationException($"{groupRoleName} - group role is not registered."));
            _membership.Add(new Membership(loginRole, groupRole));
            return this;
        }

        [DebuggerNonUserCode]
        internal IEnumerable<LoginRole>  LoginRoles => _rolesWithLogin;

        [DebuggerNonUserCode]
        internal IEnumerable<GroupRole> GroupRoles => _groupGroupRoles;

        [DebuggerNonUserCode]
        internal IEnumerable<LoginRole> GetLoginMembers(GroupRole groupRole) => _membership
            .Where(m => m.GroupRole == groupRole)
            .Select(m => m.LoginRole);

        internal void Assert()
        {

        }
    }
}
