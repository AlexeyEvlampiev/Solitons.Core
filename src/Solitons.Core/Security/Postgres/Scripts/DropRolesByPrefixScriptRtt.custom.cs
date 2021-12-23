using System;
using System.Data;

namespace Solitons.Security.Postgres.Scripts
{
    public partial class DropRolesByPrefixScriptRtt
    {
        private DropRolesByPrefixScriptRtt(string prefix)
        {
            RolePrefix = prefix;
        }

        public string RolePrefix { get; }

        internal static void Execute(IDbConnection connection, string prefix)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (prefix == null) throw new ArgumentNullException(nameof(prefix));

            var rtt = new DropRolesByPrefixScriptRtt(prefix);
            using var command = connection.CreateCommand();
            command.CommandText = rtt;
            command.ExecuteNonQuery();
        }
    }
}
