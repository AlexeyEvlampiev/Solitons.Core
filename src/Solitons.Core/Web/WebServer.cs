using Solitons.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Web
{
    sealed class WebServer : IWebServer
    {
        private readonly IDomainSerializer _serializer;
        private readonly IObservable<IHttpEventHandler> _listeners;
        private readonly IClaimsTransformation[] _claimsTransformations;

        public WebServer(DomainContext context, IEnumerable<IHttpEventHandler> handlers)
        {
            context.ThrowIfNullArgument(nameof(context));
            _serializer = context.GetSerializer();
            _listeners = handlers
                .ThrowIfNullArgument(nameof(handlers))
                .ToArray()
                .ToObservable();
            _claimsTransformations = handlers
                .OfType<IClaimsTransformation>()
                .ToArray();
        }

        [DebuggerStepThrough]
        public async Task<WebResponse> InvokeAsync(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation = default)
        {
            request.ThrowIfNullArgument(nameof(request));
            logger.ThrowIfNullArgument(nameof(logger));
            cancellation.ThrowIfCancellationRequested();

            var caller = request.Caller;
            foreach(var transform in _claimsTransformations)
            {
                caller = await transform.TransformAsync(caller);
                cancellation.ThrowIfCancellationRequested();
            }

            try
            {
                var dtoRequest = await _serializer
                    .AsDomainWebRequestAsync(request);
                if(dtoRequest is null) return WebResponse.Create(System.Net.HttpStatusCode.NotFound);
                var response = await _listeners
                    .SelectMany(listener=> listener.ToObservable(dtoRequest, logger))
                    .LastOrDefaultAsync()
                    .ToTask(cancellation);
                response ??= WebResponse.Create(HttpStatusCode.NotFound);
                return response;
            }
            catch (ClaimNotFoundException ex)
            {
                await logger.ErrorAsync(ex.Message, log=> log
                    .WithDetails(ex.ToString())
                    .WithProperty(nameof(ClaimTypes), ex.ClaimType));
                var status = request.Caller.Identities.Any() 
                    ? HttpStatusCode.Forbidden 
                    : HttpStatusCode.Unauthorized;
                return WebResponse.Create(status);
            }
            catch (QueryParameterNotFoundException ex)
            {
                await logger.ErrorAsync(ex.Message, log => log
                    .WithDetails(ex.ToString())
                    .WithProperty(nameof(ex.QueryParameterName), ex.QueryParameterName));
                return WebResponse.Create(HttpStatusCode.BadRequest, $"{ex.QueryParameterName} query parameter is required");
            }
            catch (Exception ex)
            {
                var correlation = Guid.NewGuid().ToString("N");
                await logger.ErrorAsync(ex.Message, log => log
                    .WithDetails(ex.ToString())
                    .WithProperty("CorrelationID", correlation));
                return WebResponse.Create(HttpStatusCode.InternalServerError, $"Correlation ID: {correlation}");
            }
        }
    }
}
