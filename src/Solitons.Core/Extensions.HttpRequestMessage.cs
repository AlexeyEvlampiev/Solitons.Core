using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Solitons.Diagnostics;

namespace Solitons;

public static partial class Extensions
{
    private const string AsyncLoggerHttpRequestOptionsKey = "logger";

    /// <summary>
    /// Adds an IAsyncLogger instance to the options of the HttpRequestMessage.
    /// </summary>
    /// <param name="request">The HttpRequestMessage instance.</param>
    /// <param name="logger">The IAsyncLogger instance to add to the request's options.</param>
    /// <returns>The same HttpRequestMessage instance, to allow chaining of calls.</returns>
    [DebuggerNonUserCode]
    public static HttpRequestMessage AddLogger(this HttpRequestMessage request, IAsyncLogger logger)
    {
        var key = new HttpRequestOptionsKey<IAsyncLogger>(AsyncLoggerHttpRequestOptionsKey);
        request.Options.Set(key, logger);
        return request;
    }

    /// <summary>
    /// Retrieves an IAsyncLogger instance from the options of the HttpRequestMessage.
    /// </summary>
    /// <param name="request">The HttpRequestMessage instance.</param>
    /// <returns>The IAsyncLogger instance, or an IAsyncLogger.Null if no logger is set in the options.</returns>
    [DebuggerNonUserCode]
    public static IAsyncLogger GetLogger(this HttpRequestMessage request)
    {
        var key = new HttpRequestOptionsKey<IAsyncLogger>(AsyncLoggerHttpRequestOptionsKey);
        request.Options.TryGetValue(key, out var logger);
        return logger ?? IAsyncLogger.Null;
    }
}