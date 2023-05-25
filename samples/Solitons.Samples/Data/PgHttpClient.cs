namespace Solitons.Data;

public sealed class PgHttpClient : HttpClient
{

    public PgHttpClient(string connectionString) 
        : base(new PgHttpMessageHandler(connectionString))
    {
        BaseAddress = new Uri("pg://api");
    }
}