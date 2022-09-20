using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public abstract class DatabaseRpcCommand : IDatabaseRpcCommand
{
    private readonly IDatabaseRpcProvider _provider;
    private readonly IDataContractSerializer _serializer;
    private readonly Lazy<DatabaseRpcCommandMetadata> _metadata;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="serializer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    protected internal DatabaseRpcCommand(IDatabaseRpcProvider provider, IDataContractSerializer serializer)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));


        _metadata = new Lazy<DatabaseRpcCommandMetadata>(() =>
        {
            var metadata = DatabaseRpcCommandMetadata.From(GetType());
            metadata.Validate(serializer, errorMsg=> throw new InvalidOperationException(errorMsg));
            return metadata;
        });
    }

    [DebuggerStepThrough]
    bool IDatabaseRpcCommand.CanAccept(MediaContent request) => _serializer
        .CanDeserialize(Metadata.Request.DtoType, request.ContentType);

    async Task<MediaContent> IDatabaseRpcCommand.InvokeAsync(MediaContent request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        if (false == _serializer.CanDeserialize(Metadata.Request.DtoType, request.ContentType))
        {
            throw new ArgumentOutOfRangeException(
                $"Cannot deserialize {Metadata.Request.DtoType} from the '{request.ContentType}' content");
        }

        _serializer.Deserialize(Metadata.Request.DtoType, request.Content, request.ContentType);
        request = await request.TransformAsync(TransformRequestAsync);
        var content = await _provider.InvokeAsync(Metadata, request.Content, cancellation);
        content = await TransformResponseAsync(content);
        var dto = _serializer.Deserialize(Metadata.Response.DtoType, content, Metadata.Response.ContentType);
        content = _serializer.Serialize(dto, Metadata.Response.ContentType);
        return new MediaContent(content, Metadata.Response.ContentType);
    }

    async Task IDatabaseRpcCommand.SendAsync(MediaContent request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        if (false == _serializer.CanDeserialize(Metadata.Request.DtoType, request.ContentType))
        {
            throw new ArgumentOutOfRangeException($"Cannot deserialize {Metadata.Request.DtoType} from the '{request.ContentType}' content");
        }

        request = await request.TransformAsync(TransformRequestAsync);
        _serializer.Deserialize(Metadata.Request.DtoType, request.Content, request.ContentType);
        await _provider.SendAsync(Metadata, request.Content, cancellation);
    }

    Task IDatabaseRpcCommand.SendAsync(object dto, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        var self = (IDatabaseRpcCommand)this;
        if (dto is MediaContent media)
        {
            return self.SendAsync(media, cancellation);
        }

        if (Metadata.Request.DtoType.IsInstanceOfType(dto))
        {
            var contentType = Metadata.Request.ContentType;
            var content = _serializer.Serialize(dto, contentType);
            return self.SendAsync(new MediaContent(content, contentType), cancellation);
        }

        throw new InvalidCastException(new StringBuilder("Invalid request DTO type")
            .Append($" Expected: {Metadata.Request.DtoType}. Actual: {dto.GetType()}")
            .ToString());
    }

    [DebuggerStepThrough]
    Task IDatabaseRpcCommand.SendViaAsync(
        ILargeObjectQueueProducer queue, 
        object dto, 
        Action<DataTransferPackage> config, 
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return SendViaAsync(queue, dto, config, cancellation);
    }

    [DebuggerStepThrough]
    Task IDatabaseRpcCommand.SendViaAsync(
        ILargeObjectQueueProducer queue,
        object dto,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return SendViaAsync(
            queue, 
            dto,
            package=>{},
            cancellation);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    protected async Task<object> GeneralizedInvokeAsync(object? request, CancellationToken cancellation = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        cancellation.ThrowIfCancellationRequested();
        var requestString = _serializer.Serialize(request, Metadata.Request.ContentType);
        requestString = await TransformRequestAsync(requestString);

        var responseString = await _provider.InvokeAsync(Metadata, requestString, cancellation);
        responseString = await TransformResponseAsync(responseString);
        var response = _serializer.Deserialize(Metadata.Response.DtoType, responseString, Metadata.Response.ContentType);
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
    protected async Task GeneralizedInvokeAsync(object? request, Func<object, Task> callback, CancellationToken cancellation)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        callback.ThrowIfNullArgument(nameof(callback));
        cancellation.ThrowIfCancellationRequested();

        var requestString = _serializer.Serialize(request, Metadata.Request.ContentType);
        requestString = await TransformRequestAsync(requestString);

        await _provider.InvokeAsync(Metadata, requestString, OnResponse, cancellation);

        async Task OnResponse(string responseString)
        {
            responseString = await TransformResponseAsync(responseString);
            var response = _serializer.Deserialize(Metadata.Response.DtoType, responseString, Metadata.Response.ContentType);
            await callback.Invoke(response);
        }
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    protected async Task GeneralizedSendAsync(object? request, CancellationToken cancellation)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        cancellation.ThrowIfCancellationRequested();
        var requestString = _serializer.Serialize(request, Metadata.Request.ContentType);
        requestString = await TransformRequestAsync(requestString);
        await _provider.SendAsync(Metadata, requestString, cancellation);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="callback"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    protected async Task GeneralizedSendAsync(object? request, Func<Task> callback, CancellationToken cancellation)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        cancellation.ThrowIfCancellationRequested();
        var requestString = _serializer.Serialize(request, Metadata.Request.ContentType);
        requestString = await TransformRequestAsync(requestString);
        await _provider.SendAsync(Metadata, requestString, callback, cancellation);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    protected virtual Task<string> TransformRequestAsync(string content) => Task.FromResult(content);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    protected virtual Task<string> TransformResponseAsync(string content) => Task.FromResult(content);


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


    private async Task SendViaAsync(
        ILargeObjectQueueProducer queue,
        object dto,
        Action<DataTransferPackage> config,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        if (Metadata.Request.DtoType.IsInstanceOfType(dto))
        {
            var content = _serializer.Serialize(dto, Metadata.Request.ContentType);
            content = await TransformRequestAsync(content);
            var package = new DataTransferPackage(Metadata.CommandOid, content, Metadata.Request.ContentType, Encoding.UTF8);
            config.Invoke(package);
            
            await queue.SendAsync(package, DataTransferMethod.ByValue, cancellation);
        }

        throw new InvalidCastException(new StringBuilder("Invalid request DTO type")
            .Append($" Expected: {Metadata.Request.DtoType}. Actual: {dto.GetType()}")
            .ToString());
    }
}


/// <summary>
/// 
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class DatabaseRpcCommand<TRequest, TResponse> : DatabaseRpcCommand
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="serializer"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [DebuggerNonUserCode]
    protected DatabaseRpcCommand(
        IDatabaseRpcProvider client, 
        IDataContractSerializer serializer) : base(client, serializer)
    {
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [DebuggerStepThrough]
    public async Task<TResponse> InvokeAsync([DisallowNull] TRequest request, CancellationToken cancellation = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        cancellation.ThrowIfCancellationRequested();
        return (TResponse) (await GeneralizedInvokeAsync(request, cancellation));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="callback"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [DebuggerStepThrough]
    public async Task<TResponse> InvokeAsync(TRequest request, Func<TResponse, Task> callback, CancellationToken cancellation)
    {
        TResponse? response = default;
        await GeneralizedInvokeAsync(request, OnResponse, cancellation);
        return response ?? throw new InvalidOperationException();

        [DebuggerStepThrough]
        Task OnResponse(object responseObj)
        {
            response = (TResponse)responseObj;
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
    [DebuggerStepThrough]
    public Task SendAsync(TRequest request, CancellationToken cancellation) => GeneralizedSendAsync(request, cancellation);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="callback"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [DebuggerStepThrough]
    public Task SendAsync(TRequest request, Func<Task> callback, CancellationToken cancellation) => GeneralizedSendAsync(request, callback, cancellation);


    [DebuggerStepThrough]
    Task SendViaAsync(
        ILargeObjectQueueProducer queue,
        [DisallowNull] TRequest dto,
        Action<DataTransferPackage> config,
        CancellationToken cancellation)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        cancellation.ThrowIfCancellationRequested();
        return ((IDatabaseRpcCommand)this).SendViaAsync(queue, dto, config, cancellation);
    }

    [DebuggerStepThrough]
    Task SendViaAsync(
        ILargeObjectQueueProducer queue,
        [DisallowNull] TRequest dto,
        CancellationToken cancellation)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        cancellation.ThrowIfCancellationRequested();
        return ((IDatabaseRpcCommand)this).SendViaAsync(queue, dto, cancellation);
    }
}