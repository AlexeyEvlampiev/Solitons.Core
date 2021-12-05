
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
        protected abstract IObservable<IWebResponse> GetResponses(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation);

        protected virtual async Task<IWebResponse> ProcessAsync(
            IWebRequest request, 
            IAsyncLogger logger,
            CancellationToken cancellation)
        {
            var result = await GetResponses(request, logger, cancellation)
                .SingleOrDefaultAsync()
                .ToTask(cancellation);
            return result ?? IWebResponse.Create(HttpStatusCode.NotFound);
        }

        [DebuggerStepThrough]
        async Task<IWebResponse> IRestApi.ProcessAsync(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation)
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
                return IWebResponse.Create(HttpStatusCode.InternalServerError, correlationId);
            }
            
        }

        [DebuggerStepThrough]
        IObservable<IWebResponse> IRestApi.GetResponses(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            cancellation.ThrowIfCancellationRequested();
            return GetResponses(request, logger, cancellation) ?? throw new NullReferenceException($"{GetType()}.{nameof(GetResponses)} returned null.");
        }

    }
}
