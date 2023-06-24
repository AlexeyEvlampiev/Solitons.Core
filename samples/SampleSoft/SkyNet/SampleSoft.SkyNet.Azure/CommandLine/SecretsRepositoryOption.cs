using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text.RegularExpressions;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using SampleSoft.SkyNet.Azure.Security;
using Solitons.Security;

namespace SampleSoft.SkyNet.Azure.CommandLine;

public sealed class SecretsRepositoryOption : Option<ISecretsRepository>
{
    public SecretsRepositoryOption() 
        : base("--secrets", Parse, true, "Secrets repository service identifier.")
    {
    }

    private static ISecretsRepository Parse(ArgumentResult result)
    {
        if (result.Tokens.Count == 0)
        {
            return ISecretsRepository.Environment(EnvironmentVariableTarget.User);
        }

        var token = result.Tokens.Single().Value ??
                    EnvironmentVariableTarget.User.ToString("g");

        if (Enum.TryParse<EnvironmentVariableTarget>(token, true, out var target))
        {
            return ISecretsRepository.Environment(target);
        }

        var match = Regex.Match(token,@"([\w-_]+).vault.azure.net");
        if (match.Success)
        {
            var name = match.Result("$1");
            var url = new Uri($"https://{name}.vault.azure.net/");
            var client = new SecretClient(url, new DefaultAzureCredential());
            return KeyVaultSecretsRepository.Create(client);
        }
           
        throw new NotSupportedException(token);
    }
}