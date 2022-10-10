using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Solitons.Configuration;

namespace Solitons.Security.Azure
{
    /// <summary>
    /// Azure Batch Account settings group
    /// </summary>
    /// <remarks>
    /// account={value};location={value};key={value};principal={principal}
    /// </remarks>
    public sealed class AzureBatchAccountSettingsGroup : SettingsGroup
    {
        private AzureBatchAccountSettingsGroup()
        {
            Account = String.Empty;
            Location = String.Empty;
            Key = String.Empty;
        }

        /// <summary>
        /// Account name
        /// </summary>
        [Setting("account", IsRequired = true, Pattern = @"(?i)(?:account|acc?|name|instance|service)")]
        public string Account { get; internal set; }

        /// <summary>
        /// Azure region
        /// </summary>
        [Setting("location", IsRequired = true, Pattern = @"(?i)(?:az(?:ure)?-?)?(?:location|loc|region|reg)")]
        public string Location { get; internal set; }

        /// <summary>
        /// Access key
        /// </summary>
        [Setting("key", IsRequired = true, Pattern = @"(?i)(?:key|secret)")]
        public string Key { get; internal set; }

        /// <summary>
        /// Get a unique identifier assigned to this Batch Account resource, when it’s registered with Azure Active Directory.
        /// </summary>
        [Setting("principal", IsRequired = false, Pattern = @"(?i)(?:(?:principal|object)(?:-?id)?)|oid|pid")]
        public Guid? PrincipalId { get; internal set; }

        /// <summary>
        /// Account endpoint
        /// </summary>
        public string BatchUrl => $"https://{Account}.{Location}.batch.azure.com";

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="FormatException"></exception>
        protected override void OnDeserialized()
        {
            var errors = new List<string>();
            if (Regex.IsMatch(Account, @"^[a-z\d]{3,24}$") == false)
            {
                errors.Add($"Invalid {nameof(Account)} value. Expected: 3-to-24 lowercase letters and numbers.");
            }

            if (Regex.IsMatch(Location, @"^[a-z]{4,50}$") == false)
            {
                errors.Add($"Invalid {nameof(Location)} value. Expected: 4-to-50 lowercase letters.");
            }

            if (Key.IsBase64String() == false)
            {
                errors.Add($"Invalid {nameof(Key)} value. Expected: base64 string.");
            }

            if (errors.Any())
            {
                throw new FormatException(errors.Join("; "));
            }

            base.OnDeserialized();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static AzureBatchAccountSettingsGroup? Parse(string? input) =>
            input == null ? null : Parse<AzureBatchAccountSettingsGroup>(input);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSynopsis() => GetSynopsis<AzureBatchAccountSettingsGroup>();
    }
}
