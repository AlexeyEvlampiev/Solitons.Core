using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IDatabaseRpcProvider
    {
        Task<object> InvokeAsync(
            DbCommandAttribute annotation, 
            object payload, 
            CancellationToken cancellation);

        bool CanSerialize(Type type, string contentType);
    }

    public partial interface IDatabaseRpcProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T CreateProxy<T>() where T : class
        {
            return DatabaseRpcDispatchProxy<T>.Create(this);
        }

    }



}
