namespace Solitons.Examples.WebApp.Data.Model;

public sealed record PgServerInfo()
{
    public string? Host { get; init; }
    public string? Database { get; init; }
    public int Port { get; init; }
    public string? Username { get; init; }
}