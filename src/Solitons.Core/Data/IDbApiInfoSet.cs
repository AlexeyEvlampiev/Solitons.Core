using System;
using System.Collections.Generic;

namespace Solitons.Data
{
    public interface IDbApiInfoSet
    {
        IEnumerable<Guid> GetCommandIds();
        Guid GetRequestContractId(Guid commandId);
        Guid GetResponseContractId(Guid commandId);
        string GetRequestContentType(Guid commandId);
        string GetResponseContentType(Guid commandId);
        SchemaValidationCallback? GetSchemaValidationCallback(Guid contractId, string contentType);
    }
}
