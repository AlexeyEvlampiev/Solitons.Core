using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;


public partial interface IHttpTransactionHandler
{
    Task<HttpResponseMessage> InvokeAsync(
        HttpRequestMessage request,
        HttpTransactionInterceptor interceptor,
        CancellationToken cancellation);
}

public partial interface IHttpTransactionHandler
{
    [DebuggerNonUserCode]
    public virtual HttpMessageHandler AsHttpMessageHandler(HttpTransactionInterceptor interceptor)
    {
        return new TxHttpMessageHandler(this, interceptor);
    }

    [DebuggerNonUserCode]
    public sealed HttpMessageHandler AsHttpMessageHandler(Func<IHttpTransactionCallback, Task> interceptor)
    {
        return AsHttpMessageHandler(OnCommitingAsync);
        [DebuggerStepThrough]
        Task OnCommitingAsync(IHttpTransactionCallback callback, CancellationToken _)
        {
            return interceptor.Invoke(callback);
        }
    }

    [DebuggerNonUserCode]
    public sealed HttpMessageHandler AsHttpMessageHandler()
    {
        return AsHttpMessageHandler(OnCommitingAsync);
        [DebuggerStepThrough]
        Task OnCommitingAsync(IHttpTransactionCallback callback, CancellationToken _)
        {
            return Task.CompletedTask;
        }
    }


    [DebuggerStepThrough]
    public sealed Task<HttpResponseMessage> InvokeAsync(
        HttpRequestMessage request,
        Func<IHttpTransactionCallback, Task> interceptor,
        CancellationToken cancellation)
    {
        return InvokeAsync(
            request, 
            OnCommitingAsync, 
            cancellation);

        [DebuggerStepThrough]
        Task OnCommitingAsync(IHttpTransactionCallback callback, CancellationToken _)
        {
            return interceptor.Invoke(callback);
        }
    }
}