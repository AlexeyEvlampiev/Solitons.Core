using System.Net;
using Azure.Messaging.ServiceBus;
using Solitons;
using Solitons.Net.Http;

namespace SampleSoft.SkyNet.Backend
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString =
                "Endpoint=sb://skynet.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=mgT6ntoz1rJFl4hdPWpncWFagrpC8836++ASbEn6xvg=";
            var client = new ServiceBusClient(connectionString,
                new ServiceBusClientOptions()
                {
                    Identifier = "Http Broker",
                    TransportType = ServiceBusTransportType.AmqpWebSockets,
                    RetryOptions = new ServiceBusRetryOptions()
                    {
                        Mode = ServiceBusRetryMode.Fixed,
                        MaxRetries = 5
                    }
                });
            var processor = client.CreateProcessor(
                "rpc",
                "requests",
                new ServiceBusProcessorOptions()
                {
                    AutoCompleteMessages = true,
                    Identifier = "Http Broker",
                    PrefetchCount = 10,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });

            var sender = client.CreateSender("rpc", new ServiceBusSenderOptions()
            {
                Identifier = "Http Broker",
            });

            processor.ProcessErrorAsync += (arg) =>
            {
                ConsoleColor.Red.AsForegroundColor(() => Console.WriteLine(arg.Exception.Message));
                return Task.CompletedTask;
            };

            //processor.ProcessMessageAsync += async (arg) =>
            //{
            //    var http = await HttpMessageBinaryFormatter.ToRequestAsync(arg.Message.Body.ToArray(), CancellationToken.None);
            //    var response = new HttpResponseMessage(HttpStatusCode.OK);
            //    var serviceMusResponse = await ServiceBusHttpMessageHandler.CreateResponseMessageAsync(arg.Message, response, CancellationToken.None);
            //    await sender.SendMessageAsync(serviceMusResponse, CancellationToken.None);
            //};

            await processor.StartProcessingAsync();
            Console.ReadKey();
        }
    }
}