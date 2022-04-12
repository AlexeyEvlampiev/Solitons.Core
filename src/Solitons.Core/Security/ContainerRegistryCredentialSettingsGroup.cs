using System;
using System.Text.RegularExpressions;
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
        [Setting("server", IsRequired = true, Pattern = "(?i)(host|server|address|ur[li]|endpoint)")]
        public string Server { get; private set; } = "docker.io";

        /// <summary>
        /// 
        /// </summary>
        [Setting("user", IsRequired = true, Pattern = "(?i)(user(-?name)?|login|u(sr)?)")]
        public string Username { get; private set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [Setting("password", IsRequired = true, Pattern = "(?i)(pass(word)?|secret|p(wd)?)")]
        public string Password { get; private set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Host => Regex.Match(Server, @"^\w+").Value;

        /// <summary>
        /// 
        /// </summary>
        public Uri Uri => new Uri($"https://{Server}");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ContainerRegistryCredentialSettingsGroup Parse(string input) =>
            Parse<ContainerRegistryCredentialSettingsGroup>(input);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSynopsis() => GetSynopsis<ContainerRegistryCredentialSettingsGroup>();

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="FormatException"></exception>
        protected override void OnDeserialized()
        {
            base.OnDeserialized();
            if (Server.IsNullOrWhiteSpace())
                throw new InvalidOperationException($"{GetType()}.{nameof(Server)} is required.");
            Server = Server.Trim();
            if (Uri.IsWellFormedUriString(Server, UriKind.Absolute))
            {
                Server = Regex.Replace(Server, @"(?i)^https?://", string.Empty);
            }
            else if (false == Uri.IsWellFormedUriString($"https://{Server}", UriKind.Absolute))
            {
                throw new FormatException($"{GetType()}.{nameof(Server)} must be an absolute url.");
            }
        }


    }
}
