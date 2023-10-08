using System.Diagnostics;
using Npgsql;
using Solitons;
using Solitons.Net.Http;

namespace SampleSoft.SkyNet.Azure.Postgres;

public sealed class SkyNetDbHttpClient : HttpClient
{
    private readonly Stack<IDisposable> _disposables = new();

    [DebuggerStepThrough]
    public SkyNetDbHttpClient(string connectionString, Action<SkyNetDbHttpClientOptions>? config = default)
        : this(BuildHandler(connectionString, config))
    {
    }

    [DebuggerStepThrough]
    public SkyNetDbHttpClient(NpgsqlConnection connection) 
        : this(new SkyNetDbHttpMessageHandler(connection))
    {
    }

    private SkyNetDbHttpClient(HttpMessageHandler handler) : base(handler)
    {
        BaseAddress = new Uri("skynetdb://api");
        foreach (var item in handler.UnrollHandlerChain())
        {
            _disposables.Push(item);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            while (_disposables.Count > 0)
            {
                var item = _disposables.Pop();
                item.Dispose();
            }
        }
        base.Dispose(disposing);
    }


    private static HttpMessageHandler BuildHandler(
        string connectionString,
        Action<SkyNetDbHttpClientOptions>? config)
    {
        var options  = new SkyNetDbHttpClientOptions();
        config?.Invoke(options);
        var handler = new SemaphoreDelegatingHandler(options.SemaphoreInitCount, options.SemaphoreWaitTimeout);
        handler.InnerHandler = new SkyNetDbHttpMessageHandler(connectionString);
        return handler;
    }

}