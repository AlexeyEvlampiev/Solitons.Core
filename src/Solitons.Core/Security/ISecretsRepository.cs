using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface ISecretsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        Task<string> GetSecretAsync(string secretName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        Task<string?> GetSecretIfExistsAsync(string secretName);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        Task<string> GetOrSetSecretAsync(string secretName, string defaultValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <param name="secretValue"></param>
        /// <returns></returns>
        Task SetSecretAsync(string secretName, string secretValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        bool IsSecretNotFoundError(Exception exception);
    }

    public partial interface ISecretsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [Obsolete($"Use {nameof(GetSecretIfExistsAsync)} instead", true)]
        public Task<string?> TryGetSecretAsync(string secretName) => GetSecretIfExistsAsync(secretName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheExpiration"></param>
        /// <param name="secretNameComparer"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [DebuggerStepThrough]
        public ISecretsRepository ReadThroughCache(
            IObservable<Unit> cacheExpiration,
            StringComparer secretNameComparer)
        {
            if (this is ReadThroughCacheSecretsRepository)
            {
                throw new InvalidOperationException($"Multilayered cache not supported.");
            }

            return new ReadThroughCacheSecretsRepository(this, cacheExpiration, secretNameComparer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretNameComparer"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public ISecretsRepository ReadThroughCache(
            StringComparer secretNameComparer) =>
            ReadThroughCache(Observable.Empty<Unit>(), secretNameComparer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheExpiration"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public ISecretsRepository ReadThroughCache(
            IObservable<Unit> cacheExpiration) =>
            ReadThroughCache(cacheExpiration, StringComparer.Ordinal);
    }
}
