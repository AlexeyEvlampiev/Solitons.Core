using System.Diagnostics;
using Solitons.Common;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseHttpTriggerEventArgsAttribute : DatabaseHttpTriggerArgsAttribute
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
