using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainQueueStreamConsumerCallback
    {
        /// <summary>
        /// Gets the behaviour.
        /// </summary>
        QueueConsumerBehaviour RequiredBehaviour { get; }

        /// <summary>
        /// Called by queue consumers prior to starting the queue message pump.
        /// </summary>
        /// <param name="retriesCount">The starting attempts count.</param>
        /// <param name="logger"></param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task OnInitializedAsync(int retriesCount, IAsyncLogger logger, CancellationToken cancellation);

        /// <summary>
        /// Called when a new message is received.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="message">The message.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task OnMessageAsync(object dto, IQueueMessage message, IAsyncLogger logger, CancellationToken cancellation = default);

        /// <summary>
        /// Called when [unsupported data contract asynchronous].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task OnUnsupportedDataContractAsync(IQueueMessage message, IAsyncLogger logger, CancellationToken cancellation = default);

        /// <summary>
        /// Called when [serialization exception asynchronous].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task OnSerializationExceptionAsync(IQueueMessage message, Exception exception, IAsyncLogger logger, CancellationToken cancellation = default);


        /// <summary>
        /// Gets the delay before next receive attempt asynchronous.
        /// </summary>
        /// <param name="retryCount">The retries count.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task<TimeSpan> GetMessageReceiveDelayAsync(int retryCount, IAsyncLogger logger, CancellationToken cancellation = default);

        /// <summary>
        /// Gets the required message visibility timeout asynchronous.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task<TimeSpan> GetRequiredMessageVisibilityTimeoutAsync(TimeSpan minValue, TimeSpan maxValue, IAsyncLogger logger, CancellationToken cancellation = default);


        /// <summary>
        /// Determines whether [is transient error] [the specified e].
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns>
        ///   <c>true</c> if [is transient error] [the specified e]; otherwise, <c>false</c>.
        /// </returns>
        bool IsTransientError(Exception e);

        /// <summary>
        /// Gets the starting up retry delay asynchronous.
        /// </summary>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task<TimeSpan> GetActivationRetryDelayAsync(int retryCount, IAsyncLogger logger, CancellationToken cancellation);

        /// <summary>
        /// Whens the ready for next attempt to receive asynchronous.
        /// </summary>
        /// <param name="emptyResponseCount">The empty response count.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        [DebuggerStepThrough]
        public async Task WhenReadyForNextDequeueAttemptAsync(int emptyResponseCount, IAsyncLogger logger, CancellationToken cancellation = default)
            => await Task.Delay(await GetMessageReceiveDelayAsync(emptyResponseCount, logger, cancellation), cancellation);

        /// <summary>
        /// Whens the ready to retry starting up asynchronous.
        /// </summary>
        /// <param name="startingAttempt">The starting attempt.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        [DebuggerStepThrough]
        public async Task WhenReadyToRetryStartingUpAsync(int startingAttempt, IAsyncLogger logger, CancellationToken cancellation = default)
            => await Task.Delay(await GetActivationRetryDelayAsync(startingAttempt, logger, cancellation), cancellation);

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        public async Task InitializeAsync(IAsyncLogger logger, CancellationToken cancellation)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            cancellation.ThrowIfCancellationRequested();
            for (int attempt = 0; !cancellation.IsCancellationRequested; ++attempt)
            {
                try
                {
                    await OnInitializedAsync(attempt, logger, cancellation);
                    return;
                }
                catch (Exception e) when (IsTransientError(e))
                {
                    Debug.WriteLine(e.Message);
                    await WhenReadyToRetryStartingUpAsync(attempt, logger, cancellation);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        public delegate Task ManagedHandler(IQueueMessage message, IAsyncLogger logger, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logger"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task OnMalformedMessageAsync(IQueueMessage message, IAsyncLogger logger, CancellationToken cancellation);
    }
}
