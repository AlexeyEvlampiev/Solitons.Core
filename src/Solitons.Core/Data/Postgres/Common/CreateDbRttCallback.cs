using System.Threading.Tasks;

namespace Solitons.Data.Postgres.Common
{
    public abstract class CreateDbRttCallback : ICreateDbRttCallback
    {

        public CreateDbRttCallback(string databaseName)
        {
            DatabaseName = databaseName;
        }

        public string DatabaseName { get; }

        public virtual string DbAdminRole => $"{DatabaseName}_admin";


        public abstract Task<bool> DatabaseAdminRoleExistsAsync();
        public abstract string GeneratePassword(string databaseAdminRole);
        public virtual int GetConnectionLimit(string dbAdminRole) => -1;
    }
}
