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
using Azure.Messaging.ServiceBus.Administration;
using Solitons;
using Solitons.Net.Http;

namespace SampleSoft.SkyNet.Azure.ServiceBus;

public sealed class ServiceBusHttpMessageHandler : BrokeredHttpMessageHandler
{
    private const string TopicName = "rpc";
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

    public static async Task<ServiceBusHttpMessageHandler> CreateAsync(
        string connectionString,
        CancellationToken cancellation)
    {
        var subscriptionName = Guid.NewGuid().ToString("N");
        var adminClient = new ServiceBusAdministrationClient(connectionString);
        var subscription = await adminClient.CreateSubscriptionAsync(
            new CreateSubscriptionOptions(TopicName, subscriptionName)
            {
                Status = EntityStatus.Active, 
                DeadLetteringOnMessageExpiration = true, 
                DefaultMessageTimeToLive = TimeSpan.FromSeconds(10), 
                EnableBatchedOperations = true, 
                ForwardDeadLetteredMessagesTo = "dead-letters", 
                MaxDeliveryCount = 2, 
                UserMetadata = "RPC responses"
            }, cancellation);


        var client = new ServiceBusClient(connectionString,
            new ServiceBusClientOptions()
            {
                Identifier = $"SkyNet-RPC-Sender",
                TransportType = ServiceBusTransportType.AmqpTcp,
                RetryOptions = new ServiceBusRetryOptions()
                {
                    Mode = ServiceBusRetryMode.Fixed,
                    MaxRetries = 2
                }
            });
        var sender = client.CreateSender("rpc", new ServiceBusSenderOptions()
        {
            Identifier = subscriptionName
        });

        


        client.CreateProcessor("rpc", subscription, new ServiceBusProcessorOptions());
        

    }

    protected override Task PublishAsync(
        IBrokeredRequest request, 
        CancellationToken cancellation)
    {
        var serviceBusRequest = (ServiceBusBrokeredRequest)request;
        return _sender.SendMessageAsync(serviceBusRequest.ServiceBusMessage, cancellation);
    }

    protected override async Task<IBrokeredRequest> CreateBrokeredRequestAsync(
        HttpRequestMessage httpRequest, 
        CancellationToken cancellation)
    {
        var httpRequestBytes = await HttpMessageBinaryFormatter.ToByteArrayAsync(
            httpRequest, 
            cancellation);

        return new ServiceBusBrokeredRequest(httpRequestBytes, _serviceBusSessionId);
    }

    protected override Task<HttpResponseMessage> CreateHttpResponseAsync(
        IBrokeredResponse brokeredResponse,
        CancellationToken cancellation)
    {
        var serviceBusBrokeredResponse = (ServiceBusBrokeredResponse)brokeredResponse;
        var serviceBusMessage = serviceBusBrokeredResponse.ServiceBusMessage;
        var body = serviceBusMessage.Body.ToArray();
        return HttpMessageBinaryFormatter.ToResponseAsync(body, cancellation);
    }

    sealed class ServiceBusBrokeredRequest : BrokeredRequest
    {
        public ServiceBusBrokeredRequest(byte[] body, string replyToSessionId)
        {
            ServiceBusMessage = new ServiceBusMessage(body)
            {
                CorrelationId = base.HttpSessionId.ToString("N"),
                ReplyToSessionId = replyToSessionId
            };
        }

        public ServiceBusMessage ServiceBusMessage { get; }
    }

    sealed class ServiceBusBrokeredResponse : IBrokeredResponse
    {
        public ServiceBusBrokeredResponse(ServiceBusMessage serviceBusMessage)
        {
            ServiceBusMessage = serviceBusMessage;
            HttpSessionId = Guid.Parse(serviceBusMessage.CorrelationId);
        }

        public Guid HttpSessionId { get; }

        public ServiceBusMessage ServiceBusMessage { get; }
    }
}