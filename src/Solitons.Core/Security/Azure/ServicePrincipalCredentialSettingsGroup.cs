using System;
using Solitons.Configuration;

namespace Solitons.Security.Azure
{
    /// <summary>
    /// Service principal credentials
    /// </summary>
    /// <remarks>
    /// client-id={value};client-secret={value};tenant-id={value};object-id={value}
    /// </remarks>
    public sealed class ServicePrincipalCredentialSettingsGroup : SettingsGroup
    {
        /// <summary>
        /// The client ID of the application the service principal is associated with
        /// </summary>
        [Setting("client-id", IsRequired = true, Pattern = @"(?is)(?:client([-\s]*id)?|cid)")]
        public Guid ClientId { get; set; }

        /// <summary>
        /// The secret for the client ID
        /// </summary>
        [Setting("client-secret", IsRequired = true, Pattern = @"(?is)(client[-\s]*)?secret")]
        public string ClientSecret { get; set; } = string.Empty;

        /// <summary>
        /// The tenant ID or domain the application is in
        /// </summary>
        [Setting("tenant-id", IsRequired = true, Pattern = @"(?is)(tenant([-\s]*id)?|tid)")]
        public Guid TenantId { get; set; }

        /// <summary>
        ///  The AAD object id of the client
        /// </summary>
        [Setting("object-id", IsRequired = false, Pattern = @"(?is)(obj(ect)?([-\s]*id)?|oid)")]
        public Guid? ObjectId { get; set; }

        /// <summary>
        /// Default Azure subscription id
        /// </summary>
        [Setting("default-subscription-id", IsRequired = false, Pattern = @"(?is)(?:def(?:ault)?-?)?(?:(?:subscription|subs?|s)-?)(?:id)?")]
        public Guid? DefaultSubscriptionId { get; set; }

        /// <summary>
        /// Default Azure resource group
        /// </summary>
        [Setting("default-resource-group-name", IsRequired = false, Pattern = @"(?is)(?:def(?:ault)?-?)?(?:(?:resource|res|r)-?)(?:(?:group|grp|g)-?)(?:name)?")]
        public string? DefaultResourceGroupName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ServicePrincipalCredentialSettingsGroup Parse(string text) =>
            Parse<ServicePrincipalCredentialSettingsGroup>(text);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSynopsis() => 
            GetSynopsis<ServicePrincipalCredentialSettingsGroup>();

    }
}
