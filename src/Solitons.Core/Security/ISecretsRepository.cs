using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security;

/// <summary>
/// Represents a repository of secrets, such as passwords or API keys.
/// </summary>
public partial interface ISecretsRepository
{
    /// <summary>
    /// Asynchronously returns an array of names of all secrets in the repository.
    /// </summary>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>An array of names of all secrets in the repository.</returns>
    Task<string[]> ListSecretNamesAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <returns>The value of the secret with the specified name.</returns>
    Task<string> GetSecretAsync(string secretName);

    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name, if it exists in the repository.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <returns>The value of the secret with the specified name, or <c>null</c> if it does not exist in the repository.</returns>
    Task<string?> GetSecretIfExistsAsync(string secretName);


    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name, or sets it to the specified default value if it does not exist in the repository.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="defaultValue">The default value to set if the secret does not exist.</param>
    /// <returns>The value of the secret with the specified name, or the default value if it does not exist in the repository.</returns>
    Task<string> GetOrSetSecretAsync(string secretName, string defaultValue);

    /// <summary>
    /// Asynchronously sets the value of the secret with the specified name.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="secretValue">The value of the secret.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetSecretAsync(string secretName, string secretValue);

    /// <summary>
    /// Determines if the specified exception is a "secret not found" error.
    /// </summary>
    /// <param name="exception">The exception to check.</param>
    /// <returns><c>true</c> if the exception is a "secret not found" error; otherwise, <c>false</c>.</returns>
    bool IsSecretNotFoundError(Exception exception);
}
