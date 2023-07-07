using System.Diagnostics;
using System.Net;
using System.Reactive.Linq;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Polly;
using Solitons;
using Solitons.Management.Azure;
using Solitons.Security;
using Solitons.Security.Common;

namespace SampleSoft.SkyNet.Azure.Security;

public sealed class KeyVaultSecretsRepository : SecretsRepository, ISecretsRepository
{
    private readonly SecretClient _nativeClient;

    private KeyVaultSecretsRepository(SecretClient nativeClient)
    {
        _nativeClient = nativeClient ?? throw new ArgumentNullException(nameof(nativeClient));
    }


    [DebuggerNonUserCode]
    public static ISecretsRepository Create(SecretClient nativeClient) => new KeyVaultSecretsRepository(nativeClient);

    private KeyVaultSecretsRepository(Uri vaultUri, TokenCredential credential)
    {
        if (vaultUri == null) throw new ArgumentNullException(nameof(vaultUri));
        if (credential == null) throw new ArgumentNullException(nameof(credential));
        _nativeClient = new SecretClient(vaultUri, credential);
    }

    private KeyVaultSecretsRepository(string keyVaultUrl, string tenantId, string clientId, string clientSecret)
    {
        if (false == Uri.IsWellFormedUriString(keyVaultUrl, UriKind.Absolute))
            throw new ArgumentException($"'{keyVaultUrl}' is not a valid uri.");
        _nativeClient = new SecretClient(vaultUri:
            new Uri(keyVaultUrl), credential: new ClientSecretCredential(tenantId, clientId, clientSecret));
    }

    [DebuggerStepThrough]
    public static ISecretsRepository Create(
        Uri uri,
        string tenantId,
        string clientId,
        string secret)
    {
        return new KeyVaultSecretsRepository(
            uri.ToString(), 
            tenantId, 
            clientId,
            secret);
    }

    [DebuggerStepThrough]
    public static ISecretsRepository Create(
        Uri uri,
        IAzureClientSecretCredentials credentials)
    {
        return new KeyVaultSecretsRepository(
            uri.ToString(),
            credentials.TenantId,
            credentials.ClientId,
            credentials.ClientSecret);
    }

    protected override Task<string[]> ListSecretNamesAsync(CancellationToken cancellation)
    {
        return AsyncFunc
            .Wrap(async () =>
            {
                var names = new List<string>();
                var properties = _nativeClient
                    .GetPropertiesOfSecretsAsync(cancellation);
                await foreach (var property in properties)
                {
                    names.Add(property.Name);
                }
                return names.ToArray();
            })
            .WithRetryOnError(exceptions => exceptions
                .OfType<RequestFailedException>()
                .Take(3)
                .SelectMany((ex, attempt) => Observable
                    .Timer(++attempt * TimeSpan.FromMilliseconds(100))
                    .Select(_ => ex))
                .Where(ex => ex.Status != (int)HttpStatusCode.Unauthorized)
            )
            .Invoke();
    }

    protected override async Task<string> GetSecretAsync(string secretName)
    {
        var secret = await _nativeClient.GetSecretAsync(secretName);
        return secret.Value.Value;
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

    public override string ToString() => _nativeClient.VaultUri.ToString();
}