using System;
using System.Diagnostics;
using Solitons.Common;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class DatabaseHttpTriggerEventArgsAttribute : DbHttpTriggerEventArgsAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionRegexp"></param>
        /// <param name="methodRegexp"></param>
        /// <param name="uriRegexp"></param>
        /// <param name="procedure"></param>
        [DebuggerStepThrough]
        public DatabaseHttpTriggerEventArgsAttribute(
            string versionRegexp, 
            string methodRegexp, 
            string uriRegexp, 
            string procedure) : base(versionRegexp, methodRegexp, uriRegexp, procedure)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodRegexp"></param>
        /// <param name="uriRegexp"></param>
        /// <param name="procedure"></param>
        [DebuggerStepThrough]
        public DatabaseHttpTriggerEventArgsAttribute(
            string methodRegexp,
            string uriRegexp,
            string procedure) : this(".*", methodRegexp, uriRegexp, procedure)
        {
            
        }
    }
}
