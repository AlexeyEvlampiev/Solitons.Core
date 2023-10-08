using System;
using System.Net.Http;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

public abstract class ReactiveAwaitableConnectedHttpMessageHandler : HttpMessageHandler
{

    protected abstract void OnHttpRequest(HttpTriggerEventArgs args);


    protected sealed override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        return await Observable
            .Create<HttpResponseMessage>(observer =>
            {
                var args = new HttpTriggerEventArgs(request, observer);
                OnHttpRequest(args);
                return Disposable.Empty;
            })
            .ToTask(cancellation);
    }

    protected sealed record HttpTriggerEventArgs
    {
        private readonly IObserver<HttpResponseMessage> _listener;

        public HttpTriggerEventArgs(HttpRequestMessage request, IObserver<HttpResponseMessage> listener)
        {
            Request = request;
            _listener = listener;
        }

        public HttpRequestMessage Request { get; }

        public void OnError(Exception error)
        {
            _listener.OnError(error);
        }

        public void OnComplete(HttpResponseMessage value)
        {
            _listener.OnNext(value);
            _listener.OnCompleted();
        }
    }
}
