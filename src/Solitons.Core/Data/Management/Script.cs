using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Management;

/// <summary>
/// Represents an upgrade script for a Postgres database.
/// </summary>
public abstract record Script
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Script"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the script.</param>
    /// <param name="path">The path where the script is located.</param>
    protected Script(string id, string path)
    {
        Id = id;
        Path = path;
    }

    /// <summary>
    /// Loads the script's contents asynchronously.
    /// </summary>
    /// <param name="cancellation">The cancellation token to cancel operation.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains the script's contents.</returns>
    public abstract Task<string> LoadAsync(CancellationToken cancellation);

    /// <summary>
    /// Gets the unique identifier of the script.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the path where the script is located.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => Path;
}
