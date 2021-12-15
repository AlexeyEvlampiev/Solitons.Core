
using System;
using System.Diagnostics;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Web.Common
{
    public abstract class RestApi : IRestApi
    {
        protected abstract IObservable<WebResponse> GetResponses(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation);

        protected virtual async Task<WebResponse> ProcessAsync(
            IWebRequest request, 
            IAsyncLogger logger,
            CancellationToken cancellation)
        {
            var result = await GetResponses(request, logger, cancellation)
                .SingleOrDefaultAsync()
                .ToTask(cancellation);
            return result ?? WebResponse.Create(HttpStatusCode.NotFound);
        }

        [DebuggerStepThrough]
        async Task<WebResponse> IRestApi.ProcessAsync(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            cancellation.ThrowIfCancellationRequested();
            try
            {
                return await ProcessAsync(request, logger, cancellation);
            }
            catch (Exception e)
            {
                var correlationId = Guid.NewGuid().ToString("N");
                await logger.ErrorAsync(e.Message, log => log
                    .WithTag(correlationId)
                    .WithProperty("correlationId", correlationId)
                    .WithDetails(e.ToString()));
                return WebResponse.Create(HttpStatusCode.InternalServerError, correlationId);
            }
            
        }

        [DebuggerStepThrough]
        IObservable<WebResponse> IRestApi.GetResponses(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            cancellation.ThrowIfCancellationRequested();
            return GetResponses(request, logger, cancellation) ?? throw new NullReferenceException($"{GetType()}.{nameof(GetResponses)} returned null.");
        }

    }
}
