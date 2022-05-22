using System;
using Solitons.Common;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionArgs : SerializationCallback, ITransactionArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        protected TransactionArgs(Guid commandId)
        {
            CommandId = commandId
                .ThrowIfEmptyArgument(nameof(commandId));
        }

        /// <summary>
        /// 
        /// </summary>
        protected Guid CommandId { get; }

        Guid ITransactionArgs.TransactionTypeId => CommandId;
    }
}
