using System;
using Solitons.Common;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class RemoteTriggerArgs : SerializationCallback, IRemoteTriggerArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intentId"></param>
        protected RemoteTriggerArgs(Guid intentId)
        {
            IntentId = intentId
                .ThrowIfEmptyArgument(nameof(intentId));
        }

        /// <summary>
        /// 
        /// </summary>
        protected Guid IntentId { get; }

        Guid IRemoteTriggerArgs.IntentId => IntentId;
    }
}
