using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Web.Common;

namespace Solitons.Web
{
    sealed class UnionRestApi : RestApi
    {
        private readonly IObservable<IRestApi> _scopes;


        public UnionRestApi(IEnumerable<IRestApi> scopes)
        {
            if (scopes == null) throw new ArgumentNullException(nameof(scopes));
            _scopes = scopes
                .SkipNulls()
                .Distinct()
                .ToObservable();
        }


        [DebuggerStepThrough]
        protected override IObservable<IWebResponse> GetResponses(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation)
        {
            return _scopes
                .SelectMany(scope => scope.GetResponses(request, logger, cancellation));

        }

        [DebuggerStepThrough]
        protected override async Task<IWebResponse> ProcessAsync(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation)
        {
            var result = await GetResponses(request, logger, cancellation)
                .SingleOrDefaultAsync()
                .ToTask(cancellation);
            return result ?? IWebResponse.Create(HttpStatusCode.NotFound);
        }
    }
}
