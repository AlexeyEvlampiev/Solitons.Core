using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UpdateChecker<T> : ObservableBase<T> where T : class
    {
        private readonly IObservable<Unit> _updateCheckTrigger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCheckInterval"></param>
        [DebuggerStepThrough]
        protected UpdateChecker(TimeSpan updateCheckInterval) : this(Observable.Defer(()=> Observable
            .Timer(DateTimeOffset.UtcNow, updateCheckInterval)
            .Select(_=>Unit.Default)))
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateCheckTrigger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected UpdateChecker(IObservable<Unit> updateCheckTrigger)
        {
            _updateCheckTrigger = updateCheckTrigger ?? throw new ArgumentNullException(nameof(updateCheckTrigger));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<State> LoadLatestStateAsync(State? currentState, CancellationToken cancellation);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        protected sealed override IDisposable SubscribeCore(IObserver<T> observer) => Observable
            .Create<T>(RunAsync)
            .Subscribe(observer);


        private async Task RunAsync(IObserver<T> observer, CancellationToken cancellation)
        {
            var cachedState = await LoadLatestStateAsync(null, cancellation);
            observer.OnNext(cachedState.Value);
            await _updateCheckTrigger
                .FirstAsync()
                .ToTask(cancellation);

            while (true)
            {
                cancellation.ThrowIfCancellationRequested();

                var currentState = await LoadLatestStateAsync(cachedState, cancellation);
                if (currentState is null)
                {
                    throw new NullReferenceException($"{GetType()}.{nameof(LoadLatestStateAsync)} returned null.");
                }

                if (false == currentState.Equals(cachedState))
                {
                    cachedState = currentState;
                    observer.OnNext(currentState.Value);
                }

                await _updateCheckTrigger
                    .FirstAsync()
                    .ToTask(cancellation);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected sealed record State
        {
            /// <summary>
            /// 
            /// </summary>
            public T Value { get; }

            /// <summary>
            /// 
            /// </summary>
            public string ETag { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="eTag"></param>
            /// <exception cref="ArgumentNullException"></exception>
            [DebuggerNonUserCode]
            public State(T value, string eTag)
            {
                Value = value ?? throw new ArgumentNullException(nameof(value));
                ETag = eTag.ThrowIfNullOrWhiteSpaceArgument(nameof(eTag));
            }
        }
    }
}
