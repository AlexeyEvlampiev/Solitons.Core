using Solitons.Security;

namespace SampleSoft.SkyNet.Azure.Manifest;

public interface IManifest
{
    ISecretsRepository GetSecretsRepository();
    bool HasMaintenanceDbConnectionString(out string connectionString);
    string? SharedPassword { get; }
}