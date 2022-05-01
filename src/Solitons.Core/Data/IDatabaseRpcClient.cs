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
        /// <param name="command"></param>
        /// <param name="content"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<string> InvokeAsync(
            DatabaseApiCommandInfo command, 
            string content,
            CancellationToken cancellation = default);
    }
}
