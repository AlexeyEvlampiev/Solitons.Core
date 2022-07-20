using System;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseRpcCommandFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandOid"></param>
        /// <returns></returns>
        IDatabaseRpcCommand? Create(Guid commandOid);

    }
}
