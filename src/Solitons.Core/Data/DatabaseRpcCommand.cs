using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
    private static readonly DatabaseRpcCommandMetadataCache Cache = new();

    #region ctors
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="serializer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    protected internal DatabaseRpcCommand(
        IDatabaseRpcProvider provider, 
        IDataContractSerializer serializer)
    {
        Metadata = Cache.GetOrCreate(GetType());
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

        var (request, response) = (Metadata.Request, Metadata.Response);
        if (false == _serializer
                .CanSerialize(request.DtoType, request.ContentType))
        {
            throw new ArgumentException(
                new StringBuilder("RPC request DTO type not supported.")
                    .Append($" Request DTO type: {request.DtoType}.")
                    .Append($" RPC: {Metadata}")
                .ToString(), 
                nameof(serializer));
        }

        if (false == _serializer
                .CanSerialize(response.DtoType, response.ContentType))
        {
            throw new ArgumentException(
                new StringBuilder("RPC response DTO type not supported.")
                    .Append($" Response DTO type: {response.DtoType}.")
                    .Append($" RPC: {Metadata}")
                    .ToString(),
                nameof(serializer));
        }
    }

    #endregion

    #region IDatabaseRpcCommand Implementation

    [DebuggerStepThrough]
    bool IDatabaseRpcCommand.CanAccept(MediaContent request) => _serializer
        .CanDeserialize(Metadata.Request.DtoType, request.ContentType);

    [DebuggerStepThrough]
    Task<MediaContent> IDatabaseRpcCommand.InvokeAsync(MediaContent request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return InvokeAsync(request, cancellation);
    }

    [DebuggerStepThrough]
    Task IDatabaseRpcCommand.SendAsync(MediaContent request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return SendAsync(request, cancellation);
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

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    protected virtual async Task<MediaContent> InvokeAsync(MediaContent request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        if (false == _serializer.CanDeserialize(Metadata.Request.DtoType, Metadata.Request.ContentType) ||
            false == _serializer.CanDeserialize(Metadata.Request.DtoType, request.ContentType))
        {
            throw new ArgumentOutOfRangeException(
                $"Cannot deserialize {Metadata.Request.DtoType} from the '{request.ContentType}' content.");
        }

        if (false == request.ContentType.Equals(Metadata.Request.ContentType, StringComparison.OrdinalIgnoreCase))
        {
            var transformed = _serializer.Transform(
                Metadata.Request.DtoType.GUID,
                request,
                Metadata.Request.ContentType);
            Debug.Assert(transformed.ContentType.Equals(Metadata.Request.ContentType));
            var result = await InvokeAsync(transformed, cancellation);
            if (_serializer.CanSerialize(Metadata.Response.DtoType, request.ContentType))
            {
                return _serializer.Transform(
                    Metadata.Response.DtoType.GUID,
                    result,
                    request.ContentType);
            }
            return result;
        }

        // 1) Apply custom request transformation
        request = request.WithContent(await TransformRequestAsync(request.Content));
        // 2) Validate request schema by deserializing the request content
        var requestDto = _serializer.Deserialize(Metadata.Request.DtoType, request.Content, request.ContentType);
        // 3) Define the [on-committed] callback delegate variable
        Func<Task> onCommittedAsync = () => 
            throw new InvalidOperationException($"{nameof(onCommittedAsync)} override is missing.");
        // 4) Invoke the RPC transaction
        var response = await _provider.InvokeAsync(Metadata, request.Content, ParseResponseAsync, cancellation);
        // 5) Invoke the overriden [on-committed] callback variable
        await onCommittedAsync.Invoke();
        return response;


        [DebuggerStepThrough]
        async Task<MediaContent> ParseResponseAsync(string content)
        {
            cancellation.ThrowIfCancellationRequested();
            content = await TransformResponseAsync(content);
            cancellation.ThrowIfCancellationRequested();
            // Validate request schema by deserializing the response content
            var responseDto = _serializer.Deserialize(Metadata.Response.DtoType, content, Metadata.Response.ContentType);
            // Sanitize the response by serializing the response dto
            content = _serializer.Serialize(responseDto, Metadata.Response.ContentType);
            onCommittedAsync = () => OnInvokedAsync(requestDto, responseDto, cancellation);
            return new MediaContent(content, Metadata.Response.ContentType);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected virtual async Task SendAsync(MediaContent request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        if (false == _serializer.CanDeserialize(Metadata.Request.DtoType, request.ContentType))
        {
            throw new ArgumentOutOfRangeException($"Cannot deserialize {Metadata.Request.DtoType} from the '{request.ContentType}' content");
        }

        // 1) Apply custom request transformation
        request = request.WithContent(await TransformRequestAsync(request.Content));
        // 2) Validate request schema by deserializing the request content
        var requestDto = _serializer.Deserialize(Metadata.Request.DtoType, request.Content, request.ContentType);
        // 3) Invoke the Send -transaction
        await _provider.SendAsync(Metadata, request.Content, OnCommittingAsync, cancellation);

        [DebuggerStepThrough]
        async Task OnCommittingAsync()
        {
            cancellation.ThrowIfCancellationRequested();
            await OnSentAsync(requestDto, cancellation);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestDto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    protected virtual async Task<object> InvokeAsync(object requestDto, CancellationToken cancellation = default)
    {
        if (requestDto is null) throw new ArgumentNullException(nameof(requestDto));
        cancellation.ThrowIfCancellationRequested();
        var request = _serializer
            .Serialize(requestDto, Metadata.Request.ContentType)
            .Convert(c => new MediaContent(c, Metadata.Request.ContentType));

        request = request
            .WithContent(await TransformRequestAsync(request.Content));


        return await _provider.InvokeAsync(Metadata, request.Content, ParseResponseAsync, cancellation);


        [DebuggerStepThrough]
        async Task<object> ParseResponseAsync(string content)
        {
            cancellation.ThrowIfCancellationRequested();
            content = await TransformResponseAsync(content);
            cancellation.ThrowIfCancellationRequested();
            var responseDto = _serializer.Deserialize(Metadata.Response.DtoType, content, Metadata.Response.ContentType);
            await OnInvokedAsync(requestDto, responseDto, cancellation);
            return responseDto;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestDto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    protected virtual async Task SendAsync(object requestDto, CancellationToken cancellation)
    {
        if (requestDto is null) throw new ArgumentNullException(nameof(requestDto));
        cancellation.ThrowIfCancellationRequested();
        var content = _serializer.Serialize(requestDto, Metadata.Request.ContentType);
        content = await TransformRequestAsync(content);
        await _provider.SendAsync(Metadata, content, NotifyMessageSentAsync, cancellation);

        [DebuggerStepThrough]
        async Task NotifyMessageSentAsync()
        {
            cancellation.ThrowIfCancellationRequested();
            await OnSentAsync(requestDto, cancellation);
        }
    }


    /// <summary>
    /// Called before the RPC transaction
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    protected virtual Task<string> TransformRequestAsync(string request) => Task.FromResult(request);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    protected virtual Task<string> TransformResponseAsync(string content) => Task.FromResult(content);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestDto"></param>
    /// <param name="responseDto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    protected virtual Task OnInvokedAsync(object requestDto, object responseDto, CancellationToken cancellation) => Task.CompletedTask;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestDto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    protected virtual Task OnSentAsync(
        object requestDto,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the command RPC descriptor
    /// </summary>
    protected DatabaseRpcCommandMetadata Metadata { get; }

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
public abstract class DatabaseRpcCommand<TRequest, TResponse> 
    : DatabaseRpcCommand
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
    /// <param name="requestDto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [DebuggerStepThrough]
    public async Task<TResponse> InvokeAsync([DisallowNull] TRequest requestDto, CancellationToken cancellation = default)
    {
        if (requestDto == null) throw new ArgumentNullException(nameof(requestDto));
        cancellation.ThrowIfCancellationRequested();
        return (TResponse) (await base.InvokeAsync(requestDto, cancellation));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestDto"></param>
    /// <param name="responseDto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    protected sealed override Task OnInvokedAsync(object requestDto, object responseDto, CancellationToken cancellation) 
        => OnInvokedAsync((TRequest)requestDto, (TResponse)responseDto, cancellation);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestDto"></param>
    /// <param name="responseDto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    protected virtual Task OnInvokedAsync(
        TRequest requestDto, 
        TResponse responseDto,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestDto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    protected sealed override Task OnSentAsync(
        object requestDto, 
        CancellationToken cancellation) =>
        this.OnSentAsync((TRequest)requestDto, cancellation);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestDto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    protected virtual Task OnSentAsync(TRequest requestDto, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [DebuggerStepThrough]
    public Task SendAsync(TRequest request, CancellationToken cancellation) => base.SendAsync(request, cancellation);


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