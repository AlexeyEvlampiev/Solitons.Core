﻿using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Solitons;

namespace SampleSoft.SkyNet.Azure.Postgres;

public sealed class SkyNetDbHttpClient : HttpClient, 
    IAsyncDisposable,
    IAwaitable
{
    private readonly SkyNetDbHttpMessageHandler _databaseHttpMessageHandler;
    private readonly DelegatingHandler[] _delegatingHandlers;
    private readonly AsyncStackAutoDisposer _disposer = new();
    private readonly IAwaitable[] _awaitables;

    [DebuggerStepThrough]
    public SkyNetDbHttpClient(
        SkyNetDbHttpMessageHandler handler,
        DelegatingHandler[] delegatingHandlers)
        : base(BuildHandler(handler, delegatingHandlers))
    {
        BaseAddress = new Uri("skynetdb://api");
        _databaseHttpMessageHandler = handler;
        _delegatingHandlers = delegatingHandlers;
        var awaitables = new List<IAwaitable>() { handler };
        foreach (var middleware in delegatingHandlers)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (middleware is IAwaitable awaitable)
            {
                awaitables.Add(awaitable);
            }
        }
        _awaitables = awaitables.ToArray();
        _disposer.AddResource((IDisposable)this);
    }

    [DebuggerStepThrough]
    public SkyNetDbHttpClient(
        SkyNetDbHttpMessageHandler handler,
        IEnumerable<DelegatingHandler> delegatingHandlers)
        : this(handler, delegatingHandlers.ToArray())
    {
    }

    [DebuggerStepThrough]
    public SkyNetDbHttpClient(
        SkyNetDbHttpMessageHandler handler)
        : this(handler, Enumerable.Empty<DelegatingHandler>())
    {
    }


    [DebuggerStepThrough]
    public SkyNetDbHttpClient(
        string connectionString,
        IEnumerable<DelegatingHandler> delegatingHandlers)
        : this(new SkyNetDbHttpMessageHandler(connectionString), delegatingHandlers)
    {
        // The database handler is owned by this client, so will be disposed accordingly
        _disposer.AddResource(_databaseHttpMessageHandler);
    }

    [DebuggerStepThrough]
    public SkyNetDbHttpClient(
        string connectionString)
        : this(new SkyNetDbHttpMessageHandler(connectionString), Enumerable.Empty<DelegatingHandler>())
    {
        // The database handler is owned by this client, so will be disposed accordingly
        _disposer.AddResource(_databaseHttpMessageHandler);
    }

    [DebuggerStepThrough]
    public SkyNetDbHttpClient(
        string connectionString,
        Func<DelegatingHandler[]> middlewareFactory)
        : this(new SkyNetDbHttpMessageHandler(connectionString), middlewareFactory.Invoke())
    {
        // All the http handlers are owned by this client, so will be disposed accordingly
        _disposer.AddResource(_databaseHttpMessageHandler);
        foreach (var middleware in _delegatingHandlers)
        {
            _disposer.AddResource(middleware);
        }
    }

    private static HttpMessageHandler BuildHandler(
        SkyNetDbHttpMessageHandler innerHandler,
        IEnumerable<DelegatingHandler> delegatingHandlers)
    {
        HttpMessageHandler handler = innerHandler;
        foreach (var middleware in delegatingHandlers)
        {
            middleware.InnerHandler = handler;
            handler = middleware;
        }

        return handler;
    }

    public async Task RunAsync(CancellationToken cancellation = default)
    {
        try
        {
            await IAwaitable
                .WhenAny(_awaitables)
                .AsTask(cancellation);
        }
        finally
        {
            if (false == cancellation.IsCancellationRequested)
            {
                await this.DisposeAsync();
            }
        }
    }

    Task IAwaitable.AsTask(CancellationToken cancellation) => RunAsync(cancellation);

    [DebuggerStepThrough]
    public ValueTask DisposeAsync() => _disposer.DisposeAsync();
}