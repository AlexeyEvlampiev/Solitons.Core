namespace Solitons.Management.Azure;

/// <summary>
/// Provides the credentials required to authenticate with an Azure service using a client secret or certificate.
/// </summary>
public interface IAzureClientSecretCredentials
{
    /// <summary>
    /// Gets the Azure Active Directory tenant ID associated with the service.
    /// </summary>
    string TenantId { get; }

    /// <summary>
    /// Gets the client ID associated with the service.
    /// </summary>
    string ClientId { get; }

    /// <summary>
    /// Gets the client secret used to authenticate with the service.
    /// </summary>
    /// <remarks>
    /// If using a client secret, this property should contain the secret key. If using a certificate, this property should
    /// contain the certificate in either PFX or PEM format.
    /// </remarks>
    string ClientSecret { get; }
}