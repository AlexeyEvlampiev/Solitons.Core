using System;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommandArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid CommandId => GetType().GUID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        public static ICommandArgs CreateEmpty(Guid commandId) => new EmptyCommandArgs(commandId.ThrowIfEmptyArgument(nameof(commandId)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        public static ICommandArgs CreateEmpty(string commandId) => CreateEmpty(Guid.Parse(commandId.ThrowIfNullOrWhiteSpaceArgument(nameof(commandId))));
    }
}