using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security.Common;

/// <summary>
/// A proxy for <see cref="ISecretsRepository"/> that delegates all calls to an underlying inner repository.
/// It can be extended to add additional behaviors, such as read-through caching, while keeping the original repository intact.
/// </summary>
public abstract class SecretsRepositoryProxy : ISecretsRepository
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly ISecretsRepository _innerRepository;

    /// <summary>
    /// Constructs a new instance of <see cref="SecretsRepositoryProxy"/> with the provided inner repository.
    /// </summary>
    /// <param name="innerRepository">The underlying repository that the proxy delegates to.</param>
    [DebuggerNonUserCode]
    protected SecretsRepositoryProxy(ISecretsRepository innerRepository)
    {
        _innerRepository = innerRepository;
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public sealed override string ToString() => _innerRepository.ToString()!;

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public sealed override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj is SecretsRepositoryProxy other)
            return _innerRepository.Equals(other._innerRepository);
        return _innerRepository.Equals(obj);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public sealed override int GetHashCode() => _innerRepository.GetHashCode();

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual Task<string[]> ListSecretNamesAsync(CancellationToken cancellation = default)
    {
        return _innerRepository.ListSecretNamesAsync(cancellation);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual Task<string> GetSecretAsync(string secretName, CancellationToken cancellation = default)
    {
        return _innerRepository.GetSecretAsync(secretName, cancellation);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual Task<string?> GetSecretIfExistsAsync(string secretName, CancellationToken cancellation = default)
    {
        return _innerRepository.GetSecretIfExistsAsync(secretName, cancellation);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual Task<string> GetOrSetSecretAsync(string secretName, string defaultValue, CancellationToken cancellation = default)
    {
        return _innerRepository.GetOrSetSecretAsync(secretName, defaultValue, cancellation);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual Task SetSecretAsync(string secretName, string secretValue, CancellationToken cancellation = default)
    {
        return _innerRepository.SetSecretAsync(secretName, secretValue, cancellation);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual bool IsSecretNotFoundError(Exception exception)
    {
        return _innerRepository.IsSecretNotFoundError(exception);
    }
}
