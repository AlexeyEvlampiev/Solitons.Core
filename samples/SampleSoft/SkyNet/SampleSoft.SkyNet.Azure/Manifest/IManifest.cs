using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solitons.Security;

namespace SampleSoft.SkyNet.Azure.Manifest;

public interface IManifest
{
    ISecretsRepository GetSecretsRepository();
    bool HasMaintenanceDbConnectionString(out string connectionString);
    string? SharedPassword { get; }
}