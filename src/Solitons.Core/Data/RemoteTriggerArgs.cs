using System;
using Solitons.Common;

namespace Solitons.Data;

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
        IntentId = ThrowIf.ArgumentNullOrEmpty(intentId);
    }

    /// <summary>
    /// 
    /// </summary>
    protected Guid IntentId { get; }

    Guid IRemoteTriggerArgs.IntentId => IntentId;
}