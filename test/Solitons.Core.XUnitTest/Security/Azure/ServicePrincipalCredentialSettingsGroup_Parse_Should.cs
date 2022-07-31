using System;
using Xunit;

namespace Solitons.Security.Azure
{
    // ReSharper disable once InconsistentNaming
    public class ServicePrincipalCredentialSettingsGroup_Parse_Should
    {
        [Theory]
        [InlineData("client-id", "client-secret", "tenant-id", "object-id", "default-subscription-id", "default-resource-group-name")]
        [InlineData("client", "secret", "tenant", "object", "subscription-id", "resource-group-name")]
        [InlineData("cid", "secret", "tid", "oid", "subscription", "resource-group")]
        [InlineData("cid", "secret", "tid", "oid", "sub", "rg")]
        public void HandleFullSetOfParameters(
            string clientIdField, 
            string clientSecretField, 
            string tenantIdField,
            string objectIdField,
            string subscriptionIdField, 
            string resourceNameField)
        {
            var clientId = Guid.NewGuid();
            var clientSecret = Guid.NewGuid().ToString();
            var objectId = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            var defaultSubscriptionId = Guid.NewGuid();
            var defaultResourceGroupName = "test-resource-group";
            var input = string.Join(";",new []
            {
                $"{clientIdField}={clientId}",
                $"{clientSecretField}={clientSecret}",
                $"{objectIdField}={objectId}",
                $"{tenantIdField}={tenantId}",
                $"{subscriptionIdField}={defaultSubscriptionId}",
                $"{resourceNameField}={defaultResourceGroupName}"
            });

            var settings = ServicePrincipalCredentialSettingsGroup.Parse(input);
            Assert.Equal(clientId, settings.ClientId);
            Assert.Equal(clientSecret, settings.ClientSecret);
            Assert.Equal(objectId, settings.ObjectId);
            Assert.Equal(tenantId, settings.TenantId);
            Assert.Equal(defaultSubscriptionId, settings.DefaultSubscriptionId);
            Assert.Equal(defaultResourceGroupName, settings.DefaultResourceGroupName);
        }
    }
}
