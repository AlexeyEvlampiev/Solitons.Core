using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public abstract class DatabaseRpcCommand
{
    private readonly Lazy<DatabaseRpcCommandMetadata> _metadata;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serializer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected DatabaseRpcCommand(IDataContractSerializer serializer)
    {
        Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        _metadata = new Lazy<DatabaseRpcCommandMetadata>(() =>
        {
            var metadata = DatabaseRpcCommandMetadata.From(GetType());
            metadata.Validate(serializer, errorMsg=> throw new InvalidOperationException(errorMsg));
            return metadata;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    protected IDataContractSerializer Serializer { get; }

    /// <summary>
    /// 
    /// </summary>
    protected DatabaseRpcCommandMetadata Metadata => _metadata.Value;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public sealed override string ToString()
    {
        return $"Procedure: {Metadata.Procedure}; {Metadata.Request.DtoType} => {Metadata.Response.DtoType}";
    }
}


/// <summary>
/// 
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class DatabaseRpcCommand<TRequest, TResponse> : DatabaseRpcCommand
{
    private readonly IDatabaseRpcProvider _client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="serializer"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected DatabaseRpcCommand(
        IDatabaseRpcProvider client, 
        IDataContractSerializer serializer) : base(serializer)
    {
        _client = client.ThrowIfNullArgument(nameof(client));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<TResponse> InvokeAsync(TRequest request, CancellationToken cancellation = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        cancellation.ThrowIfCancellationRequested();
        var requestString = Serializer.Serialize(request, Metadata.Request.ContentType);
        var responseString = await _client.InvokeAsync(Metadata, requestString, cancellation);
        var response = Serializer.Deserialize<TResponse>(responseString, Metadata.Response.ContentType);
        return response;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="callback"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task InvokeAsync(TRequest request, Func<TResponse, Task> callback, CancellationToken cancellation)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        callback.ThrowIfNullArgument(nameof(callback));
        cancellation.ThrowIfCancellationRequested();
        var requestString = Serializer.Serialize(request, Metadata.Request.ContentType);
        await _client.InvokeAsync(Metadata, requestString, OnResponse, cancellation);
        
        Task OnResponse(string responseString)
        {
            var response = Serializer.Deserialize<TResponse>(responseString, Metadata.Response.ContentType);
            return callback.Invoke(response);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public Task SendAsync(TRequest request, CancellationToken cancellation)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        cancellation.ThrowIfCancellationRequested();
        var requestString = Serializer.Serialize(request, Metadata.Request.ContentType);
        return _client.SendAsync(Metadata, requestString, cancellation);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="callback"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public Task SendAsync(TRequest request, Func<Task> callback, CancellationToken cancellation)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        cancellation.ThrowIfCancellationRequested();
        var requestString = Serializer.Serialize(request, Metadata.Request.ContentType);
        return _client.SendAsync(Metadata, requestString, callback, cancellation);
    }

}