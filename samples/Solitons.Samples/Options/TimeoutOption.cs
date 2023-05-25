using System.CommandLine;
using System.CommandLine.Parsing;

namespace Solitons.Options
{
    public sealed class TimeoutOption : Option<CancellationToken>
    {
        public const int DefaultTimeoutInSeconds = 60 * 60;

        public TimeoutOption() 
            : base("--timeout", Parse, true, "Execution timeout")
        {

        }

        private static CancellationToken Parse(ArgumentResult result)
        {
            if (result.Tokens.Count == 0)
            {
                return new CancellationTokenSource(DefaultTimeoutInSeconds * 1000).Token;
            }

            var token = result.Tokens[0].Value;
            if (int.TryParse(token, out var seconds))
            {
                if (seconds > 0)
                {
                    return new CancellationTokenSource(seconds * 1000).Token;
                }

                result.ErrorMessage = $"The specified timeout value should be expressed as a positive integer representing a valid number of seconds.";
                return CancellationToken.None;
            }

            if (TimeSpan.TryParse(token, out var timeout))
            {
                if (timeout > TimeSpan.Zero)
                {
                    return new CancellationTokenSource(timeout).Token;
                }

                result.ErrorMessage = $"The timeout value specified must be a positive duration.";
                return CancellationToken.None;
            }

            result.ErrorMessage = $"The specified timeout value must be expressed as a positive duration measured in seconds or a valid timespan expression.";
            return CancellationToken.None;
        }
    }
}
