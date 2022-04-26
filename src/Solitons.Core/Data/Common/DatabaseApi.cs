using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DatabaseApi : IDatabaseApi
    {
        protected abstract Task<IHandler?> FindHandlerAsync(Guid commandId, string contentType);
        protected abstract IEnumerable<string> GetCommandContentTypes(Guid commandId);

        protected virtual string BuildResourceNotFoundMessage(Guid commandId)
        {
            throw new NotImplementedException();
        }

        protected virtual string BuildContentTypeNotSupportedMessage(string[] expectedTypes, string actualType)
        {
            throw new NotImplementedException();
        }

        protected virtual string BuildInvalidRequestMessage(string reason)
        {
            throw new NotImplementedException();
        }

        protected virtual string BuildInvalidResponseMessage(string reason)
        {
            throw new NotImplementedException();
        }


        [DebuggerStepThrough]
        async Task<MediaContent> IDatabaseApi.InvokeAsync(
            Guid commandId, 
            MediaContent request, 
            IDatabaseApiCallback callback,
            CancellationToken cancellation)
        {
            commandId = commandId.ThrowIfEmptyArgument(nameof(commandId));
            request = request.ThrowIfNullArgument(nameof(request));
            callback = callback.ThrowIfNullArgument(nameof(callback));
            cancellation.ThrowIfCancellationRequested();

            var route = await FindHandlerAsync(commandId, request.ContentType);
            if (route is null)
            {
                var expectedContentTypes = GetCommandContentTypes(commandId).ToArray();
                if (expectedContentTypes.Any())
                {
                    var contentTypeError = BuildContentTypeNotSupportedMessage(expectedContentTypes, request.ContentType);
                    callback.OnContentTypeNotSupported(contentTypeError);
                    throw new NotSupportedException(contentTypeError);
                }

                var resourceNotFoundError = BuildResourceNotFoundMessage(commandId);
                callback.OnResourceNotFound(resourceNotFoundError);
                throw new NotSupportedException(resourceNotFoundError);
            }

            if (false == route.IsValidRequest(request.Content, out string invalidRequestReason))
            {
                var invalidRequestError = BuildInvalidRequestMessage(invalidRequestReason);
                callback.OnInvalidRequest(invalidRequestError);
                throw new InvalidOperationException(invalidRequestError);
            }

            var response = await route.InvokeAsync(request.Content, cancellation);

            if (false == route.IsValidResponse(request.Content, out var invalidResponseReason))
            {
                var invalidResponseError = BuildInvalidResponseMessage(invalidResponseReason);
                callback.OnInvalidResponse(invalidResponseError);
                throw new InvalidOperationException(invalidResponseError);
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        protected interface IHandler
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="content"></param>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<MediaContent> InvokeAsync(string content, CancellationToken cancellation = default);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="content"></param>
            /// <param name="reason"></param>
            /// <returns></returns>
            bool IsValidRequest(string content, out string reason);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="content"></param>
            /// <param name="reason"></param>
            /// <returns></returns>
            bool IsValidResponse(string content, out string reason);
        }
    }
}
