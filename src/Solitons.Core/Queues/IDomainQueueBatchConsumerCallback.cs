using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDomainQueueBatchConsumerCallback
    {
        /// <summary>
        /// Gets the behaviour.
        /// </summary>
        /// <value>
        /// The behaviour.
        /// </value>
        QueueConsumerBehaviour RequiredBehaviour { get; }

        /// <summary>
        /// Called when [batch asynchronous].
        /// </summary>
        /// <param name="batch">The batch dto by message map.</param>
        /// <param name="logger">The logger</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task OnBatchAsync(
            IDictionary<IQueueMessage, object> batch,
            IAsyncLogger logger,
            CancellationToken cancellation = default);

        /// <summary>
        /// Called by queue consumers prior to starting the queue message batch pump.
        /// </summary>
        /// <param name="startingAttempt">The starting attempt.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task OnInitializedAsync(int startingAttempt, CancellationToken cancellation);

        /// <summary>
        /// Called when [transient data not found asynchronous].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="asyncLogger">The asynchronous logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task OnTransientDataNotFoundAsync(
            IQueueMessage message,
            IAsyncLogger asyncLogger,
            CancellationToken cancellation);

        /// <summary>
        /// Called when [message transient storage expired asynchronous].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task OnMessageTransientStorageExpiredAsync(
            IQueueMessage message,
            IAsyncLogger logger,
            CancellationToken cancellation);

        /// <summary>
        /// Called when [data contract not supported asynchronous].
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task OnDataContractNotSupportedAsync(
            IQueueMessage message,
            IAsyncLogger logger,
            CancellationToken cancellation);

        /// <summary>
        /// Gets the required batch size asynchronous.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task<int> GetRequiredBatchSizeAsync(
            int minValue,
            int maxValue,
            IAsyncLogger logger,
            CancellationToken cancellation = default);

        /// <summary>
        /// Gets the required message visibility timeout asynchronous.
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task<TimeSpan> GetRequiredMessageVisibilityTimeoutAsync(
            TimeSpan minValue,
            TimeSpan maxValue,
            IAsyncLogger logger,
            CancellationToken cancellation = default);

        /// <summary>
        /// Determines whether [is transient error] [the specified exception].
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        ///   <c>true</c> if [is transient error] [the specified exception]; otherwise, <c>false</c>.
        /// </returns>
        bool IsTransientError(Exception exception);


        /// <summary>
        /// Gets the batch receive delay asynchronous.
        /// </summary>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task<TimeSpan> GetBatchReceiveDelayAsync(int retryCount, IAsyncLogger logger, CancellationToken cancellation);

        /// <summary>
        /// Gets the activation retry delay asynchronous.
        /// </summary>
        /// <param name="startingAttempt">The starting attempt.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task<TimeSpan> GetActivationRetryDelayAsync(int startingAttempt, IAsyncLogger logger, CancellationToken cancellation);


        /// <summary>
        /// Whens the ready for activation retry asynchronous.
        /// </summary>
        /// <param name="retryCount">The starting attempt.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        [DebuggerStepThrough]
        public async Task WhenReadyForActivationRetryAsync(int retryCount, IAsyncLogger logger, CancellationToken cancellation = default)
            => await Task.Delay(await GetActivationRetryDelayAsync(retryCount, logger, cancellation), cancellation);

        /// <summary>
        /// Whens the ready for next dequeue attempt asynchronous.
        /// </summary>
        /// <param name="emptyResponseCount">The empty response count.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        [DebuggerStepThrough]
        public async Task WhenReadyForNextAttemptToReceiveMessagesAsync(int emptyResponseCount, IAsyncLogger logger, CancellationToken cancellation = default)
            => await Task.Delay(await GetBatchReceiveDelayAsync(emptyResponseCount, logger, cancellation), cancellation);

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="cancellation">The cancellation.</param>
        public async Task InitializeAsync(IAsyncLogger logger, CancellationToken cancellation)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            cancellation.ThrowIfCancellationRequested();

            for (int retryCount = 0; !cancellation.IsCancellationRequested; ++retryCount)
            {
                try
                {
                    await OnInitializedAsync(retryCount, cancellation);
                    return;
                }
                catch (Exception e) when (IsTransientError(e))
                {
                    Debug.WriteLine(e.Message);
                    await WhenReadyForActivationRetryAsync(retryCount, logger, cancellation);
                }
            }
        }

        Task OnMalformedMessageAsync(IQueueMessage message, IAsyncLogger logger, CancellationToken cancellation);
    }
}
