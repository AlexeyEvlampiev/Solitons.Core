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
        /// <param name="payload"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<string> InvokeAsync(DbCommandAttribute annotation, string payload, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        protected abstract string Serialize(object request, string contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract object Deserialize(string content, string contentType, Type type);

        [DebuggerStepThrough]
        Task<string> IDatabaseRpcProvider.InvokeAsync(DbCommandAttribute annotation, string payload, CancellationToken cancellation)
        {
            if (annotation == null) throw new ArgumentNullException(nameof(annotation));
            payload = payload.ThrowIfNullOrWhiteSpaceArgument(nameof(payload));
            cancellation.ThrowIfCancellationRequested();
            return InvokeAsync(annotation, payload, cancellation);
        }

        [DebuggerStepThrough]
        string IDatabaseRpcProvider.Serialize(object request, string contentType)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            contentType = contentType
                .ThrowIfNullOrWhiteSpaceArgument(nameof(contentType))
                .Trim();
            return Serialize(request, contentType);
        }

        [DebuggerStepThrough]
        object IDatabaseRpcProvider.Deserialize(string content, string contentType, Type type)
        {
            content = content.ThrowIfNullOrWhiteSpaceArgument(nameof(content));
            contentType = contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType));
            if (type == null) throw new ArgumentNullException(nameof(type));
            return Deserialize(content, contentType, type) ?? throw new NullReferenceException($"{GetType()}.{nameof(Deserialize)} returned null.");
        }
    }
}
