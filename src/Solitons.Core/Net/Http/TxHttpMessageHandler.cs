using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

sealed class TxHttpMessageHandler : HttpMessageHandler
{
    private readonly IHttpTransactionHandler _handler;
    private readonly HttpTransactionInterceptor _interceptor;


    public TxHttpMessageHandler(
        IHttpTransactionHandler handler,
        HttpTransactionInterceptor interceptor)
    {
        _handler = handler;
        _interceptor = interceptor;
    }

    [DebuggerStepThrough]
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return _handler.InvokeAsync(request, _interceptor, cancellation);
    }
}