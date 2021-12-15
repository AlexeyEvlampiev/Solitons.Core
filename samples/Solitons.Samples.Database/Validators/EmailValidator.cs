

using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Solitons.Samples.Database.Validators
{
    public sealed class EmailValidator : IOptionValidator
    {
        private readonly Regex _regex = new Regex(@"^[a-zA-Z0-9.!#$%&''*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
        public ValidationResult? GetValidationResult(CommandOption option, ValidationContext context)
        {
            if (option.HasValue())
            {
                var invalidValues = option.Values.Skip(_regex.IsMatch).ToList();
                return invalidValues.Count == 0 
                    ? ValidationResult.Success 
                    : new ValidationResult($"Invalid email address(es): {invalidValues.Join()}");
            }
            return ValidationResult.Success;
        }
    }
}
