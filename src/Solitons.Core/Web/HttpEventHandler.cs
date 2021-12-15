using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Web
{
    sealed class HttpEventHandler : IHttpEventHandler
    {
        private readonly IDomainSerializer _serializer;
        private readonly IDomainHttpEventHandler[] _handlers;
        private readonly IHttpEventHandlerCallback _callback;

        public HttpEventHandler(
            DomainContext context,
            IEnumerable<IDomainHttpEventHandler> handlers,
            IHttpEventHandlerCallback callback)
        {
            _serializer = context
                .GetSerializer()
                .ThrowIfNull(() => new NullReferenceException($"{context.GetType()}.{nameof(context.GetSerializer)}() returned null."));

            _handlers = handlers
                .ThrowIfNullArgument(nameof(handlers))
                .ToArray();
            _callback = callback
                .ThrowIfNullArgument(nameof(callback));
        }


        public async Task<WebResponse> InvokeAsync(
            IWebRequest request,
            IAsyncLogger logger,
            CancellationToken cancellation)
        {
            var domainWebRequest = await _serializer.AsDomainWebRequestAsync(request);
            if (domainWebRequest == null)
                return WebResponse.Create(HttpStatusCode.NotFound);

            logger = logger
                .WithProperty("httpEventArgs", domainWebRequest.HttpEventArgs.GetType().ToString());

            var handler = _handlers
                    .Where(h => h.CanProcess(domainWebRequest))
                    .Do((handler, count) => _callback.OnFoundMultipleRoutes(domainWebRequest, logger))
                    .FirstOrDefault();

            if (handler is null)
            {
                _callback.OnFoundNoRoutes(domainWebRequest, logger);
                return WebResponse.Create(HttpStatusCode.NotFound);
            }


            var response = await handler.InvokeAsync(domainWebRequest, logger, cancellation);
            if (response is null)
            {
                _callback.OnNullResponseObject(domainWebRequest, logger);
                return WebResponse.Create(HttpStatusCode.NotFound);
            }

            return response;
        }

    }
}
