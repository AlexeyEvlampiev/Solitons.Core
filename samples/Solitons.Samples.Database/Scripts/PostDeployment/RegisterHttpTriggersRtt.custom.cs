using Solitons.Samples.Domain;
using Solitons.Samples.Domain.Contracts;

namespace Solitons.Samples.Database.Scripts.PostDeployment
{
    public partial class RegisterHttpTriggersRtt
    {
        public RegisterHttpTriggersRtt()
        {
            var context = SampleDomainContext.GetOrCreate();
            var triggers = context.GetDatabaseExternalTriggerArgs<SampleDbHttpTriggerAttribute>();
        }
    }
}
