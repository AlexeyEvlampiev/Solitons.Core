using Solitons.Data;
using Solitons.Web.Common;
using System;
using System.Data;

namespace Solitons.Common
{
    public abstract class DatabaseHttpTriggerArgsAttribute : HttpEventArgsAttribute, IDatabaseExternalTriggerArgsAttribute
    {
        private TimeSpan _timout = TimeSpan.FromSeconds(30);

        protected DatabaseHttpTriggerArgsAttribute(string versionRegexp, string methodRegexp, string uriRegexp, string procedure) 
            : base(versionRegexp, methodRegexp, uriRegexp)
        {
            Procedure = procedure;
            IsolationLevel = IsolationLevel.ReadCommitted;
            ProcedureEventArgsContentType = "application/json";
            ProcedurePayloadContentType = "application/json";
        }

        public string Procedure { get; }

        public IsolationLevel IsolationLevel { get; set; }

        TimeSpan IDatabaseExternalTriggerArgsAttribute.Timeout 
        { 
            get => _timout;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DatabaseOperationTimeout
        {
            get => _timout.ToString();
            set => _timout = TimeSpan.Parse(value);
        }



        public string ProcedureEventArgsContentType { get; set; }

        public string ProcedurePayloadContentType { get; set; }
        public string Authorize { get; set; }
    }
}
