using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public abstract class DatabaseRpcCommand
{
    /// <summary>
    /// 
    /// </summary>
    public readonly record struct DataContract(Type DtoType, string ContentType);

    internal DatabaseRpcCommand(
        string procedure,
        DataContract requestInfo,
        DataContract responseInfo)
    {
        RequestInfo = requestInfo;
        ResponseInfo = responseInfo;
        Procedure = procedure.ThrowIfNullOrWhiteSpaceArgument(nameof(procedure));

        OperationTimeout = TimeSpan.FromSeconds(30);
        IsolationLevel = IsolationLevel.ReadCommitted;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Procedure { get; }

    /// <summary>
    /// 
    /// </summary>
    public DataContract RequestInfo { get; }

    /// <summary>
    /// 
    /// </summary>
    public DataContract ResponseInfo { get; }

    /// <summary>
    /// 
    /// </summary>
    public TimeSpan OperationTimeout { get; protected init; }

    /// <summary>
    /// 
    /// </summary>
    public IsolationLevel IsolationLevel { get; protected init; }
}


/// <summary>
/// 
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class DatabaseRpcCommand<TRequest, TResponse> : DatabaseRpcCommand
{
    private const string DefaultContentType = "application/json";

    private readonly IDatabaseRpcProvider _client;
    private readonly IDataContractSerializer _serializer;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="procedure"></param>
    /// <param name="requestContentType"></param>
    /// <param name="responseContentType"></param>
    /// <param name="client"></param>
    /// <param name="serializer"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected DatabaseRpcCommand(
        string procedure, 
        string requestContentType,
        string responseContentType,
        IDatabaseRpcProvider client, 
        IDataContractSerializer serializer) : base(procedure, 
        new DataContract(typeof(TRequest), responseContentType), 
        new DataContract(typeof(TResponse), responseContentType))
    {
        _client = client.ThrowIfNullArgument(nameof(client));
        _serializer = serializer.ThrowIfNullArgument(nameof(serializer));
        if (false == _serializer.CanSerialize(typeof(TRequest), requestContentType))
        {
            throw new ArgumentOutOfRangeException($"{typeof(TRequest)} cannot be serialized to {requestContentType}");
        }

        if (false == _serializer.CanDeserialize(typeof(TResponse), responseContentType))
        {
            throw new ArgumentOutOfRangeException($"{typeof(TResponse)} cannot be deserialized from {responseContentType}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="procedure"></param>
    /// <param name="client"></param>
    /// <param name="serializer"></param>
    [DebuggerStepThrough]
    protected DatabaseRpcCommand(
        string procedure,
        IDatabaseRpcProvider client,
        IDataContractSerializer serializer) 
        : this(procedure, DefaultContentType, DefaultContentType, client, serializer)
    {
        
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
        var requestString = _serializer.Serialize(request, RequestInfo.ContentType);
        var responseString = await _client.InvokeAsync(this, requestString, cancellation);
        var response = _serializer.Deserialize<TResponse>(responseString, ResponseInfo.ContentType);
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
        var requestString = _serializer.Serialize(request, RequestInfo.ContentType);
        await _client.InvokeAsync(this, requestString, OnResponse, cancellation);
        
        Task OnResponse(string responseString)
        {
            var response = _serializer.Deserialize<TResponse>(responseString, ResponseInfo.ContentType);
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
        var requestString = _serializer.Serialize(request, RequestInfo.ContentType);
        return _client.SendAsync(this, requestString, cancellation);
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
        var requestString = _serializer.Serialize(request, RequestInfo.ContentType);
        return _client.SendAsync(this, requestString, callback, cancellation);
    }

}