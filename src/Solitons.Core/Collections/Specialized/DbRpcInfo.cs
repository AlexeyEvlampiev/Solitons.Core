using System;
using System.Collections.Generic;
using System.Data;

namespace Solitons.Collections.Specialized
{
    public sealed record DbRpcInfo
    {
        internal DbRpcInfo(Guid oid, string procedure, Type requestType, Type responseType)
        {
            Oid = oid;
            Procedure = procedure;
            RequestType = requestType;
            ResponseType = responseType;
            RequestContentType = "application/json";
            ResponseContentType = "application/json";
            IsolationLevel = IsolationLevel.ReadCommitted;
            CommandTimeout = TimeSpan.FromSeconds(30);
            Id = procedure;
            Description = procedure;
        }

        

        public Guid Oid { get; }
        public string Procedure { get; }
        public Type RequestType { get; }
        public Type ResponseType { get; }
        public string Description { get; internal set; }

        public IsolationLevel IsolationLevel { get; internal set; }

        public bool EnableAsyncExecution { get; internal set; }

        public IEnumerable<string> AuthorizedRoles { get; internal set; }

        public string RequestContentType { get; internal set; }
        public string ResponseContentType { get; internal set; }

        public TimeSpan CommandTimeout { get; internal set; }

        public string Id { get; internal set; }
    }
}
