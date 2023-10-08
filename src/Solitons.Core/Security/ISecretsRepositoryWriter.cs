using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security;

/// <summary>
/// Provides an abstraction for writing to a secrets repository. This interface enables streamlined interaction with various secret storage mechanisms, 
/// allowing for consistent and provider-agnostic secret management in cloud-based applications. Implementations of this interface can 
/// ensure modularity and flexibility, letting applications adapt to different cloud and hosting environments with ease.
/// </summary>
/// <remarks>
/// Building on the Solitons philosophy, this abstraction promotes a cloud-agnostic approach to secret management. By leveraging this interface, 
/// developers can seamlessly switch between different secret repository backends, ensuring that the core application logic remains independent 
/// of specific cloud providers or secret storage mechanisms.
/// </remarks>
public partial interface ISecretsRepositoryWriter
{
    /// <summary>
    /// Asynchronously sets the value of the secret with the specified name.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="secretValue">The value of the secret.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetSecretAsync(string secretName, string secretValue, CancellationToken cancellation = default);
}