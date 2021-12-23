namespace Solitons.Security.Postgres
{
    public interface IPgRoleBuilder
    {
        IPgRoleBuilder WithLoginRole(string roleName, int connectionLimit = -1);
        IPgRoleBuilder WithGroupRole(string roleName);
        IPgRoleBuilder WithMembership(string loginRole, string groupRoule);
    }
}
