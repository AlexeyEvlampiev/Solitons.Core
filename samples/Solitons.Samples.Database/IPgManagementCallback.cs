
namespace Solitons.Samples.Database
{
    public interface IPgManagementCallback
    {
        string GetDatabaseName();
        string GetDatabaseAdminRoleName();
        string? GetConnectionStringIfExists(string loginRoleName);

        string GenerateRandomPassword();

        void SetConnectionString(string dbAdminRole, string connectionString);
    }

}
