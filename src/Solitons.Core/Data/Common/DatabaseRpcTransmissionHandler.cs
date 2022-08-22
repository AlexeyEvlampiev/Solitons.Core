using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DatabaseRpcTransmissionHandler : LargeObjectQueueConsumerCallback
    {
        private readonly IDatabaseRpcCommandFactory _commandFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandFactory"></param>
        protected DatabaseRpcTransmissionHandler(
            IDatabaseRpcCommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
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
            var command = _commandFactory.Create(package.IntentId);
            if (command is null)
            {
                return OnRpcCommandNotFoundAsync(package, dto, cancellation);
            }

            return command.SendAsync(dto, cancellation);
        }
    }
}
