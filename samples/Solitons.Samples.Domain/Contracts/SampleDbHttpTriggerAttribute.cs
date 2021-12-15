using Solitons.Common;

namespace Solitons.Samples.Domain.Contracts
{
    public sealed class SampleDbHttpTriggerAttribute : DatabaseHttpTriggerArgsAttribute
    {
        public SampleDbHttpTriggerAttribute(string versionRegexp, string methodRegexp, string uriRegexp, string procedure) 
            : base(versionRegexp, methodRegexp, uriRegexp, procedure)
        {
        }
    }
}
