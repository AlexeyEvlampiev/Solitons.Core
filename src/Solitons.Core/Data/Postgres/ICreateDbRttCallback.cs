using System.Threading.Tasks;

namespace Solitons.Data.Postgres
{
    public interface ICreateDbRttCallback
    {
        Task<bool> DatabaseAdminRoleExistsAsync();
        string GeneratePassword(string databaseAdminRole);
        string DbAdminRole { get; }
        string DatabaseName { get; }
        int GetConnectionLimit(string dbAdminRole);
    }
}
