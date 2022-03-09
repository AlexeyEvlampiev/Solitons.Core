

using Solitons.Data;
using Solitons.Samples.Domain.Contracts;

namespace Solitons.Samples.Domain
{
    public sealed class SampleDataContractSerializer : DataContractSerializer
    {
        public SampleDataContractSerializer() : base(DataContractSerializerBehaviour.Default)
        {
            Register(typeof(ImageGetRequest), IMediaTypeSerializer.BasicJsonSerializer);
            Register(typeof(ImageGetRequest), IMediaTypeSerializer.BasicXmlSerializer);

            Register(typeof(WeatherForecastRequest), IMediaTypeSerializer.BasicJsonSerializer);
            Register(typeof(WeatherForecastRequest), IMediaTypeSerializer.BasicXmlSerializer);

            Register(typeof(WeatherForecastResponse), IMediaTypeSerializer.BasicJsonSerializer);
            Register(typeof(WeatherForecastResponse), IMediaTypeSerializer.BasicXmlSerializer);
        }
    }
}
