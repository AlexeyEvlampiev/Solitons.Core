using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security.Common;

/// <summary>
/// Provides the abstract base class for a repository of secrets, such as passwords or API keys.
/// Derived classes are expected to provide concrete implementations of the methods.
/// </summary>
public abstract class SecretsRepository : ISecretsRepository
{
    /// <summary>
    /// Asynchronously returns an array of names of all secrets in the repository.
    /// Derived classes must provide a concrete implementation.
    /// </summary>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains an array of names of all secrets in the repository.</returns>
    protected abstract Task<string[]> ListSecretNamesAsync(CancellationToken cancellation);

    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name.
    /// Derived classes must provide a concrete implementation.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains the value of the secret with the specified name.</returns>
    protected abstract Task<string> GetSecretAsync(string secretName);

    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name, if it exists in the repository.
    /// Derived classes must provide a concrete implementation.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains the value of the secret with the specified name, or <c>null</c> if it does not exist in the repository.</returns>
    protected abstract Task<string?> GetSecretIfExistsAsync(string secretName);

    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name, or sets it to the specified default value if it does not exist in the repository.
    /// Derived classes must provide a concrete implementation.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="defaultValue">The default value to set if the secret does not exist.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains the value of the secret with the specified name, or the default value if it does not exist in the repository.</returns>
    protected abstract Task<string> GetOrSetSecretAsync(string secretName, string defaultValue);

    /// <summary>
    /// Asynchronously sets the value of the secret with the specified name.
    /// Derived classes must provide a concrete implementation.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="secretValue">The value of the secret.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected abstract Task SetSecretAsync(string secretName, string secretValue);

    /// <summary>
    /// Determines if the specified exception is a "secret not found" error.
    /// Derived classes must provide a concrete implementation.
    /// </summary>
    /// <param name="exception">The exception to check.</param>
    /// <returns><c>true</c> if the exception is a "secret not found" error; otherwise, <c>false</c>.</returns>
    protected abstract bool IsSecretNotFoundError(Exception exception);

    /// <summary>
    /// Determines if the provided secret name is valid. A secret name is considered valid if it is printable.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <returns><c>true</c> if the secret name is valid; otherwise, <c>false</c>.</returns>
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
            ThrowIf.ArgumentNullOrWhiteSpace(defaultValue, nameof(defaultValue)));
    }

    [DebuggerStepThrough]
    Task ISecretsRepository.SetSecretAsync(string secretName, string secretValue)
    {
        if (false == IsValidSecretName(secretName))
            throw new ArgumentException($"'{secretName}' is not a valid secret name.");
        return SetSecretAsync(
            secretName, 
            ThrowIf.ArgumentNullOrWhiteSpace(secretValue, nameof(secretValue)));
    }

    [DebuggerStepThrough]
    bool ISecretsRepository.IsSecretNotFoundError(Exception exception) => IsSecretNotFoundError(exception);
}