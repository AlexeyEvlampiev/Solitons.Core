using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DatabaseRpcProvider : IDatabaseRpcProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="annotation"></param>
        /// <returns></returns>
        protected abstract IRpcHandler BuildRpcCommand(DbCommandAttribute annotation);


        [DebuggerStepThrough]
        async Task<object> IDatabaseRpcProvider.InvokeAsync(
            DbCommandAttribute annotation,
            object request,
            IDataContractSerializer serializer,
            Func<object, Task>? onResponse,
            CancellationToken cancellation)
        {
            if (annotation == null) throw new ArgumentNullException(nameof(annotation));
            request = request.ThrowIfNullArgument(nameof(request));
            cancellation.ThrowIfCancellationRequested();

            var rpc = BuildRpcCommand(annotation);
            return await rpc.InvokeAsync(request, onResponse, cancellation);
        }


        /// <summary>
        /// 
        /// </summary>
        protected interface IRpcHandler
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="request"></param>
            /// <param name="onResponse"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<object> InvokeAsync(object request, Func<object, Task>? onResponse, CancellationToken cancellation);
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract class RpcHandler : IRpcHandler
        {
            private readonly IDataContractSerializer _serializer;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="annotation"></param>
            /// <param name="serializer"></param>
            /// <exception cref="ArgumentNullException"></exception>
            protected RpcHandler(DbCommandAttribute annotation, IDataContractSerializer serializer)
            {
                Annotation = annotation ?? throw new ArgumentNullException(nameof(annotation));
                _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            }

            /// <summary>
            /// 
            /// </summary>
            protected DbCommandAttribute Annotation { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="request"></param>
            /// <param name="onResponse"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            protected abstract Task<object> InvokeAsync(object request, Func<object, Task>? onResponse, CancellationToken cancellation);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            protected string SerializeRequest(object request) =>
                _serializer.Serialize(request, Annotation.RequestContentType);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="response"></param>
            /// <returns></returns>
            protected object DeserializeResponse(string response) =>
                _serializer.Deserialize(
                    Annotation.ResponseType, 
                    Annotation.ResponseContentType, 
                    response);

            [DebuggerStepThrough]
            Task<object> IRpcHandler.InvokeAsync(object request, Func<object, Task>? onResponse, CancellationToken cancellation)
            {
                if (request == null) throw new ArgumentNullException(nameof(request));
                cancellation.ThrowIfCancellationRequested();
                return InvokeAsync(request, onResponse, cancellation);
            }
        }
    }
}
