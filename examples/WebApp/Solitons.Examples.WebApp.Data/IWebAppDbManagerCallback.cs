

using Solitons.Examples.WebApp.Data.Model;

namespace Solitons.Examples.WebApp.Data;

public interface IWebAppDbManagerCallback
{
    void OnOperationRetry(string operation);
    void OnOperationFailed(string operation, Exception error);
    void OnCreatingDatabase(string databaseName, PgServerInfo pgServerInfo);
}