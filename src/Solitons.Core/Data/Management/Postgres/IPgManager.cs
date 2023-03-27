using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Management.Postgres;

/// <summary>
/// Provides functionality for managing a Postgres database.
/// </summary>
public interface IPgManager
{
    /// <summary>
    /// Gets the name of the database that this instance manages.
    /// </summary>
    string Database { get; }

    /// <summary>
    /// Creates the database asynchronously.
    /// </summary>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateDbAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Drops the database asynchronously.
    /// </summary>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DropDbAsync(CancellationToken cancellation = default);

    /// <summary>
    /// Drops and recreates the database asynchronously.
    /// </summary>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [DebuggerStepThrough]
    public sealed async Task DropAndRecreateAsync(CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        await DropDbAsync(cancellation);
        cancellation.ThrowIfCancellationRequested();
        await CreateDbAsync(cancellation);
    }
}