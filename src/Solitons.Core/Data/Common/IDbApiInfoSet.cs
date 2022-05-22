using System;
using System.Collections.Generic;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDbApiInfoSet
    {
        /// <summary>
        /// 
        /// </summary>
        string ETag { get; }

        IEnumerable<Guid> GetCommandIds();
        Guid GetRequestContractId(Guid commandId);
        Guid GetResponseContractId(Guid commandId);
        string GetRequestContentType(Guid commandId);
        string GetResponseContentType(Guid commandId);
        SchemaValidationCallback? GetSchemaValidationCallback(Guid contractId, string contentType);
        bool IsSchedulable(Guid commandId);
    }
}
