using System.CommandLine.Parsing;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.RegularExpressions;
using Npgsql;

namespace Solitons.Examples.WebApp.Data;

sealed class Validator
{
    private readonly Regex _unresolvedEnvVariableRegex = new(@"^%([\w-_]+)%$");


    public void ValidateConnectionString(OptionResult result)
    {
        var connectionString = result.GetValueOrDefault<string>()!;
        if (IsUnresolvedEnvVariable(connectionString, out var variableName))
        {
            result.ErrorMessage = $"'{variableName}' environment variable not found.";
        }

        try
        {
            Observable
                .Using(
                    () => new NpgsqlConnection(connectionString),
                    connection => connection
                        .OpenAsync()
                        .ToObservable()
                        .RetryWhen(exceptions => exceptions
                            .OfType<NpgsqlException>()
                            .Take(5)
                            .SelectMany((_, attempt) => Task
                                .Delay(100 * attempt)
                                .ToObservable()))
                )
                .GetAwaiter()
                .GetResult();
        }
        catch (ArgumentException e)
        {
            result.ErrorMessage = $"Invalid postgres connection string. {e.Message}";
        }
        catch (NpgsqlException e)
        {
            result.ErrorMessage = $"Connection test failed. {e.Message}";
        }
    }

    private bool IsUnresolvedEnvVariable(string input, out string variableName)
    {
        var match = _unresolvedEnvVariableRegex.Match(input);
        if (match.Success)
        {
            variableName = match.Groups[1].Value;
            return true;
        }

        variableName = string.Empty;
        return false;
    }
}