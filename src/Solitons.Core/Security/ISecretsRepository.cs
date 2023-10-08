using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Security;

/// <summary>
/// Provides a unified abstraction for reading from and writing to a secrets repository. This interface encapsulates the functionalities 
/// offered by both <see cref="ISecretsRepositoryReader"/> and <see cref="ISecretsRepositoryWriter"/>, ensuring a comprehensive 
/// interaction model with secret storage mechanisms, regardless of the underlying cloud provider or storage model.
/// </summary>
/// <remarks>
/// Adhering to the Solitons design principles, this interface streamlines secret management across different environments, 
/// fostering modularity and portability. Developers can benefit from a consistent set of operations for secret management 
/// while maintaining the flexibility to adapt to different backend implementations.
/// </remarks>
public partial interface ISecretsRepository : ISecretsRepositoryReader, ISecretsRepositoryWriter
{
    /// <summary>
    /// Asynchronously returns the value of the secret with the specified name, or sets it to the specified default value if it does not exist in the repository.
    /// </summary>
    /// <param name="secretName">The name of the secret.</param>
    /// <param name="defaultValue">The default value to set if the secret does not exist.</param>
    /// <param name="cancellation">A cancellation token to cancel the operation.</param>
    /// <returns>The value of the secret with the specified name, or the default value if it does not exist in the repository.</returns>
    Task<string> GetOrSetSecretAsync(string secretName, string defaultValue, CancellationToken cancellation = default);

}