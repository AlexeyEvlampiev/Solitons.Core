using System;
using System.Data;
using System.Reflection;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class StoredProcedureAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedure"></param>
        public StoredProcedureAttribute(string procedure)
        {
            Procedure = procedure.ThrowIfNullOrWhiteSpaceArgument(nameof(procedure));
            OperationTimeoutInSeconds = 30;
            IsolationLevel = IsolationLevel.ReadCommitted;
        }

        public int OperationTimeoutInSeconds { get; init; }
        public IsolationLevel IsolationLevel { get; init; }
        public string Procedure { get; }

        public override string ToString()
        {
            return $"{Procedure}, {IsolationLevel}";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static StoredProcedureAttribute? Get(MethodInfo method) => method
            .ThrowIfNullArgument(nameof(method))
            .GetCustomAttribute<StoredProcedureAttribute>();
    }
}
