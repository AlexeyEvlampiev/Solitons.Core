using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;

namespace Solitons;

public static partial class Extensions
{
    /// <summary>
    /// Asynchronously reads the HTTP content and returns it as a string.
    /// </summary>
    /// <param name="content">The HTTP content to be read.</param>
    /// <param name="cancellation">Optional token to observe for cancellation of the read operation.</param>
    /// <returns>An IObservable that contains the content of the HTTP response message as a string.</returns>
    [DebuggerNonUserCode]
    public static IObservable<string> ReadAsString(this HttpContent content, CancellationToken cancellation = default)
    {
        return Observable.FromAsync(() => content.ReadAsStringAsync(cancellation));
    }
}