using System.Diagnostics;
using Solitons.Collections;
using Solitons.Configuration;

namespace Solitons.Samples.Azure
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AzureActiveDirectoryB2CSettingsGroup : SettingsGroup
    {
        public const string ConfigurationSectionName = "AzureAdB2C";

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:Instance")]
        [Setting("Instance", IsRequired = true, Pattern = "(?i)Instance")]
        public string Instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:ClientId")]
        [Setting("ClientId", IsRequired = true, Pattern = "(?i)(?:Client|App(?:lication)?)Id")]
        public Guid ClientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:Domain")]
        [Setting("Domain", IsRequired = true, Pattern = "(?i)Domain")]
        public string Domain { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:Scopes")]
        [Setting("Scopes", IsRequired = true, Pattern = "(?i)Scopes?")]
        public string Scopes { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:SignUpSignInPolicyId")]
        [Setting("SignUpSignInPolicyId", IsRequired = true, Pattern = "(?i)(?:SignUp)?SignIn(?:Policy(?:Id)?)")]
        public string SignUpSignInPolicyId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static AzureActiveDirectoryB2CSettingsGroup Parse(string text)
        {
            return ThrowIf
                .ArgumentNullOrWhiteSpace(text, nameof(text))
                .Convert(Parse<AzureActiveDirectoryB2CSettingsGroup>);
        }
    }
}
