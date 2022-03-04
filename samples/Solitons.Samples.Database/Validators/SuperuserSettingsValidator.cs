using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Validation;
using Solitons.Samples.Database.Models;

namespace Solitons.Samples.Database.Validators
{
    public sealed class SuperuserSettingsValidator : IOptionValidator
    {
        public ValidationResult GetValidationResult(CommandOption option, ValidationContext context)
        {
            if (option.HasValue())
            {
                try
                {
                    var settings = SuperuserSettingsGroup.Parse(option.Value());
                    return ValidationResult.Success;
                }
                catch (Exception e)
                {
                    return new ValidationResult($"Invalid superuser settings string. {e.Message}");
                }
            }
            return ValidationResult.Success;
        }
    }
}
