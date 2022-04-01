using System;
using System.Text;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseRpcInvocationException : Exception
    {
        internal DatabaseRpcInvocationException(DbCommandAttribute annotation, Exception innerException) 
            : base(new StringBuilder("Database rpc invocation failed.")
                .Append($" Failed stored procedure call: {annotation.Procedure}.")
                .Append($" Command oid: {annotation.CommandId}.")
                .Append($" Error: {innerException.Message}")
                .ToString(), innerException)
        {
            Annotation = annotation;
        }

        /// <summary>
        /// 
        /// </summary>
        public DbCommandAttribute Annotation { get; }
    }
}
