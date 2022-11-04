namespace Solitons.Samples.Database
{
    internal sealed record LoginRoleInfo(string RoleName, string SecretName)
    {
        public int ConnectionLimit { get; init; } = 10;
    }
}
