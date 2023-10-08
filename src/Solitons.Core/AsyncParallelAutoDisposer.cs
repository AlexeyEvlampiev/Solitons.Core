using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Solitons;

/// <summary>
/// Manages a collection of IAsyncDisposable resources. 
/// Disposes all items independently in parallel when this instance is disposed asynchronously.
/// </summary>
/// <remarks>
/// AddResource methods are not thread-safe. Users should ensure thread-safety when using this class.
/// </remarks>
public class AsyncParallelAutoDisposer : AsyncAutoDisposer
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly HashSet<DisposableResource> _set = new();


    /// <summary>
    /// Gets the number of elements contained in the collection.
    /// </summary>
    public override int ResourceCount => _set.Count;

    /// <summary>
    /// Adds a disposable resource to the internal collection of resources.
    /// </summary>
    /// <param name="resource">The resource to be managed by the disposer.</param>
    protected override void Add(DisposableResource resource)
    {
        _set.Add(resource);
    }

    /// <summary>
    /// Asynchronously disposes all the disposable resources that are present in the collection, 
    /// by invoking each of their DisposeAsync() methods in parallel. 
    /// </summary>
    protected sealed override async Task DisposeAllAsync()
    {
        var tasks = _set
            .Select(_ => _.AsyncDisposable.DisposeAsync().AsTask())
            .ToList();

        _set.Clear();
        await Task.WhenAll(tasks);
    }
}
