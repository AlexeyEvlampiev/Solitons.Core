using System.CommandLine;
using System.CommandLine.Parsing;
using Solitons.Security;

namespace Solitons.Examples.WebApp.Data.Options
{
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

            throw new NotImplementedException("TODO: implement Azure Key Vault secrets");
        }
    }
}
