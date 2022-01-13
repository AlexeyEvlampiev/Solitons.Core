
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Solitons.Samples.Domain.Contracts
{
    [Guid("6b4e939f-0c6f-42d1-b6a3-ebaa6d647ca5")]
    public class WeatherForecastResponse : BasicJsonDataTransferObject
    {
        [JsonPropertyName("items")] public Item[] Items { get; set; } = Array.Empty<Item>();

        protected override void OnDeserialization(object sender)
        {
            if (Items is null)
                throw new NullReferenceException();
            base.OnDeserialization(sender);
        }

        public sealed class Item
        {
            [JsonPropertyName("date")]
            public DateTime Date { get; set; }

            [JsonPropertyName("temp")]
            public int TemperatureC { get; set; }

            [JsonPropertyName("summary")]
            public string? Summary { get; set; }
        }
    }
}


