using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

public abstract class AwaitableHttpClient : HttpClient, IAwaitable
{
    private readonly IAwaitable _awaitable;

    [DebuggerStepThrough]
    protected AwaitableHttpClient(
        HttpMessageHandler handler) 
        : this(handler, Enumerable.Empty<IAwaitable>())
    {
    }

    protected AwaitableHttpClient(
        HttpMessageHandler handler, 
        IEnumerable<IAwaitable> innerAwaitables) : base(handler)
    {
        _awaitable = handler
            .UnrollHandlerChain()
            // ReSharper disable once SuspiciousTypeConversion.Global
            .OfType<IAwaitable>()
            .Union(innerAwaitables)
            .Distinct()
            .Convert(IAwaitable.WhenAny);
    }

    public Task RunAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return _awaitable.AsTask(cancellation);
    }

    [DebuggerStepThrough]
    Task IAwaitable.AsTask(CancellationToken cancellation) => RunAsync(cancellation);
}   