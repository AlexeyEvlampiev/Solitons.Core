using Solitons.Data;
using Solitons.Web.Common;
using System;
using System.Data;

namespace Solitons.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DatabaseHttpTriggerEventArgsAttribute : HttpEventArgsAttribute, IDatabaseExternalTriggerArgsAttribute
    {
        private TimeSpan _timeout = TimeSpan.FromSeconds(30);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionRegexp"></param>
        /// <param name="methodRegexp"></param>
        /// <param name="uriRegexp"></param>
        /// <param name="procedure"></param>
        protected DatabaseHttpTriggerEventArgsAttribute(string versionRegexp, string methodRegexp, string uriRegexp, string procedure) 
            : base(versionRegexp, methodRegexp, uriRegexp)
        {
            Procedure = procedure;
            IsolationLevel = IsolationLevel.ReadCommitted;
            ProcedureEventArgsContentType = "application/json";
            ProcedurePayloadContentType = "application/json";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodRegexp"></param>
        /// <param name="uriRegexp"></param>
        /// <param name="procedure"></param>
        protected DatabaseHttpTriggerEventArgsAttribute(string methodRegexp, string uriRegexp, string procedure)
            : this(".*", methodRegexp, uriRegexp, procedure)
        {
        }

        public string Procedure { get; }

        public IsolationLevel IsolationLevel { get; set; }

        TimeSpan IDatabaseExternalTriggerArgsAttribute.Timeout 
        { 
            get => _timeout;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DatabaseOperationTimeout
        {
            get => _timeout.ToString();
            set => _timeout = TimeSpan.Parse(value);
        }



        public string ProcedureEventArgsContentType { get; set; }

        public string ProcedurePayloadContentType { get; set; }
        public string Authorize { get; set; }
    }
}
