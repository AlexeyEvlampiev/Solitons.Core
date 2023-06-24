using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Solitons.Reactive;

/// <summary>
/// Represents an Observable that applies a retry policy to another Observable.
/// </summary>
/// <typeparam name="T">The type of elements in the sequence.</typeparam>
sealed class RetryPolicyObservable<T> : ObservableBase<T>
{
    private readonly IObservable<T> _source;

    /// <summary>
    /// Initializes a new instance of the RetryPolicyObservable class.
    /// </summary>
    /// <param name="source">The Observable to apply the retry policy to.</param>
    /// <param name="handler">The handler that implements the retry policy.</param>
    [DebuggerStepThrough]
    public RetryPolicyObservable(IObservable<T> source, Func<RetryPolicyArgs, Task<bool>> handler)
    {
        _source = Observable.Create<T>([DebuggerStepThrough] async (observer, cancellation) =>
        {
            var start = DateTimeOffset.UtcNow;
            for (int counter = 0;; ++counter)
            {
                try
                {
                    await source
                        .Do(observer.OnNext)
                        .LastOrDefaultAsync();
                    observer.OnCompleted();
                    return;
                }
                catch (OperationCanceledException ex)
                {
                    observer.OnError(ex);
                    return;
                }
                catch (Exception e) 
                {
                    var attempt = new RetryPolicyArgs(e, counter, start);
                    if (await handler.Invoke(attempt))
                    {
                        continue;
                    }
                    observer.OnError(e);
                    return;
                }
                
            }
        });
    }

    /// <summary>
    /// Subscribe the observer to the source Observable.
    /// </summary>
    /// <param name="observer">The observer that will receive notifications from the source Observable.</param>
    /// <returns>A disposable object that can be used to unsubscribe the observer from the source Observable.</returns>
    protected override IDisposable SubscribeCore(IObserver<T> observer) => _source.Subscribe(observer);
}
