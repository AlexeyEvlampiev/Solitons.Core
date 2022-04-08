using System;
using System.Threading.Tasks;

namespace Solitons.Security
{
    public interface ISecretsRepository
    {
        Task<string> GetSecretAsync(string secretName);
        Task<string?> TryGetSecretAsync(string secretName);
        Task<string> GetOrSetSecretAsync(string secretName, string defaultValue);

        Task SetSecretAsync(string secretName, string secretValue);

        bool IsSecretNotFoundError(Exception exception);
    }
}
