using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    public partial interface IDatabaseRpcProvider
    {
        Task<string> InvokeAsync(DbCommandAttribute annotation, string payload, CancellationToken cancellation);

        string Serialize(object request, string contentType);

        object Deserialize(string content, string contentType, Type type);
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
