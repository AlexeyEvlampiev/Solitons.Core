using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Management;

/// <summary>
/// Represents a upgrade script embedded within an assembly.
/// </summary>
public record AssemblyEmbeddedScript : Script
{
    private readonly Assembly _assembly;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyEmbeddedScript"/> class.
    /// </summary>
    /// <param name="id">The identifier of the script.</param>
    /// <param name="resourceName">The name of the embedded resource that contains the script.</param>
    /// <param name="assembly">The assembly where the script is embedded.</param>
    public AssemblyEmbeddedScript(string id, string resourceName, Assembly assembly) : base(id, resourceName)
    {
        _assembly = assembly;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="AssemblyEmbeddedScript"/> class.
    /// </summary>
    /// <param name="id">The identifier of the script.</param>
    /// <param name="resourceName">The name of the embedded resource that contains the script.</param>
    /// <param name="assembly">The assembly where the script is embedded.</param>
    /// <returns>The created AssemblyEmbeddedScript instance.</returns>
    public static AssemblyEmbeddedScript Create(string id, string resourceName, Assembly assembly) =>
        new AssemblyEmbeddedScript(id, resourceName, assembly);

    /// <summary>
    /// Loads the script from the embedded resource asynchronously.
    /// </summary>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous load operation. The task result contains the script content.</returns>
    public override async Task<string> LoadAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        await using var stream = _assembly
            .GetManifestResourceStream(Path)
            .Convert(nullable => ThrowIf.NullReference(
                nullable,
                $"Failed to load assembly manifest resource '{Path}' from assembly '{_assembly.FullName}'. " +
                "Ensure the resource name is correct and the resource is properly embedded into the assembly."));

        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();
        return content;
    }
}