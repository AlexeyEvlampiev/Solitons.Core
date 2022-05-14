using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseRpcClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eTag"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IDatabaseApiInfo?> GetApiInfoAsync(string? eTag, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="content"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<string> InvokeAsync(
            IDatabaseApiCommandInfo command, 
            string content,
            CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="content"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task EnqueueAsync(
            IDatabaseApiCommandInfo command,
            string content,
            CancellationToken cancellation = default);
    }

}
