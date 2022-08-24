using System;
using Solitons.Common;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DistributedEventArgs : SerializationCallback, IDistributedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intentId"></param>
        protected DistributedEventArgs(Guid intentId)
        {
            IntentId = intentId
                .ThrowIfEmptyArgument(nameof(intentId));
        }

        /// <summary>
        /// 
        /// </summary>
        protected Guid IntentId { get; }

        Guid IDistributedEventArgs.IntentId => IntentId;
    }
}
