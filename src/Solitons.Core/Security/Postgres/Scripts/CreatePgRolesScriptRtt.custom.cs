using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Solitons.Security.Postgres.Scripts
{
    public partial class CreatePgRolesScriptRtt
    {
        private readonly PgRolesBuilder _builder;
        private readonly PgNamingRules _namingRules;


        [DebuggerNonUserCode]
        internal CreatePgRolesScriptRtt(string databaseName, PgRolesBuilder builder, PgNamingRules namingRules)
        {
            _builder = builder;
            _namingRules = namingRules;
            DatabaseName = databaseName;
        }

        internal string DatabaseName { get; }

        internal string GetRoleFullName(string name) => _namingRules.BuildRoleFullName(DatabaseName, name);

        internal IEnumerable<PgLoginRole> LoginRoles => _builder.LoginRoles;

        internal IEnumerable<PgGroupRole> GroupRoles => _builder.GroupRoles;



        private IEnumerable<PgLoginRole> GetLoginMembers(PgGroupRole groupRole) => _builder.GetLoginMembers(groupRole);
    }
}
