using Solitons.Samples.Database.Models;

namespace Solitons.Samples.Database.Scripts.PostDeployment
{
    public partial class RegisterSuperuserRtt
    {
        private readonly IEnumerable<SuperuserSettingsGroup> _settings;

        public RegisterSuperuserRtt(IEnumerable<SuperuserSettingsGroup> emails)
        {
            _settings = ThrowIf.NullArgument(emails, nameof(emails));
        }

        public string ValuesCsv => _settings.Select(e=> $"('{e.Email}', '{e.OrganizationId}'::uuid)").Join();
    }
}
