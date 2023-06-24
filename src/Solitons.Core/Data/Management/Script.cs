using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Management;

public abstract record Script
{
    protected Script(string id, string path)
    {
        Id = id;
        Path = path;
    }

    public abstract Task<string> LoadAsync(CancellationToken cancellation);

    public string Id { get; }
    public string Path { get; }

    public override string ToString() => Path;
}