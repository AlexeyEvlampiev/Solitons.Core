using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Solitons.Security.Postgres.Scripts
{
    public partial class CreatePgRolesScriptRtt
    {
        private readonly PgRoleBuilder _builder;


        [DebuggerNonUserCode]
        private CreatePgRolesScriptRtt(string databaseName, PgRoleBuilder builder)
        {
            _builder = builder;
            DatabaseName = databaseName;
        }

        public string DatabaseName { get; }

        public string GetRoleFullName(string name) => $"{DatabaseName}_{name}";

        internal IEnumerable<PgRoleBuilder.LoginRole> RolesWithLogin => _builder.LoginRoles;

        internal IEnumerable<PgRoleBuilder.GroupRole> RouleGroups => _builder.GroupRoles;


        internal static void Execute(IDbConnection connection, string databaseName, PgRoleBuilder roles)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (databaseName == null) throw new ArgumentNullException(nameof(databaseName));
            if (roles == null) throw new ArgumentNullException(nameof(roles));
            roles.Assert();

 

            string rtt = new CreatePgRolesScriptRtt(databaseName, roles);
            using var command = connection.CreateCommand();
            command.CommandText = rtt;
            command.ExecuteNonQuery();
        }

        private IEnumerable<PgRoleBuilder.LoginRole> GetLoginMembers(PgRoleBuilder.GroupRole groupRole) => _builder.GetLoginMembers(groupRole);
    }
}
