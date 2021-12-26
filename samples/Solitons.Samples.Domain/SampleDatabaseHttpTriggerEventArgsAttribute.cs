

using Solitons.Common;

namespace Solitons.Samples.Domain
{
    [SampleApi]
    public sealed class SampleDatabaseHttpTriggerEventArgsAttribute : DatabaseHttpTriggerEventArgsAttribute
    {
        public SampleDatabaseHttpTriggerEventArgsAttribute(string versionRegexp, string methodRegexp, string uriRegexp, string procedure) : base(versionRegexp, methodRegexp, uriRegexp, procedure)
        {
        }

        public SampleDatabaseHttpTriggerEventArgsAttribute(string methodRegexp, string uriRegexp, string procedure) : base(methodRegexp, uriRegexp, procedure)
        {
        }
    }
}
