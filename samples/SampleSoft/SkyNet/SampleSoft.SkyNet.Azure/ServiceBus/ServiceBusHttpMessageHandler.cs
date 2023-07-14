using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Solitons;
using Solitons.Net.Http;

namespace SampleSoft.SkyNet.Azure.ServiceBus;

public sealed class ServiceBusHttpMessageHandler : BrokeredHttpMessageHandler
{
    private readonly ServiceBusSender _sender;
    private readonly ServiceBusProcessor _processor;
    private readonly IObserver<ServiceBusBrokeredResponse> _responses;
    private readonly string _serviceBusSessionId = Guid.NewGuid().ToString();

    private ServiceBusHttpMessageHandler(
        ServiceBusSender sender,
        ServiceBusProcessor processor,
        EventLoopScheduler scheduler) 
        : this(new Subject<ServiceBusBrokeredResponse>(), scheduler)
    {
        _sender = sender;
        _processor = processor;
    }

    [DebuggerStepThrough]
    private ServiceBusHttpMessageHandler(
        Subject<ServiceBusBrokeredResponse> responses,
        EventLoopScheduler scheduler) : base(responses, scheduler)
    {
        _responses = responses.AsObserver();
    }


    protected override Task PublishAsync(
        IBrokeredRequest request, 
        CancellationToken cancellation)
    {
        var serviceBusRequest = (ServiceBusBrokeredRequest)request;
        return _sender.SendMessageAsync(serviceBusRequest.ServiceBusMessage, cancellation);
    }

    protected override IBrokeredRequest CreateBrokeredRequest(
        HttpMethod statusCode, 
        Uri? requestUri, 
        HttpHeaders headers,
        HttpHeaders? trailingHeaders, 
        byte[] content, 
        Version version)
    {
        throw new NotImplementedException();
    }

    protected override HttpResponseMessage CreateHttpResponse(IBrokeredResponse brokeredResponse)
    {
        throw new NotImplementedException();
    }

    sealed class ServiceBusBrokeredRequest : BrokeredRequest
    {
        public static async Task<ServiceBusBrokeredRequest> CreateAsync(
            HttpRequestMessage request, 
            string serviceBusSessionId,
            CancellationToken cancellation)
        {
            var content = Array.Empty<byte>();
            if (request?.Content != null)
            {
                content = await request.Content.ReadAsByteArrayAsync(cancellation);
            }

            var serviceBusMessage = new ServiceBusMessage(content)
            {
                ReplyToSessionId = serviceBusSessionId
            };

            foreach (var httpRequestHeader in request!.Headers)
            {
                var value = httpRequestHeader.Value.Join(",");
                serviceBusMessage.ApplicationProperties.Add(httpRequestHeader.Key, value);
            }
            

            return new ServiceBusBrokeredRequest(serviceBusMessage);
        }
        public ServiceBusBrokeredRequest(ServiceBusMessage request)
        {
            ServiceBusMessage = request;
            request.CorrelationId = base.HttpSessionId.ToString("N");
        }

        public ServiceBusMessage ServiceBusMessage { get; }
    }

    sealed class ServiceBusBrokeredResponse : IBrokeredResponse
    {
        public Guid HttpSessionId => throw new NotImplementedException();
    }
}