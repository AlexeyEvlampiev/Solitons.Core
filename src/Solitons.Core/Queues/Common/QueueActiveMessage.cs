using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IQueueActiveMessage" />
    public abstract class QueueActiveMessage : IQueueActiveMessage
    {
        enum Status : int
        {
            Received = 0,
            Completed = 1,
            Abandoned = 2,
            Expired = 3,
            Unknown = 256
        }

        private int _status = (int)Status.Received;

        protected QueueActiveMessage(byte[] body)
        {
            Body = body.ThrowIfNullArgument(nameof(body));
        }

        protected abstract Task CompleteAsync(CancellationToken cancellation);
        protected abstract Task AbandonAsync(CancellationToken cancellation);

        protected abstract bool IsTransientError(Exception exception);

        public byte[] Body { get; }



        private Status ChangeStatus(Status value, Status comparand)
        {
            return (Status)Interlocked
                .CompareExchange(ref _status, (int)value, (int)comparand);
        }

        private Status ChangeStatus(Status value)
        {
            return (Status)Interlocked
                .Exchange(ref _status, (int)value);
        }


        [DebuggerStepThrough]
        async Task<bool> IQueueActiveMessage.CompleteAsync(bool throwIfAlreadyCompleted, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();

            switch (ChangeStatus(Status.Completed, Status.Received))
            {
                case Status.Received:
                    // OK
                    Debug.WriteLine("The message is being completed");
                    break;
                case Status.Completed:
                    return throwIfAlreadyCompleted
                        ? throw new InvalidOperationException($"Message is already completed.")
                        : false;
                case Status.Abandoned:
                    throw new InvalidOperationException($"Message could not be completed as it is abandoned.");
                case Status.Expired:
                    throw new InvalidOperationException("Message could not be completed as it's lock is expired.");
                case Status.Unknown:
                    throw new InvalidOperationException("Message could not be completed as it's state is unknown.");
                default:
                    throw new InvalidOperationException();
            }


            try
            {
                await CompleteAsync(cancellation);
                return true;
            }
            catch (QueueMessageOperationException e) when (e.CausedBy(this))
            {
                switch (e.ErrorCode)
                {
                    case (QueueServiceErrorCodes.MessageNotFound):
                        ChangeStatus(Status.Unknown);
                        throw;
                    case (QueueServiceErrorCodes.PopReceiptMismatch):
                        ChangeStatus(Status.Expired);
                        throw;
                    default:
                        ChangeStatus(Status.Unknown);
                        throw;
                }
            }
        }

        [DebuggerStepThrough]
        async Task<bool> IQueueActiveMessage.AbandonAsync(CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();

            var prevStatus = (Status)Interlocked
                .CompareExchange(ref _status, (int)Status.Completed, (int)Status.Abandoned);
            switch (prevStatus)
            {
                case Status.Expired:
                case Status.Abandoned:
                case Status.Completed: return false;
                case Status.Received: Debug.WriteLine("Abandoning a cloud queue message"); break;
                default: throw new InvalidOperationException();
            }

            try
            {
                await AbandonAsync(cancellation);
                return true;
            }
            catch (QueueMessageOperationException e) when (e.CausedBy(this))
            {
                Debug.WriteLine($"Message could not be abandoned. Error code: {e.ErrorCode}");
                return false;
            }
        }


        [DebuggerStepThrough]
        bool IQueueMessage.IsTransientError(Exception exception) => 
            exception is not null && IsTransientError(exception);


        [DebuggerStepThrough]
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (this is IQueueActiveMessage activeMessage)
            {
                bool abandoned = await activeMessage.AbandonAsync(CancellationToken.None);
                Debug.WriteLine(abandoned
                    ? "A Cloud Queue Message has been disposed by the provider abanding call."
                    : "A Cloud Queue Message is already disposed.");
            }
        }


    }
}
