using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// Base class for implementing database-related remote procedure calls (RPCs).
/// </summary>
public abstract class DatabaseRpcCommand : IDatabaseRpcCommand
{
    private readonly IDatabaseRpcProvider _provider;
    private readonly IDataContractSerializer _serializer;
    private static readonly DatabaseRpcCommandMetadataCache Cache = new();

    private delegate Task OnCommittedAsync();

    #region ctors

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseRpcCommand"/> class.
    /// </summary>
    /// <param name="provider">The provider instance that provides the database RPC service.</param>
    /// <param name="serializer">The serializer instance used to serialize and deserialize data.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="provider"/> or <paramref name="serializer"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the request or response DTO type is not supported by the serializer.</exception>
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

    /// <inheritdoc />
    [DebuggerStepThrough]
    bool IDatabaseRpcCommand.CanAccept(TextMediaContent request) => _serializer
        .CanDeserialize(Metadata.Request.DtoType, request.ContentType);

    /// <inheritdoc />
    [DebuggerStepThrough]
    Task<TextMediaContent> IDatabaseRpcCommand.InvokeAsync(TextMediaContent request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return InvokeAsync(request, cancellation);
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    Task<TextMediaContent> IDatabaseRpcCommand.WhatIfAsync(TextMediaContent request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return WhatIfAsync(request, cancellation);
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    Task IDatabaseRpcCommand.SendAsync(TextMediaContent request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return SendAsync(request, cancellation);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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
    /// Invokes the database RPC command asynchronously.
    /// </summary>
    /// <param name="request">The RPC request message.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>The RPC response message.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the deserialization of the request or response message fails.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the <see cref="OnInvokedAsync(object, object, CancellationToken)"/> override is missing.
    /// </exception>
    protected virtual async Task<TextMediaContent> InvokeAsync(
        TextMediaContent request, 
        CancellationToken cancellation)
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
        request = request.WithContent(await TransformRequestAsync(request.Content, cancellation));
        // 2) Validate request schema by deserializing the request content
        var requestDto = _serializer.Deserialize(Metadata.Request.DtoType, request.Content, request.ContentType);
        // 3) Initialize the [on-committed] callback variable
        OnCommittedAsync onCommittedAsync = () => 
            throw new InvalidOperationException($"{nameof(onCommittedAsync)} override is missing.");
        // 4) Invoke the RPC transaction
        var response = await _provider.InvokeAsync(Metadata, request.Content, ParseResponseAsync, cancellation);
        // 5) Invoke the overriden [on-committed] callback variable
        await onCommittedAsync.Invoke();
        return response;



        [DebuggerStepThrough]
        async Task<TextMediaContent> ParseResponseAsync(string content)
        {
            ThrowIf.Cancelled(cancellation);
            content = await TransformResponseAsync(content, cancellation);
            ThrowIf.Cancelled(cancellation);
            // Validate request schema by deserializing the response content
            var responseDto = _serializer.Deserialize(Metadata.Response.DtoType, content, Metadata.Response.ContentType);
            // Sanitize the response by serializing the response dto
            content = _serializer.Serialize(responseDto, Metadata.Response.ContentType);
            // Apply custom transformation to the response content
            content = await TransformResponseAsync(content, cancellation);
            // Override the committed- callback
            onCommittedAsync = () => OnInvokedAsync(requestDto, responseDto, cancellation);
            return new TextMediaContent(content, Metadata.Response.ContentType);
        }
    }

    /// <summary>
    /// Executes a "what-if" database RPC command, which rolls back the changes made by the command after executing it.
    /// </summary>
    /// <param name="request">The RPC request message.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>The RPC response message.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the deserialization of the request or response message fails.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the <see cref="OnRevertedAsync(object, object, CancellationToken)"/> override is missing.
    /// </exception>
    protected virtual async Task<TextMediaContent> WhatIfAsync(TextMediaContent request, CancellationToken cancellation)
    {
        ThrowIf.Cancelled(cancellation);
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
            // Recursion:
            var result = await WhatIfAsync(transformed, cancellation);
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
        request = request.WithContent(await TransformRequestAsync(request.Content, cancellation));
        // 2) Validate request schema by deserializing the request content
        var requestDto = _serializer.Deserialize(Metadata.Request.DtoType, request.Content, request.ContentType);
        // 3) Initialize the [on-committed] callback variable
        OnCommittedAsync onCommittedAsync = () =>
            throw new InvalidOperationException($"{nameof(onCommittedAsync)} override is missing.");
        // 4) Invoke the RPC transaction with rollback
        var response = await _provider.WhatIfAsync(Metadata, request.Content, ParseResponseAsync, cancellation);
        await onCommittedAsync.Invoke();
        return response;


        [DebuggerStepThrough]
        async Task<TextMediaContent> ParseResponseAsync(string content)
        {
            ThrowIf.Cancelled(cancellation);
            content = await TransformResponseAsync(content, cancellation);
            ThrowIf.Cancelled(cancellation);
            // Validate request schema by deserializing the response content
            var responseDto = _serializer.Deserialize(Metadata.Response.DtoType, content, Metadata.Response.ContentType);
            // Sanitize the response by serialization
            content = _serializer.Serialize(responseDto, Metadata.Response.ContentType);
            // Apply custom transformation to the response content
            content = await TransformResponseAsync(content, cancellation);
            // Override the committed- callback
            onCommittedAsync = () => OnRevertedAsync(requestDto, responseDto, cancellation);
            return new TextMediaContent(content, Metadata.Response.ContentType);
        }
    }


    /// <summary>
    /// Sends the database RPC command asynchronously, without expecting a response from the server.
    /// </summary>
    /// <param name="request">The RPC request message.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task representing the asynchronous send operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the deserialization of the request message fails.
    /// </exception>
    protected virtual async Task SendAsync(TextMediaContent request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        if (false == _serializer.CanDeserialize(Metadata.Request.DtoType, request.ContentType))
        {
            throw new ArgumentOutOfRangeException($"Cannot deserialize {Metadata.Request.DtoType} from the '{request.ContentType}' content");
        }

        // 1) Apply custom request transformation
        request = request.WithContent(await TransformRequestAsync(request.Content, cancellation));
        // 2) Validate request schema by deserializing the request content
        var requestDto = _serializer.Deserialize(Metadata.Request.DtoType, request.Content, request.ContentType);
        // 3) Invoke the Send -transaction
        await _provider.SendAsync(Metadata, request.Content, OnCommittingAsync, cancellation);

        [DebuggerStepThrough]
        async Task OnCommittingAsync()
        {
            ThrowIf.Cancelled(cancellation);
            await OnSentAsync(requestDto, cancellation);
        }
    }

    /// <summary>
    /// Invokes the database RPC command asynchronously with the specified request object.
    /// </summary>
    /// <param name="requestDto">The request object.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>The response object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request object is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the serialization of the request message fails or the deserialization of the response message fails.
    /// </exception>
    protected virtual async Task<object> InvokeAsync(object requestDto, CancellationToken cancellation = default)
    {
        ThrowIf.ArgumentNull(requestDto);
        ThrowIf.Cancelled(cancellation);
        var request = _serializer
            .Serialize(requestDto, Metadata.Request.ContentType)
            .Convert(c => new TextMediaContent(c, Metadata.Request.ContentType));

        request = request
            .WithContent(await TransformRequestAsync(request.Content, cancellation));

        ThrowIf.Cancelled(cancellation);

        return await _provider.InvokeAsync(Metadata, request.Content, ParseResponseAsync, cancellation);


        [DebuggerStepThrough]
        async Task<object> ParseResponseAsync(string content)
        {
            ThrowIf.Cancelled(cancellation);
            content = await TransformResponseAsync(content, cancellation);
            ThrowIf.Cancelled(cancellation);
            var responseDto = _serializer.Deserialize(Metadata.Response.DtoType, content, Metadata.Response.ContentType);
            await OnInvokedAsync(requestDto, responseDto, cancellation);
            return responseDto;
        }
    }

    /// <summary>
    /// Executes the database RPC command in a What-If mode, meaning that it simulates the command execution and
    /// returns the expected response without actually executing any write operation.
    /// </summary>
    /// <param name="requestDto">The request object.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>The response object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request object is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the serialization of the request message fails or the deserialization of the response message fails.
    /// </exception>
    protected virtual async Task<object> WhatIfAsync(object requestDto, CancellationToken cancellation = default)
    {
        ThrowIf.ArgumentNull(requestDto, nameof(requestDto));
        ThrowIf.Cancelled(cancellation);

        return await _serializer
            .Serialize(requestDto, Metadata.Request.ContentType)
            .Convert(Observable.Return)
            .SelectMany(content => TransformRequestAsync(content, cancellation))
            .Do(_ => ThrowIf.Cancelled(cancellation))
            .SelectMany(content => _provider.WhatIfAsync(Metadata, content, ParseResponseAsync, cancellation));
        [DebuggerStepThrough]
        async Task<object> ParseResponseAsync(string content)
        {
            ThrowIf.Cancelled(cancellation);
            content = await TransformResponseAsync(content, cancellation);
            ThrowIf.Cancelled(cancellation);
            var responseDto = _serializer.Deserialize(Metadata.Response.DtoType, content, Metadata.Response.ContentType);
            await OnRevertedAsync(requestDto, responseDto, cancellation);
            return responseDto;
        }
    }

    /// <summary>
    /// Sends the specified request to a database queue.
    /// </summary>
    /// <param name="requestDto">The request to send.</param>
    /// <param name="cancellation">The cancellation token to observe.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified content type in the request is not supported.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the specified request is null.
    /// </exception>
    protected virtual async Task SendAsync(object requestDto, CancellationToken cancellation)
    {
        ThrowIf.ArgumentNull(requestDto, nameof(requestDto));
        ThrowIf.Cancelled(cancellation);
        await _serializer
            .Serialize(requestDto, Metadata.Request.ContentType)
            .Convert(Observable.Return)
            .SelectMany(content => TransformRequestAsync(content, cancellation))
            .SelectMany(content => _provider
                .SendAsync(Metadata, content, NotifyMessageSentAsync, cancellation)
                .ToObservable());

        [DebuggerStepThrough]
        async Task NotifyMessageSentAsync()
        {
            ThrowIf.Cancelled(cancellation);
            await OnSentAsync(requestDto, cancellation);
        }
    }


    /// <summary>
    /// Transforms the given request content before invoking the database RPC provider.
    /// This method is called before any serialization or deserialization takes place.
    /// </summary>
    /// <param name="request">The request content to transform.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The result of the task contains the transformed request content.</returns>
    protected virtual Task<string> TransformRequestAsync(string request, CancellationToken cancellation = default) => Task.FromResult(request);


    /// <summary>
    /// Transforms the RPC response content before deserialization by invoking custom transformations.
    /// </summary>
    /// <param name="content">The RPC response content to be transformed.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the transformed response content.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the content is null.</exception>
    protected virtual Task<string> TransformResponseAsync(string content, CancellationToken cancellation = default) => Task.FromResult(content);


    /// <summary>
    /// Invoked when the RPC command has been successfully invoked.
    /// </summary>
    /// <param name="requestDto">The request DTO that was sent to the server.</param>
    /// <param name="responseDto">The response DTO that was received from the server.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual Task OnInvokedAsync(object requestDto, object responseDto, CancellationToken cancellation) => Task.CompletedTask;

    /// <summary>
    /// Callback method that gets executed when a WhatIf transaction is reverted.
    /// </summary>
    /// <param name="requestDto">The DTO that was sent in the WhatIf transaction.</param>
    /// <param name="responseDto">The response DTO received from the WhatIf transaction.</param>
    /// <param name="cancellation">The cancellation token.</param>
    protected virtual Task OnRevertedAsync(object requestDto, object responseDto, CancellationToken cancellation) => Task.CompletedTask;

    /// <summary>
    /// Placeholder method that is called after a message has been sent successfully to a database queue.
    /// Override this method in a derived class to perform custom actions, such as logging or metrics collection.
    /// </summary>
    /// <param name="requestDto">The DTO object representing the request that was sent.</param>
    /// <param name="cancellation">A cancellation token that can be used to abort the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// The default implementation of this method does nothing and returns a completed task.
    /// </remarks>
    protected virtual Task OnSentAsync(
        object requestDto,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the metadata associated with this <see cref="DatabaseRpcCommand"/> instance.
    /// </summary>
    /// <remarks>
    /// This metadata includes information about the expected request and response types, 
    /// as well as the content types that they should be serialized to and from. This property 
    /// is read-only and can only be set via a constructor or other initialization method.
    /// </remarks>
    protected DatabaseRpcCommandMetadata Metadata { get; }

    /// <summary>
    /// Returns a string that represents the current <see cref="DatabaseRpcCommand"/> instance.
    /// </summary>
    /// <returns>
    /// A string that represents the current <see cref="DatabaseRpcCommand"/> instance.
    /// </returns>
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
        ThrowIf.Cancelled(cancellation);

        if (Metadata.Request.DtoType.IsInstanceOfType(dto))
        {
            string content = _serializer.Serialize(dto, Metadata.Request.ContentType);
            content = await TransformRequestAsync(content, cancellation);
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
/// Abstract base class for Database RPC commands that take a request DTO of type <typeparamref name="TRequest"/> and return a response DTO of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request DTO.</typeparam>
/// <typeparam name="TResponse">The type of the response DTO.</typeparam>
public abstract class DatabaseRpcCommand<TRequest, TResponse> 
    : DatabaseRpcCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseRpcCommand{TRequest, TResponse}"/> class with the specified database RPC provider and data contract serializer.
    /// </summary>
    /// <param name="client">The database RPC provider to use for sending requests.</param>
    /// <param name="serializer">The data contract serializer to use for serializing and deserializing DTOs.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if an invalid value is specified for an argument.</exception>
    [DebuggerNonUserCode]
    protected DatabaseRpcCommand(
        IDatabaseRpcProvider client, 
        IDataContractSerializer serializer) : base(client, serializer)
    {
    }


    /// <summary>
    /// Sends a request DTO of type <typeparamref name="TRequest"/> to the database and returns a response DTO of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <param name="requestDto">The request DTO to send to the database.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, which returns a response DTO of type <typeparamref name="TResponse"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="requestDto"/> is null.</exception>
    [DebuggerStepThrough]
    public async Task<TResponse> InvokeAsync(
        [DisallowNull] TRequest requestDto, 
        CancellationToken cancellation = default)
    {
        ThrowIf.ArgumentNull(requestDto, nameof(requestDto));
        ThrowIf.Cancelled(cancellation);
        var result = await base.InvokeAsync(requestDto, cancellation);
        return (TResponse)result;
    }

    /// <summary>
    /// Sends a request DTO of type <typeparamref name="TRequest"/> to the database and returns a response DTO of type <typeparamref name="TResponse"/>, but does not actually execute the command on the database.
    /// </summary>
    /// <param name="requestDto">The request DTO to send to the database.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, which returns a response DTO of type <typeparamref name="TResponse"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="requestDto"/> is null.</exception>
    [DebuggerStepThrough]
    public async Task<TResponse> WhatIfAsync(
        [DisallowNull] TRequest requestDto,
        CancellationToken cancellation = default)
    {
        ThrowIf.ArgumentNull(requestDto, nameof(requestDto));
        ThrowIf.Cancelled(cancellation);
        var result = await base.WhatIfAsync(requestDto, cancellation);
        return (TResponse)result;
    }

    /// <summary>
    /// Overrides the base method to provide a strongly typed version of <see cref="OnInvokedAsync(TRequest, TResponse, CancellationToken)"/>.
    /// </summary>
    /// <param name="requestDto">The request DTO.</param>
    /// <param name="responseDto">The response DTO.</param>
    /// <param name="cancellation">A cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [DebuggerStepThrough]
    protected sealed override Task OnInvokedAsync(object requestDto, object responseDto, CancellationToken cancellation) 
        => OnInvokedAsync((TRequest)requestDto, (TResponse)responseDto, cancellation);

    /// <summary>
    /// Overrides the base class method to handle a response that is received when the original request is canceled or an error occurs.
    /// This method casts the <paramref name="requestDto"/> and <paramref name="responseDto"/> parameters to their respective types
    /// (<typeparamref name="TRequest"/> and <typeparamref name="TResponse"/>) and calls the <see cref="OnRevertedAsync(TRequest, TResponse, CancellationToken)"/> method.
    /// </summary>
    /// <param name="requestDto">The original request DTO.</param>
    /// <param name="responseDto">The response DTO received after the request was canceled or an error occurred.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [DebuggerStepThrough]
    protected sealed override Task OnRevertedAsync(object requestDto, object responseDto, CancellationToken cancellation)
        => OnRevertedAsync((TRequest)requestDto, (TResponse)responseDto, cancellation);

    /// <summary>
    /// This method is called after a successful execution of the command.
    /// Override this method to implement any custom logic that needs to be executed after the command has been successfully executed.
    /// </summary>
    /// <param name="requestDto">The request object passed to the command.</param>
    /// <param name="responseDto">The response object returned by the command.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected virtual Task OnInvokedAsync(
        TRequest requestDto, 
        TResponse responseDto,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }


    /// <summary>
    /// An asynchronous method that is called after the command has been executed and the response has been received, deserialized, and reverted back to the original request DTO.
    /// </summary>
    /// <param name="requestDto">The original request DTO sent to the server.</param>
    /// <param name="responseDto">The response DTO received from the server.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual Task OnRevertedAsync(
        TRequest requestDto,
        TResponse responseDto,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Overrides the base class method to cast the request DTO to its generic type <typeparamref name="TRequest"/>,
    /// and pass it to the <see cref="OnSentAsync(TRequest, CancellationToken)"/> method for handling.
    /// </summary>
    /// <param name="requestDto">The request DTO.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="requestDto"/> is null.</exception>
    [DebuggerStepThrough]
    protected sealed override Task OnSentAsync(
        object requestDto, 
        CancellationToken cancellation) =>
        this.OnSentAsync((TRequest)requestDto, cancellation);

    /// <summary>
    /// This method is called after the request DTO has been sent to the remote service.
    /// Override this method to perform any additional logic or logging after the request has been sent.
    /// </summary>
    /// <param name="requestDto">The request DTO object.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request DTO is null.</exception>
    protected virtual Task OnSentAsync(TRequest requestDto, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }


    /// <summary>
    /// Sends a request of type <typeparamref name="TRequest"/> to a database queue and awaits for its acknowledgment.
    /// </summary>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is null.</exception>
    [DebuggerStepThrough]
    public Task SendAsync(TRequest request, CancellationToken cancellation) => base.SendAsync(request, cancellation);

    /// <summary>
    /// Sends a request of type <typeparamref name="TRequest"/> to a database queue and awaits for its acknowledgment.
    /// </summary>
    /// <param name="queue">The database queue.</param>
    /// <param name="dto">The request DTO to be sent.</param>
    /// <param name="config">The package configuration delegate.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dto"/> is null.</exception>
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

    /// <summary>
    /// Sends a request of type <typeparamref name="TRequest"/> to a database queue and awaits for its acknowledgment.
    /// </summary>
    /// <param name="queue">The database queue to send the request to.</param>
    /// <param name="dto">The request DTO to be sent to the database.</param>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellation"/> is cancelled.</exception>
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