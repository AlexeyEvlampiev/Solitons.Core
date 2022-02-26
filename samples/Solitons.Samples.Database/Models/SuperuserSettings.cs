using System.Diagnostics;
using Solitons.Configuration;

namespace Solitons.Samples.Database.Models
{
    
    public sealed class SuperuserSettings : BasicSettings
    {
        [BasicSetting("Email", IsRequired = true, Pattern = "(?is)^email$")]
        public string Email { get; set; }

        [BasicSetting("OrganizationID", IsRequired = true, Pattern = "(?is)^org(?:anization(?:-?id)?)?$")]
        public Guid OrganizationId { get; set; }

        [DebuggerStepThrough]
        public static SuperuserSettings Parse(string text) => 
            Parse<SuperuserSettings>(text);

        [DebuggerStepThrough]
        public static string GetSynopsis() => GetSynopsis<SuperuserSettings>();
    }
}
