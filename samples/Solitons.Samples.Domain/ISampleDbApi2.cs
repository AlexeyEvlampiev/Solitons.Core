using Solitons.Data;
using Solitons.Samples.Domain.Contracts;

namespace Solitons.Samples.Domain
{
    public interface ISampleDbApi
    {
        /// <summary>
        /// weather_forecast_get
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DbCommand("22c2fd1a-01c3-484a-bcf2-15ddf7b25fbe", "weather_forecast_get")]
        Task<WeatherForecastResponse> InvokeAsync(WeatherForecastRequest request, CancellationToken cancellation = default);


        /// <summary>
        /// image_get
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DbCommand("a7cccf10-406f-4707-967f-7f2112766ba4", "image_get")]
        Task<ImageGetResponse> InvokeAsync(ImageGetRequest request, CancellationToken cancellation = default);
	}
}
