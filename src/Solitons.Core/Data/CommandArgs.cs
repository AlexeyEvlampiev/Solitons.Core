using System;
using Solitons.Common;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandArgs : SerializationCallback, ICommandArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        protected CommandArgs(Guid commandId)
        {
            CommandId = commandId
                .ThrowIfEmptyArgument(nameof(commandId));
        }

        /// <summary>
        /// 
        /// </summary>
        protected Guid CommandId { get; }

        Guid ICommandArgs.CommandId => CommandId;
    }
}
