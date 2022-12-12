using System.Diagnostics;
using System.Net;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Polly;
using Solitons.Security;
using Solitons.Security.Azure;
using Solitons.Security.Common;

namespace Solitons.Samples.Azure
{
    public sealed class KeyVaultSecretsRepository : SecretsRepository
    {
        private readonly SecretClient _nativeClient;

        private KeyVaultSecretsRepository(SecretClient nativeClient)
        {
            _nativeClient = nativeClient ?? throw new ArgumentNullException(nameof(nativeClient));
        }


        private KeyVaultSecretsRepository(Uri vaultUri, TokenCredential credential)
        {
            _nativeClient = new SecretClient(
                ThrowIf.NullArgument(vaultUri, nameof(vaultUri)),
                ThrowIf.NullArgument(credential, nameof(credential)));
        }

        private KeyVaultSecretsRepository(string keyVaultUrl, string tenantId, string clientId, string clientSecret)
        {
            if (!Uri.IsWellFormedUriString(keyVaultUrl, UriKind.Absolute))
            {
                throw new ArgumentException($"'{keyVaultUrl}' is not a valid uri.");
            }

            _nativeClient = new SecretClient(
                vaultUri: new Uri(keyVaultUrl), 
                credential: new ClientSecretCredential(tenantId, clientId, clientSecret));
        }

        [DebuggerStepThrough]
        public static ISecretsRepository Create(
            Uri uri,
            ServicePrincipalCredentialSettingsGroup credentials)
        {
            return new KeyVaultSecretsRepository(
                uri.ToString(),
                credentials.TenantId.ToString(),
                credentials.ClientId.ToString(),
                credentials.ClientSecret);
        }

        [DebuggerStepThrough]
        public static ISecretsRepository Create(
            string uri,
            ServicePrincipalCredentialSettingsGroup credentials) => Create(new Uri(uri), credentials);

        public static ISecretsRepository From(SecretClient nativeClient) => new KeyVaultSecretsRepository(nativeClient);


        protected override async Task<string> GetSecretAsync(string secretName)
        {
            var secret = await _nativeClient.GetSecretAsync(secretName);
            return secret.Value.Value;
        }

        protected override async Task<string[]> ListSecretNamesAsync(CancellationToken cancellation)
        {
            var names = new List<string>();
            await Policy
                .Handle<RequestFailedException>(ex => ex.Status != (int)HttpStatusCode.Unauthorized)
                .WaitAndRetryAsync(3, (attempt) => TimeSpan.FromMilliseconds(100 + 100 * attempt))
                .ExecuteAsync(async () =>
                {
                    var properties = _nativeClient
                        .GetPropertiesOfSecretsAsync(cancellation);
                    await foreach (var property in properties)
                    {
                        names.Add(property.Name);
                    }
                });
            return names.ToArray();
        }

        protected override async Task<string?> GetSecretIfExistsAsync(string secretName)
        {
            try
            {
                var secret = await Policy
                    .Handle<RequestFailedException>(ex => ex.Status != (int)HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(3, (attempt) => TimeSpan.FromMilliseconds(100 + 100 * attempt))
                    .ExecuteAsync(() => _nativeClient.GetSecretAsync(secretName));

                return secret.Value.Value;
            }
            catch (Exception ex) when (IsSecretNotFoundError(ex))
            {
                return null;
            }
        }

        protected override async Task<string> GetOrSetSecretAsync(string secretName, string defaultValue)
        {
            try
            {
                var bundle = await _nativeClient.GetSecretAsync(secretName);
                return bundle.Value.Value;
            }
            catch (Exception ex) when (IsSecretNotFoundError(ex))
            {
                var bundle = await _nativeClient.SetSecretAsync(secretName, defaultValue);
                return bundle.Value.Value;
            }
        }

        protected override Task SetSecretAsync(string secretName, string secretValue)
        {
            return _nativeClient.SetSecretAsync(secretName, secretValue);
        }

        protected override bool IsSecretNotFoundError(Exception exception)
        {
            if (exception is RequestFailedException ex)
            {
                return ex.Status == (int)HttpStatusCode.NotFound;
            }

            return false;
        }
    }
}
