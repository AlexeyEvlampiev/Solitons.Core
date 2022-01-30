using Solitons.Samples.Database.Models;

namespace Solitons.Samples.Database.Scripts.PostDeployment
{
    public partial class RegisterSuperuserRtt
    {
        private readonly IEnumerable<SuperuserSettings> _settings;

        public RegisterSuperuserRtt(IEnumerable<SuperuserSettings> emails)
        {
            _settings = emails.ThrowIfNullArgument(nameof(emails));
        }

        public string ValuesCsv => _settings.Select(e=> $"('{e.Email}', '{e.OrganizationId}'::uuid)").Join();
    }
}
