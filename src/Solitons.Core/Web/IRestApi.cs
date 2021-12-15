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
        IObservable<WebResponse> GetResponses(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation);
        

        public async Task<WebResponse> ProcessAsync(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            cancellation.ThrowIfCancellationRequested();
            var result = await GetResponses(request, logger, cancellation)
                .Where(response=> response is not null)
                .FirstOrDefaultAsync()
                .ToTask(cancellation);
            return result ?? WebResponse.Create(HttpStatusCode.NotFound);
        }

    }
}
