using System.CommandLine;
using System.CommandLine.Parsing;
using Npgsql;

namespace Solitons.Options;

sealed class ServerConnectionOption : Option<string?>
{
    public ServerConnectionOption()
        : base("--server", Parse, true, "Secrets repository service identifier.")
    {
    }

    private static string? Parse(ArgumentResult result)
    {
        if (result.Tokens.Count == 0)
        {
            return null;
        }

        var token = result.Tokens.Single().Value;

        try
        {
            var builder = new NpgsqlConnectionStringBuilder(token);
            using var connection = new NpgsqlConnection(builder.ConnectionString);
            connection.Open();
            return token;
        }
        catch (Exception e)
        {
            result.ErrorMessage = $"Invalid server connection string. {e.Message}";
            return null;
        }

    }
}