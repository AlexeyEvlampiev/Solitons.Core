using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security;

/// <summary>
/// Provides an abstraction for reading from a secrets repository. This interface facilitates interaction with various secrets repositories, 
/// catering to different cloud providers and hosting models. Implementing this interface enables the decoupling of application core from specific 
/// cloud providers or hosting types, thereby promoting adaptability and flexibility in distributed computing systems.
/// </summary>
/// <remarks>
/// Solitons addresses the volatility of distributed computing systems by generalizing interaction with secret repositories through this abstraction. 
/// It provides basic implementations, such as Environment variables based, In-Memory secrets repository, and SQLite-based setups, 
/// and supports advanced caching strategies to optimize network usage when secrets are sourced from remote services.
/// </remarks>
public partial interface ISecretsRepositoryReader
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
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>The value of the secret with the specified name.</returns>
    Task<string> GetSecretAsync(string secretName, CancellationToken cancellation = default);

    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name, if it exists in the repository.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>The value of the secret with the specified name, or <c>null</c> if it does not exist in the repository.</returns>
    Task<string?> GetSecretIfExistsAsync(string secretName, CancellationToken cancellation = default);


    /// <summary>
    /// Determines if the specified exception is a "secret not found" error.
    /// </summary>
    /// <param name="exception">The exception to check.</param>
    /// <returns><c>true</c> if the exception is a "secret not found" error; otherwise, <c>false</c>.</returns>
    bool IsSecretNotFoundError(Exception exception);
}