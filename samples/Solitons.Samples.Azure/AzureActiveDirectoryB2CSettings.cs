using System.Diagnostics;
using Solitons.Collections;

namespace Solitons.Samples.Azure
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AzureActiveDirectoryB2CSettings : BasicSettings
    {
        public const string ConfigurationSectionName = "AzureAdB2C";

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:Instance")]
        [BasicSetting("Instance", IsRequired = true, Pattern = "(?i)Instance")]
        public string Instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:ClientId")]
        [BasicSetting("ClientId", IsRequired = true, Pattern = "(?i)(?:Client|App(?:lication)?)Id")]
        public Guid ClientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:Domain")]
        [BasicSetting("Domain", IsRequired = true, Pattern = "(?i)Domain")]
        public string Domain { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:Scopes")]
        [BasicSetting("Scopes", IsRequired = true, Pattern = "(?i)Scopes?")]
        public string Scopes { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:SignUpSignInPolicyId")]
        [BasicSetting("SignUpSignInPolicyId", IsRequired = true, Pattern = "(?i)(?:SignUp)?SignIn(?:Policy(?:Id)?)")]
        public string SignUpSignInPolicyId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static AzureActiveDirectoryB2CSettings Parse(string text)
        {
            return Parse<AzureActiveDirectoryB2CSettings>(text
                .ThrowIfNullOrWhiteSpaceArgument(nameof(text)));
        }
    }
}
