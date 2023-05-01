using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common;

/// <summary>
/// 
/// </summary>
public abstract class DatabaseRpcTransmissionHandler : LargeObjectQueueConsumerCallback
{
    private readonly IDatabaseRpcModule _module;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="module"></param>
    protected DatabaseRpcTransmissionHandler(
        IDatabaseRpcModule module)
    {
        _module = module;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="package"></param>
    /// <param name="dto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    protected abstract Task OnRpcCommandNotFoundAsync(
        DataTransferPackage package,
        object dto,
        CancellationToken cancellation);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="package"></param>
    /// <param name="dto"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    protected override Task ProcessAsync(
        DataTransferPackage package,
        object dto,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        if (package.IntentId.IsNullOrEmpty() || 
            false == _module.Contains(package.IntentId!.Value))
        {
            return OnRpcCommandNotFoundAsync(package, dto, cancellation);
        }

        return _module.SendAsync(package.IntentId!.Value, package, cancellation);
    }
}