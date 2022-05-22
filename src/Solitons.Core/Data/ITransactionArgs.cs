using System;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransactionArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid TransactionTypeId => GetType().GUID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        public static ITransactionArgs CreateEmpty(Guid commandId) => new EmptyTransactionArgs(commandId.ThrowIfEmptyArgument(nameof(commandId)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        public static ITransactionArgs CreateEmpty(string commandId) => CreateEmpty(Guid.Parse(commandId.ThrowIfNullOrWhiteSpaceArgument(nameof(commandId))));
    }
}