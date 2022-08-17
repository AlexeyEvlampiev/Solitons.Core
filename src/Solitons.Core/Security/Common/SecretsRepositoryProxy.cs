using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class SecretsRepositoryProxy : ISecretsRepository
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly ISecretsRepository _innerRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerRepository"></param>
        [DebuggerNonUserCode]
        protected SecretsRepositoryProxy(ISecretsRepository innerRepository)
        {
            _innerRepository = innerRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public sealed override string ToString() => _innerRepository.ToString()!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public sealed override bool Equals(object? obj)
        {
            if(obj == null)return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is SecretsRepositoryProxy other) 
                return _innerRepository.Equals(other._innerRepository);
            return _innerRepository.Equals(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public sealed override int GetHashCode() => _innerRepository.GetHashCode();

        [DebuggerStepThrough]
        public virtual Task<string[]> ListSecretNamesAsync(CancellationToken cancellation = default)
        {
            return _innerRepository.ListSecretNamesAsync(cancellation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        public virtual Task<string> GetSecretAsync(string secretName)
        {
            return _innerRepository.GetSecretAsync(secretName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        public virtual Task<string?> GetSecretIfExistsAsync(string secretName)
        {
            return _innerRepository.GetSecretIfExistsAsync(secretName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual Task<string> GetOrSetSecretAsync(string secretName, string defaultValue)
        {
            return _innerRepository.GetOrSetSecretAsync(secretName, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretName"></param>
        /// <param name="secretValue"></param>
        /// <returns></returns>
        public virtual Task SetSecretAsync(string secretName, string secretValue)
        {
            return _innerRepository.SetSecretAsync(secretName, secretValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public virtual bool IsSecretNotFoundError(Exception exception)
        {
            return _innerRepository.IsSecretNotFoundError(exception);
        }
    }
}
