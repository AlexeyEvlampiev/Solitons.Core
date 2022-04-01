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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="annotation"></param>
        /// <param name="payload"></param>
        /// <param name="serializer"></param>
        /// <param name="onResponse"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<object> InvokeAsync(
            DbCommandAttribute annotation, 
            object payload, 
            IDataContractSerializer serializer,
            Func<object, Task>? onResponse,
            CancellationToken cancellation);
    }

    public partial interface IDatabaseRpcProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T CreateProxy<T>(IDataContractSerializer serializer) where T : class
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            return DatabaseRpcDispatchProxy<T>.Create(this, serializer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public IDatabaseRpcProvider AsDatabaseRpcProvider() => DatabaseRpcProviderProxy.Wrap(this);
    }



}
