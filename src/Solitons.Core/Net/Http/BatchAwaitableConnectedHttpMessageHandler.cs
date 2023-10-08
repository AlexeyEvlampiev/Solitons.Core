using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

public abstract class BatchAwaitableConnectedHttpMessageHandler : ReactiveAwaitableConnectedHttpMessageHandler
{
    private readonly IObserver<HttpTriggerEventArgs> _observer;
    protected BatchAwaitableConnectedHttpMessageHandler(CancellationTokenSource cancellationSource)
    {

        var subject = new Subject<HttpTriggerEventArgs>();
        _observer = subject.AsObserver();
        subject.Subscribe(this.OnHttpRequest);
        subject
            .Buffer(TimeSpan.FromMilliseconds(100), 100)
            .SelectMany(batch => ProcessBatchAsync(batch, cancellationSource.Token).ToObservable())
            .Subscribe();
    }

    protected  abstract Task ProcessBatchAsync(IEnumerable<HttpTriggerEventArgs> batch, CancellationToken cancellation);

    protected sealed override void OnHttpRequest(HttpTriggerEventArgs args) => _observer.OnNext(args);
}