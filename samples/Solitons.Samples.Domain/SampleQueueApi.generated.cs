namespace Solitons.Samples.Domain
{
	using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
	using Solitons.Queues;
    using Solitons.Samples.Domain.Contracts;
	/*

	public interface ISampleQueueProducer
	{ 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dto"></param>
		/// <param name="config"></param>
		/// <param name="cancellation"></param>
		/// <returns></returns>
		Task SendAsync(WeatherForecastRequest dto, Action<QueueMessageOptions> config = null, CancellationToken cancellation = default);  
	}

	public sealed class SampleQueue : ISampleQueueProducer
	{
		private readonly ITransientStorage _transientStorage;
		private readonly IDomainContractSerializer _serializer;

		public SampleQueue(ITransientStorage transientStorage)
		{
			_transientStorage = transientStorage ?? throw new ArgumentNullException(nameof(transientStorage));
			_serializer = SampleDomainContext.GetOrCreate().GetSerializer();
		}
		
		private async Task SendAsync(object dto, QueueMessageOptions options, CancellationToken cancellation)
        {
			var content = options.ContentType.IsNullOrWhiteSpace()
                ? _serializer.Serialize(dto, out _)
                : _serializer.Serialize(dto, options.ContentType);
			var bytes = content.ToUtf8Bytes();
			var thing = await _transientStorage.UploadAsync();
            throw new NotImplementedException();
		} 

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dto"></param>
		/// <param name="config"></param>
		/// <param name="cancellation"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public Task SendAsync(WeatherForecastRequest dto, Action<QueueMessageOptions> config, CancellationToken cancellation)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));
            cancellation.ThrowIfCancellationRequested();
			var options = new QueueMessageOptions();
			config?.Invoke(options);
			return SendAsync(dto, options, cancellation);
		}
		 

	}
	*/
}
