using System.Net;
using System.Reactive.Linq;
using Newtonsoft.Json;
using Npgsql;
using SampleSoft.SkyNet.Azure;
using SampleSoft.SkyNet.Azure.Postgres;
using Solitons;
using Solitons.Data;
using Xunit;

namespace SampleSoft.SkyNet.Control.SkyNetDb.Test.Images;
using static KeyVaultSecretNames;

public sealed class ImageGetTest : SkyNetDbTest
{
    [SkyNetConnectionStringSecret(SkyNetDbAdminConnectionString)]
    public async Task TestAsync(
        NpgsqlConnection connection,
        HttpClient client,
        CancellationToken cancellation)
    {
        
        await connection.ExecuteNonQueryAsync(@"
            INSERT INTO data.email(account_object_id, ""id"") VALUES 
            ('5840573e-9786-4cae-bd2e-201976dc1555', 'l.v.beethoven@skynet.com')
            ON CONFLICT(""id"") DO NOTHING;

            INSERT INTO data.image(object_id) VALUES('fff2022a-1a2e-4974-8b51-2beb215123c0')
            ON CONFLICT DO NOTHING;
            ", cancellation);

        client.DefaultRequestHeaders.Add("SKYNET-IDENTITY", "l.v.beethoven@skynet.com");


        var response = await client.GetAsync("/images/fff2022a-1a2e-4974-8b51-2beb215123c0?v=hello", cancellation);
        response.EnsureSuccessStatusCode();
        

        var payload = await response.Content
            .ReadAsString(cancellation)
            .Select(json => JsonConvert.DeserializeObject<dynamic>(json)!);

        Assert.Equal("fff2022a-1a2e-4974-8b51-2beb215123c0", (string)payload.oid.ToString());



        response = await client.GetAsync($"/images/{Guid.NewGuid()}?v=hello", cancellation);
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        payload = await response.Content
            .ReadAsString(cancellation)
            .Select(json => JsonConvert.DeserializeObject<dynamic>(json)!);
    }

}