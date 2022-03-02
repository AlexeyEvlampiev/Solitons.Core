using System.Diagnostics;
using Solitons.Collections;
using Solitons.Configuration;

namespace Solitons.Samples.Azure
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AzureActiveDirectoryB2CSettings : ConfigMap
    {
        public const string ConfigurationSectionName = "AzureAdB2C";

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:Instance")]
        [ConfigMap("Instance", IsRequired = true, Pattern = "(?i)Instance")]
        public string Instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:ClientId")]
        [ConfigMap("ClientId", IsRequired = true, Pattern = "(?i)(?:Client|App(?:lication)?)Id")]
        public Guid ClientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:Domain")]
        [ConfigMap("Domain", IsRequired = true, Pattern = "(?i)Domain")]
        public string Domain { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:Scopes")]
        [ConfigMap("Scopes", IsRequired = true, Pattern = "(?i)Scopes?")]
        public string Scopes { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DictionaryKey("AzureAdB2C:SignUpSignInPolicyId")]
        [ConfigMap("SignUpSignInPolicyId", IsRequired = true, Pattern = "(?i)(?:SignUp)?SignIn(?:Policy(?:Id)?)")]
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
