namespace Solitons.Samples.Domain
{
	using System;
	using System.ComponentModel;
	using System.Data;
	using System.Threading.Tasks;
	using Solitons.Data;

	using Solitons.Samples.Domain.Contracts;

	/// <summary>
	/// 
	/// </summary>
	public interface ISampleDbApi
	{ 
		
		/// <summary>
		/// weather_forecast_get
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellation"></param>
		/// <returns></returns>
		[Description("weather_forecast_get")]
		Task<WeatherForecastResponse> InvokeAsync(WeatherForecastRequest request, Func<WeatherForecastResponse, Task> completionCallback = null, CancellationToken cancellation = default);
	 
	}

	/// <summary>
	/// 
	/// </summary>
	public sealed class SampleDbApi : ISampleDbApi
	{
		private readonly ITransactionScriptProvider _provider;
		private readonly IDomainSerializer _serializer;

        public SampleDbApi(ITransactionScriptProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
			_serializer = SampleDomainContext.GetOrCreate().GetSerializer();
        } 
		
		/// <summary>
		/// weather_forecast_get
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellation"></param>
		/// <returns></returns>		
		public async Task<WeatherForecastResponse> InvokeAsync(WeatherForecastRequest request, Func<WeatherForecastResponse, Task> completionCallback, CancellationToken cancellation)
		{
			if (request == null) throw new ArgumentNullException(nameof(request));
			cancellation.ThrowIfCancellationRequested();

			await _provider.OnRequestAsync(request);
			var content = _serializer.Serialize(request, "application/json");	
			content = await _provider.InvokeAsync("weather_forecast_get", content, "application/json", 2, IsolationLevel.ReadCommitted,	cancellation);
			var response = (WeatherForecastResponse)_serializer.Deserialize(typeof(WeatherForecastResponse), "application/json", content);
			await _provider.OnResponseAsync(response);
			if (completionCallback is not null)
            {
                await completionCallback.Invoke(response);
            }
			return response;
		}
	 
	}
}
