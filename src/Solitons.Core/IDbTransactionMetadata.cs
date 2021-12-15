using System;
using System.Data;
using Solitons.Web;

namespace Solitons
{
    public interface IDbTransactionMetadata
    {
        Guid Guid { get; }

        string Name { get; }

        string Description { get; }

        string Schema { get; }

        string Procedure { get; }

        IsolationLevel IsolationLevel { get; }

        TimeSpan OperationTimeout { get; }

        public T AsRestApi<T>() where T : IHttpEventArgsAttribute;
    }
}
