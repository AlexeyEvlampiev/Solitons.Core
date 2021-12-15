using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Validation;
using Npgsql;
using Polly;

namespace Solitons.Samples.Database.Validators
{
    sealed class PgConnectionStringOptionValidator : IOptionValidator
    {
        public ValidationResult GetValidationResult(CommandOption option, ValidationContext context)
        {
            if (false == option.HasValue())
                return ValidationResult.Success;

            var policy = Policy
                .Handle<NpgsqlException>(ex => ex.IsTransient)
                .WaitAndRetry(5, attempt => TimeSpan.FromMilliseconds(200));
            try
            {
                using var connection = new NpgsqlConnection(option.Value());
                policy.Execute(connection.Open);
            }
            catch (NpgsqlException e)
            {
                return new ValidationResult(e.Message);
            }
            catch (Exception e)
            {
                return new ValidationResult($"Invalid postgres connection string. Error: {e.Message}");
            }
            return ValidationResult.Success;
        }
    }
}
