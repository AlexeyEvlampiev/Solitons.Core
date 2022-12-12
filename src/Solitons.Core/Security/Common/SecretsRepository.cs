using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SecretsRepository : ISecretsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<string[]> ListSecretNamesAsync(CancellationToken cancellation);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        protected abstract Task<string> GetSecretAsync(string secretName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        protected abstract Task<string?> GetSecretIfExistsAsync(string secretName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected abstract Task<string> GetOrSetSecretAsync(string secretName, string defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <param name="secretValue"></param>
        /// <returns></returns>
        protected abstract Task SetSecretAsync(string secretName, string secretValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected abstract bool IsSecretNotFoundError(Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        protected virtual bool IsValidSecretName(string secretName) => secretName.IsPrintable();

        [DebuggerStepThrough]
        Task<string[]> ISecretsRepository.ListSecretNamesAsync(CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return ListSecretNamesAsync(cancellation);
        }


        [DebuggerStepThrough]
        Task<string> ISecretsRepository.GetSecretAsync(string secretName)
        {
            if (false == IsValidSecretName(secretName))
                throw new ArgumentException($"'{secretName}' is not a valid secret name.");
            return GetSecretAsync(secretName);
        }

        [DebuggerStepThrough]
        async Task<string?> ISecretsRepository.GetSecretIfExistsAsync(string secretName)
        {
            return IsValidSecretName(secretName) 
                ? await GetSecretIfExistsAsync(secretName) 
                : null;
        }

        [DebuggerStepThrough]
        Task<string> ISecretsRepository.GetOrSetSecretAsync(string secretName, string defaultValue)
        {
            if (false == IsValidSecretName(secretName))
                throw new ArgumentException($"'{secretName}' is not a valid secret name.");

            return GetOrSetSecretAsync(
                secretName, 
                ThrowIf.NullOrWhiteSpaceArgument(defaultValue, nameof(defaultValue)));
        }

        [DebuggerStepThrough]
        Task ISecretsRepository.SetSecretAsync(string secretName, string secretValue)
        {
            if (false == IsValidSecretName(secretName))
                throw new ArgumentException($"'{secretName}' is not a valid secret name.");
            return SetSecretAsync(
                secretName, 
                ThrowIf.NullOrWhiteSpaceArgument(secretValue, nameof(secretValue)));
        }

        [DebuggerStepThrough]
        bool ISecretsRepository.IsSecretNotFoundError(Exception exception) => IsSecretNotFoundError(exception);
    }
}
