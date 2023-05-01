using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public abstract class MessagingClient
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="commandId"></param>
    /// <param name="correlationId"></param>
    /// <returns></returns>
    protected abstract DataTransferPackage Pack(object request, Guid commandId, Guid correlationId);



    /// <summary>
    /// 
    /// </summary>
    /// <param name="package"></param>
    /// <returns></returns>
    protected abstract object Unpack(DataTransferPackage package);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="correlationId"></param>
    /// <returns></returns>
    protected abstract IObservable<DataTransferPackage> GetResponses(Guid correlationId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    protected abstract Task SendAsync(DataTransferPackage request, CancellationToken cancellation);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <param name="correlationId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected virtual Task OnCompletedAsync(DataTransferPackage request, DataTransferPackage response, Guid correlationId)
    {
        var cmp = StringComparer.Ordinal;
        if (false == cmp.Equals(request.CorrelationId, correlationId) ||
            false == cmp.Equals(response.CorrelationId, correlationId))
        {
            throw new InvalidOperationException("RPC call integrity check failed");
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="transactionTypeId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public IObservable<BrokeredResponse> GetResponses(object request, Guid transactionTypeId)
    {
        request = ThrowIf.ArgumentNull(request, nameof(request));
        transactionTypeId = ThrowIf.ArgumentNullOrEmpty(transactionTypeId);

        var correlationId = Guid.NewGuid();
        var requestPackage = Pack(request, transactionTypeId, correlationId);

        if (requestPackage.IntentId != transactionTypeId)
            throw new InvalidOperationException($"");

        return Observable.Create<BrokeredResponse>(observer =>
        {
            var subscription = GetResponses(correlationId)
                .Do(responsePackage => OnCompletedAsync(requestPackage, responsePackage, correlationId))
                .Select(package =>
                {
                    var dto = Unpack(package);
                    return new BrokeredResponse(dto, package);
                })
                .Subscribe(observer);

            try
            {
                SendAsync(requestPackage, CancellationToken.None);
                return subscription;
            }
            catch (Exception)
            {
                subscription.Dispose();
                throw;
            }
                
        });

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="commandId"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    public async Task<BrokeredResponse> GetResponseAsync(object request, Guid commandId, CancellationToken cancellation = default)
    {
        request = ThrowIf.ArgumentNull(request, nameof(request));
        commandId = ThrowIf.ArgumentNullOrEmpty(commandId);
        cancellation.ThrowIfCancellationRequested();

        return await GetResponses(request, commandId)
            .FirstAsync()
            .ToTask(cancellation);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public Task<BrokeredResponse> GetResponseAsync(IRemoteTriggerArgs args, CancellationToken cancellation = default)
    {
        ThrowIf.ArgumentNull(args, nameof(args));
        cancellation.ThrowIfCancellationRequested();
        return GetResponseAsync(args, args.IntentId, cancellation);
    }
}