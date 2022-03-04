using System.Diagnostics;
using Solitons.Configuration;

namespace Solitons.Samples.Database.Models
{
    
    public sealed class SuperuserSettingsGroup : SettingsGroup
    {
        [Setting("Email", IsRequired = true, Pattern = "(?is)^email$")]
        public string Email { get; set; }

        [Setting("OrganizationID", IsRequired = true, Pattern = "(?is)^org(?:anization(?:-?id)?)?$")]
        public Guid OrganizationId { get; set; }

        [DebuggerStepThrough]
        public static SuperuserSettingsGroup Parse(string text) => 
            Parse<SuperuserSettingsGroup>(text);

        [DebuggerStepThrough]
        public static string GetSynopsis() => GetSynopsis<SuperuserSettingsGroup>();
    }
}
