

using System;
using System.Data;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DatabaseRpcCommandAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultContentType = "application/json";

        /// <summary>
        /// 
        /// </summary>
        public const string DefaultOperationTimeout = "00:00:30";

        /// <summary>
        /// 
        /// </summary>
        public DatabaseRpcCommandAttribute(string procedure)
        {
            Procedure = ThrowIf.NullOrWhiteSpaceArgument(procedure, nameof(procedure));
            RequestContentType = DefaultContentType;
            ResponseContentType = DefaultContentType;
            IsolationLevel = IsolationLevel.ReadCommitted;
            OperationTimeoutTimeSpan = TimeSpan.Parse(DefaultOperationTimeout);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Procedure { get; }

        /// <summary>
        /// 
        /// </summary>
        public string RequestContentType { get; init; } 

        /// <summary>
        /// 
        /// </summary>
        public string ResponseContentType { get; init; }
        /// <summary>
        /// 
        /// </summary>
        public IsolationLevel IsolationLevel { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public string OperationTimeout
        {
            get => OperationTimeoutTimeSpan.ToString();
            init => OperationTimeoutTimeSpan = TimeSpan.Parse(value ?? DefaultOperationTimeout);
        }

        internal TimeSpan OperationTimeoutTimeSpan { get; private set; }
    }
}
