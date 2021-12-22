using Solitons.Common;
using Solitons.Samples.Domain;

namespace Solitons.Samples.Database.Scripts.PostDeployment
{
    public partial class RegisterHttpTriggersRtt
    {
        public RegisterHttpTriggersRtt()
        {
            var context = SampleDomainContext.GetOrCreate();
            var triggers = context.GetDatabaseExternalTriggerArgs<DatabaseHttpTriggerArgsAttribute>();
        }
    }
}
