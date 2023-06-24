using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using SampleSoft.SkyNet.Azure.Manifest.Overrides;
using Solitons;
using Solitons.Data;
using Solitons.Security;

namespace SampleSoft.SkyNet.Azure.Manifest;

public sealed class ManifestData : BasicJsonDataTransferObject, IManifest
{
    private OverridesData? _overrides;

    [DebuggerStepThrough]
    public void Assert(Action<string>? onError)
    {
        onError ??= DefaultErrorHandler;
        _overrides?.Assert(onError);
    }

    [JsonPropertyName("overrides")]
    public OverridesData Overrides
    {
        get => _overrides ??= new OverridesData(){ Manifest = this };
        set
        {
            _overrides = value;
            if (_overrides != null)
            {
                _overrides.Manifest = this;
            }
        }
    }

    private void DefaultErrorHandler(string message)
    {
        throw new ValidationException(message);
    }


    public ISecretsRepository GetSecretsRepository()
    {
        return Overrides.GetSecretsRepository() ??
               throw new NotImplementedException();
    }

    public bool HasMaintenanceDbConnectionString(out string connectionString)
    {
        if (Overrides.MaintenanceDbConnectionString.IsPrintable())
        {
            connectionString = Overrides.MaintenanceDbConnectionString!;
            return true;
        }
        connectionString = string.Empty;
        return false;
    }

    public string? SharedPassword => Overrides.SharedPassword;
}