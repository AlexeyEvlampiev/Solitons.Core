using Solitons.Configuration;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ContainerRegistryCredentialSettingsGroup : SettingsGroup
    {
        /// <summary>
        /// 
        /// </summary>
        [Setting("host", IsRequired = true, Pattern = "(?i)(host|server|address|ur[li]|endpoint)")]
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [Setting("user", IsRequired = true, Pattern = "(?i)(user(-?name)?|login|u(sr)?)")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [Setting("password", IsRequired = true, Pattern = "(?i)(pass(word)?|secret|p(wd)?)")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ContainerRegistryCredentialSettingsGroup Parse(string text) =>
            Parse<ContainerRegistryCredentialSettingsGroup>(text);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSynopsis() => GetSynopsis<ContainerRegistryCredentialSettingsGroup>();
    }
}
