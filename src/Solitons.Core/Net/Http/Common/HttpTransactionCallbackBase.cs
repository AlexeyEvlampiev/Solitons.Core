using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Net.Http.Common
{
    public abstract class HttpTransactionCallbackBase :
        IHttpTransactionCallback,
        IAsyncDisposable
    {
        protected HttpTransactionCallbackBase(
            HttpRequestMessage request,
            HttpResponseMessage response)
        {
            Request = request;
            Response = response;
        }

        public HttpRequestMessage Request { get; }
        public HttpResponseMessage Response { get; }

        public abstract Task<bool> RollbackIfActiveAsync();

        protected abstract Task<bool> CommitIfActiveAsync();

        [DebuggerStepThrough]
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await CommitIfActiveAsync();
        }
    }
}
