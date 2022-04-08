using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Solitons.Security.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SecretsRepository : ISecretsRepository
    {
        protected abstract Task<string> GetSecretAsync(string secretName);
        protected abstract Task<string?> TryGetSecretAsync(string secretName);
        protected abstract Task<string> GetOrSetSecretAsync(string secretName, string defaultValue);
        protected abstract Task SetSecretAsync(string secretName, string secretValue);
        protected abstract bool IsSecretNotFoundError(Exception exception);

        protected virtual bool IsValidSecretName(string secretName) => (false == string.IsNullOrWhiteSpace(secretName));

        Task<string> ISecretsRepository.GetSecretAsync(string secretName)
        {
            if (false == IsValidSecretName(secretName))
                throw new ArgumentException($"'{secretName}' is not a valid secret name.");
            return GetSecretAsync(secretName);
        }

        async Task<string?> ISecretsRepository.TryGetSecretAsync(string secretName)
        {
            return IsValidSecretName(secretName) 
                ? await TryGetSecretAsync(secretName) 
                : null;
        }

        Task<string> ISecretsRepository.GetOrSetSecretAsync(string secretName, string defaultValue)
        {
            if (false == IsValidSecretName(secretName))
                throw new ArgumentException($"'{secretName}' is not a valid secret name.");

            return GetOrSetSecretAsync(
                secretName, 
                defaultValue.ThrowIfNullOrWhiteSpaceArgument(nameof(defaultValue)));
        }

        Task ISecretsRepository.SetSecretAsync(string secretName, string secretValue)
        {
            if (false == IsValidSecretName(secretName))
                throw new ArgumentException($"'{secretName}' is not a valid secret name.");
            return SetSecretAsync(
                secretName, 
                secretValue.ThrowIfNullOrWhiteSpaceArgument(nameof(secretValue)));
        }

        [DebuggerStepThrough]
        bool ISecretsRepository.IsSecretNotFoundError(Exception exception) => IsSecretNotFoundError(exception);
    }
}
