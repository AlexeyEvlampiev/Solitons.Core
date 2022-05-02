using System;

namespace Solitons.Data.Common
{
    

    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseApiCommandInfo : IDatabaseApiCommandInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="infoSet"></param>
        internal DatabaseApiCommandInfo(Guid commandId, IDbApiInfoSet infoSet)
        {
            CommandId = commandId.ThrowIfEmptyArgument(nameof(commandId));
            Request = new DatabaseApiCommandDataContractInfo(
                infoSet.GetRequestContractId(commandId),
                infoSet.GetRequestContentType(commandId), 
                infoSet);
            Response = new DatabaseApiCommandDataContractInfo(
                infoSet.GetResponseContractId(commandId),
                infoSet.GetResponseContentType(commandId),
                infoSet);

        }

        /// <summary>
        /// 
        /// </summary>
        public Guid CommandId { get; }

        /// <summary>
        /// 
        /// </summary>
        public DatabaseApiCommandDataContractInfo Request { get; }

        /// <summary>
        /// 
        /// </summary>
        public DatabaseApiCommandDataContractInfo Response { get; }


        IDatabaseApiCommandDataContractInfo IDatabaseApiCommandInfo.Request => Request;
        IDatabaseApiCommandDataContractInfo IDatabaseApiCommandInfo.Response => Response;

    }
}
