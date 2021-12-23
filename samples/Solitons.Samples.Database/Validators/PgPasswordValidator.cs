using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Validation;
using Solitons.Security.Postgres;

namespace Solitons.Samples.Database.Validators
{
    public sealed class PgPasswordValidator : IOptionValidator
    {
        public ValidationResult GetValidationResult(CommandOption option, ValidationContext context)
        {
            if (option.HasValue())
            {
                if(PgSecurityManagementProvider.IsValidPassword(option.Value()))
                    return ValidationResult.Success;
                return new ValidationResult($"The postgres password is not valid.");
            }

            return ValidationResult.Success;
        }
    }
}
