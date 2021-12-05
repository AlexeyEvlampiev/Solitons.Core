using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Web
{
    public interface IRestApi
    {
        IObservable<IWebResponse> GetResponses(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation);
        

        public async Task<IWebResponse> ProcessAsync(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            cancellation.ThrowIfCancellationRequested();
            var result = await GetResponses(request, logger, cancellation)
                .Where(response=> response is not null)
                .FirstOrDefaultAsync()
                .ToTask(cancellation);
            return result ?? IWebResponse.Create(HttpStatusCode.NotFound);
        }

        [DebuggerStepThrough]
        public static IRestApi Join(IEnumerable<IRestApi> scopes) => new UnionRestApi(scopes.ThrowIfNullArgument(nameof(scopes)));

        [DebuggerStepThrough]
        public static IRestApi Join(params IRestApi[] scopes) => Join(scopes
            .ThrowIfNullArgument(nameof(scopes))
            .AsEnumerable());
    }
}
