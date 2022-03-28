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
        protected abstract IRpcCommand BuildRpcCommand(DbCommandAttribute annotation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        protected abstract bool CanSerialize(Type type, string contentType);

        [DebuggerStepThrough]
        async Task<object> IDatabaseRpcProvider.InvokeAsync(DbCommandAttribute annotation, object request, CancellationToken cancellation)
        {
            if (annotation == null) throw new ArgumentNullException(nameof(annotation));
            request = request.ThrowIfNullArgument(nameof(request));
            cancellation.ThrowIfCancellationRequested();

            var rpc = BuildRpcCommand(annotation);
            return await rpc.InvokeAsync(request, cancellation);
        }

        [DebuggerStepThrough]
        bool IDatabaseRpcProvider.CanSerialize(Type type, string contentType)
        {
            return false == contentType.IsNullOrWhiteSpace() &&
                   CanSerialize(type, contentType);
        }

        /// <summary>
        /// 
        /// </summary>
        protected interface IRpcCommand
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<object> InvokeAsync(object request, CancellationToken cancellation);
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract class RpcCommand : IRpcCommand
        {
            private readonly IDataContractSerializer _serializer;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="annotation"></param>
            /// <param name="serializer"></param>
            /// <exception cref="ArgumentNullException"></exception>
            protected RpcCommand(DbCommandAttribute annotation, IDataContractSerializer serializer)
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
            /// <param name="cancellation"></param>
            /// <returns></returns>
            protected abstract Task<object> InvokeAsync(object request, CancellationToken cancellation);

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
            Task<object> IRpcCommand.InvokeAsync(object request, CancellationToken cancellation)
            {
                if (request == null) throw new ArgumentNullException(nameof(request));
                cancellation.ThrowIfCancellationRequested();
                return InvokeAsync(request, cancellation);
            }
        }
    }
}
