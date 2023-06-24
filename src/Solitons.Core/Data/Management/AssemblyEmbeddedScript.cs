using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Management;

public record AssemblyEmbeddedScript : Script
{
    private readonly Assembly _assembly;

    public AssemblyEmbeddedScript(string id, string resourceName, Assembly assembly) : base(id, resourceName)
    {
        _assembly = assembly;
    }

    public static AssemblyEmbeddedScript Create(string id, string resourceName, Assembly assembly) =>
        new AssemblyEmbeddedScript(id, resourceName, assembly);


    public override async Task<string> LoadAsync(CancellationToken cancellation)
    {
        await using var stream = _assembly
            .GetManifestResourceStream(Path)
            .Convert(nullable => ThrowIf.NullReference(
                nullable,
                $"{_assembly.FullName}.{Id} assembly manifest resource not found."));

        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        return content;
    }

}