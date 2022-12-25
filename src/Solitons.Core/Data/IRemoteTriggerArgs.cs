using System;

namespace Solitons.Data
{
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
        public static IRemoteTriggerArgs CreateEmpty(Guid commandId) => new EmptyRemoteTriggerArgs(commandId.ThrowIfEmptyArgument(nameof(commandId)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandGuid"></param>
        /// <returns></returns>
        public static IRemoteTriggerArgs CreateEmpty(string commandGuid) => CreateEmpty(ThrowIf
            .NullOrWhiteSpaceArgument(commandGuid, nameof(commandGuid))
            .Convert(Guid.Parse));
    }
}