using System;
using System.Data;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class DbCommandAttribute : Attribute
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="procedure"></param>
        public DbCommandAttribute(string guid, string procedure)
        {
            CommandId = Guid
                .Parse(guid)
                .ThrowIfEmptyArgument(nameof(guid));
            Procedure = procedure
                .ThrowIfNullOrWhiteSpaceArgument(nameof(procedure))
                .Trim();
            CommandTimeout = TimeSpan.FromSeconds(30);
            IsolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid CommandId { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Procedure { get; }

        public TimeSpan CommandTimeout { get;  }

        public string RequestContentType { get; init; } = "application/json";
        public string ResponseContentType { get; init; } = "application/json";

        public Type RequestType { get; internal set; }
        public Type ResponseType { get; internal set; }
        internal Func<object[], object> InvocationCallback { get; set; }
        public IsolationLevel IsolationLevel { get; }
    }
}
