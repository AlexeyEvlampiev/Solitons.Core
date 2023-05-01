using System;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public interface IRemoteTriggerArgs
{
    /// <summary>
    /// 
    /// </summary>
    public Guid IntentId => GetType().GUID;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="commandId"></param>
    /// <returns></returns>
    public static IRemoteTriggerArgs CreateEmpty(Guid commandId) => new EmptyRemoteTriggerArgs(
        ThrowIf.ArgumentNullOrEmpty(commandId));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="commandGuid"></param>
    /// <returns></returns>
    public static IRemoteTriggerArgs CreateEmpty(string commandGuid) => CreateEmpty(ThrowIf
        .ArgumentNullOrWhiteSpace(commandGuid, nameof(commandGuid))
        .Convert(Guid.Parse));
}