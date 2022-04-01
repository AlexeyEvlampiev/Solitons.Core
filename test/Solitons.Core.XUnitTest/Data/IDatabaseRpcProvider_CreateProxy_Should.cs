using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Solitons.Data
{
    // ReSharper disable once InconsistentNaming
    public sealed class IDatabaseRpcProvider_CreateProxy_Should
    {
        [Fact]
        public async Task HandleRpcSignature()
        {
            var serializer = DataContractSerializer
                .CreateBuilder()
                .Add(typeof(Request), IMediaTypeSerializer.BasicJsonSerializer)
                .Add(typeof(Response), IMediaTypeSerializer.BasicJsonSerializer)
                .Build();

            var mock = new Mock<IDatabaseRpcProvider>();
            mock.Setup(p => p.InvokeAsync(
                    It.IsAny<DbCommandAttribute>(),
                    It.IsAny<object>(),
                    serializer,
                    null,
                    CancellationToken.None))
                .Callback((
                    DbCommandAttribute annotation, 
                    object pRequest, 
                    IDataContractSerializer pSerializer, 
                    Func<object, Task> onResponseAsync, 
                    CancellationToken cancellation) =>
                {
                    var expectedOid = Guid.Parse("626fc813-514e-4dfa-b59b-3e78942db2ed");
                    var expectedProcedure = "basic-rpc";
                    Assert.Equal(expectedOid, annotation.CommandId);
                    Assert.Equal(expectedProcedure, annotation.Procedure);
                    Assert.Equal(typeof(Request), annotation.RequestType);
                    Assert.Equal(typeof(Response), annotation.ResponseType);
                    var r = (Request)pRequest;
                    Assert.Equal("This is a test request", r.Text);
                    Assert.True(ReferenceEquals(serializer, pSerializer));
                    Assert.Null(onResponseAsync);
                    Assert.False(cancellation.IsCancellationRequested);

                })
                .ReturnsAsync(new Response(){ Text = "This is a test request" });

            var target = DatabaseRpcProviderProxy
                .Wrap(mock.Object)
                .CreateProxy<ITestDatabaseApi>(serializer);

            var response = await target.InvokeAsync(new Request{ Text = "This is a test request"});
            Assert.Equal(response.Text, "This is a test request");

        }

        [Fact]
        public async Task HandleCallbackSignature()
        {
            var serializer = DataContractSerializer
                .CreateBuilder()
                .Add(typeof(Request), IMediaTypeSerializer.BasicJsonSerializer)
                .Add(typeof(Response), IMediaTypeSerializer.BasicJsonSerializer)
                .Build();

            var mock = new Mock<IDatabaseRpcProvider>();
            var response = new Response() { Text = "This is a test response" };
            mock.Setup(p => p.InvokeAsync(
                    It.IsAny<DbCommandAttribute>(),
                    It.IsAny<object>(),
                    serializer,
                    It.IsAny<Func<object, Task>>(),
                    CancellationToken.None))
                .Callback((
                    DbCommandAttribute annotation,
                    object pRequest,
                    IDataContractSerializer pSerializer,
                    Func<object, Task> onResponseAsync,
                    CancellationToken cancellation) =>
                {
                    var expectedOid = Guid.Parse("c8c896e9-51ec-442f-8066-206163716a39");
                    var expectedProcedure = "callback-procedure";
                    Assert.Equal(expectedOid, annotation.CommandId);
                    Assert.Equal(expectedProcedure, annotation.Procedure);
                    Assert.Equal(typeof(Request), annotation.RequestType);
                    Assert.Equal(typeof(Response), annotation.ResponseType);
                    var r = (Request)pRequest;
                    Assert.Equal("This is a test request", r.Text);
                    Assert.True(ReferenceEquals(serializer, pSerializer));
                    Assert.NotNull(onResponseAsync);
                    onResponseAsync.Invoke(response);
                    Assert.False(cancellation.IsCancellationRequested);

                })
                .ReturnsAsync(response);

            var target = DatabaseRpcProviderProxy
                .Wrap(mock.Object)
                .CreateProxy<ITestDatabaseApi>(serializer);

            bool callbackInvoked = false;
            await target.InvokeAsync(
                new Request() { Text = "This is a test request" },
                (pResponse) =>
                {
                    callbackInvoked = true;
                    Assert.Equal("This is a test response", pResponse.Text);
                    return Task.CompletedTask;
                });
            Assert.True(callbackInvoked);
        }

        public interface ITestDatabaseApi
        {
            [DbCommand("626fc813-514e-4dfa-b59b-3e78942db2ed", "basic-rpc")]
            Task<Response> InvokeAsync(Request request, CancellationToken cancellation = default);

            [DbCommand("c8c896e9-51ec-442f-8066-206163716a39", "callback-procedure")]
            Task InvokeAsync(
                Request request,
                Func<Response, Task> callback,
                CancellationToken cancellation = default);
        }

        public sealed class Request
        {
            public string Text { get; set; }
        }

        public sealed class Response
        {
            public string Text { get; set; }
        }
    }
}
