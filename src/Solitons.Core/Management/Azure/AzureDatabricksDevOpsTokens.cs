using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Management.Azure;

/// <summary>
/// Represents a sealed record for Azure Databricks DevOps tokens.
/// </summary>
public sealed record AzureDatabricksDevOpsTokens
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AzureDatabricksDevOpsTokens"/> class.
    /// </summary>
    /// <param name="databricksToken">The Databricks token.</param>
    /// <param name="managementToken">The management token.</param>
    private AzureDatabricksDevOpsTokens(string? databricksToken, string? managementToken)
    {
        DatabricksToken = ThrowIf.ArgumentNullOrWhiteSpace(databricksToken);
        ManagementToken = ThrowIf.ArgumentNullOrWhiteSpace(managementToken);
    }

    /// <summary>
    /// Gets the Databricks management token.
    /// </summary>
    public string DatabricksToken { get; }

    /// <summary>
    /// Gets the Azure Management Resource token.
    /// </summary>
    public string ManagementToken { get; }

    /// <summary>
    /// Asynchronously gets a new instance of the <see cref="AzureDatabricksDevOpsTokens"/> class.
    /// </summary>
    /// <param name="tenantId">The tenant ID.</param>
    /// <param name="clientId">The client ID.</param>
    /// <param name="secret">The secret.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a new instance of the <see cref="AzureDatabricksDevOpsTokens"/> class.</returns>
    public static async Task<AzureDatabricksDevOpsTokens> GetAsync(
        string tenantId,
        string clientId,
        string secret,
        CancellationToken cancellation = default)
    {
        var provider = new AADClient(tenantId, clientId, secret);
        var acquireDatabricksTokenTask = provider.GetDatabricksTokenAsync(cancellation);
        var acquireManagementTokenTask = provider.GetManagementTokenAsync(cancellation);
        await Task.WhenAll(acquireDatabricksTokenTask, acquireManagementTokenTask);
        return new AzureDatabricksDevOpsTokens(
            await acquireDatabricksTokenTask,
            await acquireManagementTokenTask);
    }

    /// <summary>
    /// Asynchronously gets a new instance of the <see cref="AzureDatabricksDevOpsTokens"/> class using the specified Azure Active Directory credentials.
    /// </summary>
    /// <param name="credentials">The Azure Active Directory credentials to use for authentication.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a new instance of the <see cref="AzureDatabricksDevOpsTokens"/> class.</returns>
    [DebuggerStepThrough]
    public static Task<AzureDatabricksDevOpsTokens> GetAsync(
        IAzureClientSecretCredentials credentials,
        CancellationToken cancellation = default) =>
        GetAsync(credentials.TenantId, credentials.ClientId, credentials.ClientSecret, cancellation);

    // ReSharper disable once InconsistentNaming
    sealed class AADClient
    {
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _secret;
        private readonly HttpClient _httpClient = new HttpClient();

        public AADClient(string tenantId, string clientId, string secret)
        {
            _tenantId = tenantId;
            _clientId = clientId;
            _secret = secret;
        }

        public Task<string?> GetDatabricksTokenAsync(CancellationToken cancellation = default)
        {
            return Observable
                .Return(new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/token"),
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                    { "client_id", _clientId },
                    { "grant_type", "client_credentials" },
                    { "scope", "2ff814a6-3304-4ab8-85cb-cd0e6f879c1d/.default" },
                    { "client_secret", _secret },
                    })
                })
                .SelectMany(request => _httpClient.SendAsync(request, cancellation))
                .Do(response => response.EnsureSuccessStatusCode())
                .RetryWhen(exceptions => exceptions
                    .SelectMany((ex, attempt) =>
                    {
                        if (ex is HttpRequestException httpError &&
                            attempt < 10)
                        {
                            var statusCode = (int)httpError
                                .StatusCode
                                .GetValueOrDefault(HttpStatusCode.InternalServerError);
                            if (statusCode >= 500)
                            {
                                return Observable.Return(attempt);
                            }
                        }
                        return Observable.Throw<int>(ex);
                    })
                    .Delay(TimeSpan.FromSeconds(2)))
                .SelectMany(response => response.Content.ReadAsStringAsync(cancellation))
                .Select(json => JsonNode.Parse(json))
                .Select(json => json?["access_token"]?.ToString())
                .FirstOrDefaultAsync()
                .ToTask(cancellation);
        }

        public Task<string?> GetManagementTokenAsync(CancellationToken cancellation = default)
        {
            return Observable
                .Return(new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://login.microsoftonline.com/{_tenantId}/oauth2/token"),
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                    { "client_id", _clientId },
                    { "grant_type", "client_credentials" },
                    { "resource", "https://management.core.windows.net/" },
                    { "client_secret", _secret },
                    })
                })
                .SelectMany(request => _httpClient.SendAsync(request))
                .Do(response => response.EnsureSuccessStatusCode())
                .RetryWhen(exceptions => exceptions
                    .SelectMany((ex, attempt) =>
                    {
                        if (ex is HttpRequestException httpError &&
                            attempt < 10)
                        {
                            var statusCode = (int)httpError
                                .StatusCode
                                .GetValueOrDefault(HttpStatusCode.InternalServerError);
                            if (statusCode >= 500)
                            {
                                return Observable.Return(attempt);
                            }
                        }
                        return Observable.Throw<int>(ex);
                    })
                    .Delay(TimeSpan.FromSeconds(2)))
                .SelectMany(response => response.Content.ReadAsStringAsync(cancellation))
                .Select(json => JsonNode.Parse(json))
                .Select(json => json?["access_token"]?.ToString())
                .FirstOrDefaultAsync()
                .ToTask(cancellation);
        }
    }
}