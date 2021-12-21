using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Security.Postgres
{
    public partial class CreatePgRolesScriptRtt
    {
        private readonly RoleSetBuilder _builder;

        internal sealed record RoleWithLogin(string Name, int ConnectionLimit);
        internal sealed record GroupRole(string Name);


        [DebuggerNonUserCode]
        private CreatePgRolesScriptRtt(string databaseName, RoleSetBuilder builder)
        {
            _builder = builder;
            DatabaseName = databaseName;
        }

        public sealed class RoleSetBuilder
        {
            private readonly List<RoleWithLogin> _rolesWithLogin = new();
            private readonly List<GroupRole> _groupGroupRoles = new();
            private readonly HashSet<KeyValuePair<RoleWithLogin, GroupRole>> _membership = new();
            private readonly StringComparer _comparer = StringComparer.Ordinal;

            internal RoleSetBuilder()
            {
            }

            public RoleSetBuilder WithLogin(string roleName, int connectionLimit = -1)
            {
                _rolesWithLogin.Add(new RoleWithLogin(roleName, connectionLimit));
                return this;
            }

            public RoleSetBuilder WithGroupRole(string roleName)
            {
                _groupGroupRoles.Add(new GroupRole(roleName));
                return this;
            }

            public RoleSetBuilder WithMembership(string login, string roleName)
            {
                var loginRole = _rolesWithLogin
                    .SingleOrDefault(r => _comparer.Equals(r.Name, login))
                    .ThrowIfNull(()=> new InvalidOperationException($"{login} - role with login is not registered."));

                var groupRole = _groupGroupRoles
                    .SingleOrDefault(r => _comparer.Equals(r.Name, roleName))
                    .ThrowIfNull(() => new InvalidOperationException($"{roleName} - group role is not registered."));
                _membership.Add(KeyValuePair.Create(loginRole, groupRole));
                return this;
            }

            internal IEnumerable<RoleWithLogin> LoginRoles => _rolesWithLogin;

            internal IEnumerable<GroupRole> GroupRoles => _groupGroupRoles;

            private Dictionary<RoleWithLogin, GroupRole[]> Membership
            {
                get => throw new NotImplementedException();
            }


            internal IEnumerable<RoleWithLogin> GetLoginMembers(GroupRole groupRole)
            {
                return _membership
                    .Where(p => p.Value == groupRole)
                    .Select(p => p.Key);
            }
        }

        public string DatabaseName { get; }

        public string GetRoleFullName(string name) => $"{DatabaseName}_{name}";

        internal IEnumerable<RoleWithLogin> RolesWithLogin => _builder.LoginRoles;

        internal IEnumerable<GroupRole> RouleGroups => _builder.GroupRoles;


        public static void Execute(IDbConnection connection, string databaseName, Action<RoleSetBuilder> config)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (databaseName == null) throw new ArgumentNullException(nameof(databaseName));
     

            var builder = new RoleSetBuilder();
            config(builder);
            var rtt = new CreatePgRolesScriptRtt(databaseName, builder);
            using var command = connection.CreateCommand();
            command.CommandText = rtt;
            command.ExecuteNonQuery();
        }

        private IEnumerable<RoleWithLogin> GetLoginMembers(GroupRole groupRole) => _builder.GetLoginMembers(groupRole);
    }
}
