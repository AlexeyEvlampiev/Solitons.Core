using System.Runtime.InteropServices;
using Solitons.Data;

namespace Solitons.Samples.Domain.Contracts
{
    [Guid("22c2fd1a-01c3-484a-bcf2-15ddf7b25fbe")]
    public sealed class WeatherForecastCommand : DatabaseRpcCommand<WeatherForecastRequest, WeatherForecastResponse>
    {
        public WeatherForecastCommand(IDatabaseRpcProvider client, IDataContractSerializer serializer) 
            : base("weather_forecast_get", client, serializer)
        {
            OperationTimeout = TimeSpan.FromSeconds(2);
        }
    }
}
