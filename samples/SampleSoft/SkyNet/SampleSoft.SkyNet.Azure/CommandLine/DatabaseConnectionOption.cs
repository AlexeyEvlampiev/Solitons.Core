using System.CommandLine;
using System.CommandLine.Parsing;
using Npgsql;

namespace SampleSoft.SkyNet.Azure.CommandLine;

public sealed class DatabaseConnectionOption : Option<string>
{
    public DatabaseConnectionOption()
        : base("--connection", Parse, false, "SkyNet DB connection string.")
    {
        IsRequired = true;
    }

    private static string Parse(ArgumentResult result)
    {
        if (result.Tokens.Count == 0)
        {
            result.ErrorMessage = $"A valid postgres connection string is required";
            return "";
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
            result.ErrorMessage = $"Invalid database connection string. {e.Message}";
            return "";
        }

    }
}