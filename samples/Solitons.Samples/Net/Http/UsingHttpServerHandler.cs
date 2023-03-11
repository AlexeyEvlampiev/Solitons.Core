using System.Data.SQLite;
using System.Net.Http.Json;

namespace Solitons.Net.Http
{
    public static class UsingHttpServerHandler
    {
        public static async Task RunAsync()
        {
            string databaseFilePath = "web-server.sqlite";
            string connectionString = $"Data Source={databaseFilePath};Version=3;";
            if (File.Exists(databaseFilePath))
            {
                File.Delete(databaseFilePath);
            }

            await SeedDatabaseAsync(connectionString);


            var handler = SqLiteHttpTransactionHandler
                .Create(connectionString)
                .AsHttpMessageHandler();

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("sqlite://api")
            };



            var response = await client.GetAsync("/customers");
            await DisplayAsync(response);

            response = await client.PostAsJsonAsync("/customers", 
                new { name = "Jeff Waters", email = "jeff.waters@test.gov"});
            await DisplayAsync(response);

            response = await client.GetAsync("/customers");
            await DisplayAsync(response);

            async Task DisplayAsync(HttpResponseMessage msg)
            {
                Console.WriteLine(msg);
                Console.WriteLine(await msg.Content.ReadAsStringAsync());
            }
        }

        private static async Task SeedDatabaseAsync(string connectionString)
        {
            await using var connection = new SQLiteConnection(connectionString);
            await connection.OpenAsync();

            await connection.DoAsync(cmd =>
            {
                cmd.CommandText = @"
                CREATE TABLE Customers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    Name TEXT, 
                    Email TEXT)";
                return cmd.ExecuteNonQueryAsync();
            });


            await connection.DoAsync(cmd =>
            {
                cmd.CommandText = @"INSERT INTO Customers (Name, Email) VALUES 
                    ('John Smith', 'john.smith@email.com')
                    ,('Jane Doe', 'jane.doe@email.com')
                    ,('Bob Johnson', 'bob.johnson@email.com')";
                return cmd.ExecuteNonQueryAsync();
            });
        }
    }
}
