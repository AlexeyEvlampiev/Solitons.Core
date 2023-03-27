using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http.Common
{
    public abstract class HttpTransactionHandler : IHttpTransactionHandler
    {
        async Task<HttpResponseMessage> IHttpTransactionHandler.InvokeAsync(
            HttpRequestMessage request,
            HttpTransactionInterceptor interceptor,
            CancellationToken cancellation)
        {
            ThrowIf.ArgumentNull(request);
            ThrowIf.Cancelled(cancellation);
            await using var callback = await ExecuteAsync(request, cancellation);
            await interceptor.Invoke(callback, cancellation);
            return callback.Response;
        }

        protected abstract Task<HttpTransactionCallbackBase> ExecuteAsync(
            HttpRequestMessage request,
            CancellationToken cancellation);



        [DebuggerNonUserCode]
        public virtual HttpMessageHandler AsHttpMessageHandler(HttpTransactionInterceptor interceptor)
        {
            return new TxHttpMessageHandler(this, interceptor);
        }
    }
}
