using System;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDistributedEventArgs
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
        public static IDistributedEventArgs CreateEmpty(Guid commandId) => new EmptyDistributedEventArgs(commandId.ThrowIfEmptyArgument(nameof(commandId)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandGuid"></param>
        /// <returns></returns>
        public static IDistributedEventArgs CreateEmpty(string commandGuid) => CreateEmpty(ThrowIf
            .NullOrWhiteSpaceArgument(commandGuid, nameof(commandGuid))
            .Convert(Guid.Parse));
    }
}