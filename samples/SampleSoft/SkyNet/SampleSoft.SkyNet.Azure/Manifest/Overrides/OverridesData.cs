using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;
using SampleSoft.SkyNet.Azure.SQLite;
using Solitons;
using Solitons.Data;
using Solitons.Security;

namespace SampleSoft.SkyNet.Azure.Manifest.Overrides;

public sealed class OverridesData : BasicJsonDataTransferObject
{
    [JsonPropertyName("secrets")]
    public string? SecretsConnectionString { get; set; }

    [JsonPropertyName("pgServer")]
    public string? MaintenanceDbConnectionString { get; set; }

    [JsonPropertyName("sharedPassword")]
    public string? SharedPassword { get; set; }

    internal ManifestData? Manifest { get; set; }

    public ISecretsRepository? GetSecretsRepository()
    {
        if (SecretsConnectionString.IsNullOrWhiteSpace())
        {
            return null;
        }

        if (SQLiteSecretsRepository.IsScopeConnectionString(SecretsConnectionString!))
        {
            return SQLiteSecretsRepository.Create(SecretsConnectionString!);
        }

        throw new NotImplementedException();

    }

    internal void Assert(Action<string> onError)
    {
        if (MaintenanceDbConnectionString.IsPrintable())
        {
            try
            {
                using (new NpgsqlConnection(MaintenanceDbConnectionString))
                {

                }
            }
            catch (Exception e)
            {
                onError.Invoke(e.Message);
                MaintenanceDbConnectionString = null;
            }
        }

        if (SecretsConnectionString.IsPrintable())
        {
            try
            {
                var secrets = GetSecretsRepository();
                if (secrets != null &&
                    MaintenanceDbConnectionString.IsPrintable())
                {
                    secrets.SetSecretAsync(
                        KeyVaultSecretNames.SkyNetPgServerConnectionString,
                        MaintenanceDbConnectionString!)
                        .GetAwaiter()
                        .GetResult();
                }
            }
            catch (Exception e)
            {
                onError.Invoke(e.Message);
            }
        }
    }
}