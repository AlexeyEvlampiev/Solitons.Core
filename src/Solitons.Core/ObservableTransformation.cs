using System;

namespace Solitons
{
    public delegate IObservable<TResult> ObservableTransformation<in TSource, out TResult>(
        IObservable<TSource> results);

    public delegate IObservable<TSignal> InvocationRetrySignalFactory<out TSignal>(
        IObservable<Exception> results);
}
