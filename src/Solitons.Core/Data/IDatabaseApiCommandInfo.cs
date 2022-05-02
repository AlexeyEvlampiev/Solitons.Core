using System;


namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseApiCommandInfo
    {
        /// <summary>
        /// 
        /// </summary>
        Guid CommandId { get; }

        /// <summary>
        /// 
        /// </summary>
        IDatabaseApiCommandDataContractInfo Request { get; }

        /// <summary>
        /// 
        /// </summary>
        IDatabaseApiCommandDataContractInfo Response { get; }

    }
}
