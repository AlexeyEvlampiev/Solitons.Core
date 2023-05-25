using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text.RegularExpressions;

namespace Solitons.Options;

public sealed class TestSelectorOption : Option<Regex>
{
    public TestSelectorOption()
        : base("--selector", Parse, true, "Test selection regex.")
    {
        AddAlias("--select");
        AddAlias("--filter");
    }

    private static Regex Parse(ArgumentResult result)
    {
        if (result.Tokens.Count == 0)
        {
            return new Regex(".*");
        }

        var token = result.Tokens.Single().Value;
        try
        {
            return new Regex(token, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        catch (Exception e)
        {
            result.ErrorMessage = $"Invalid database connection string. {e.Message}";
            return new Regex(".*");
        }
    }
}