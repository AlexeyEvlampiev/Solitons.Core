namespace Solitons.Security.Postgres.Scripts
{
    public partial class DropRolesByPrefixScriptRtt
    {
        internal DropRolesByPrefixScriptRtt(string prefix)
        {
            RolePrefix = prefix;
        }

        internal string RolePrefix { get; }
    }
}
