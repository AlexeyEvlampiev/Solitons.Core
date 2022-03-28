using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Solitons.Data
{
    // ReSharper disable once InconsistentNaming
    public sealed class IDatabaseRpcProvider_CreateProxy_Should
    {
        [Fact]
        public async Task CreateValidProxy()
        {
            var serializer = DataContractSerializer
                .CreateBuilder()
                .Add(typeof(Request), IMediaTypeSerializer.BasicJsonSerializer)
                .Add(typeof(Response), IMediaTypeSerializer.BasicJsonSerializer)
                .Build();

            var target = TestDatabaseRpcProvider
                .Create(InvokeAsync, serializer)
                .CreateProxy<ITestDatabaseApi>();

            var response = await target.InvokeAsync(new Request(){ Text = "This is a test request"});
            Assert.Equal("This is a test response", response.Text);
        }

        private Task<string> InvokeAsync(DbCommandAttribute args, string payload, CancellationToken cancellation)
        {
            var expectedOid = Guid.Parse("626fc813-514e-4dfa-b59b-3e78942db2ed");
            var expectedProcedure = "my-procedure";
            Assert.Equal(expectedOid, args.Oid);
            Assert.Equal(expectedProcedure, args.Procedure);
            Assert.Equal(typeof(Request), args.RequestType);
            Assert.Equal(typeof(Response), args.ResponseType);

            var request = JsonSerializer.Deserialize<Request>(payload);
            Assert.Equal("This is a test request", request!.Text);
            var response = JsonSerializer.Serialize(new Response() { Text = "This is a test response" });
            return Task.FromResult(response);
        }

        public interface ITestDatabaseApi
        {
            [DbCommand("626fc813-514e-4dfa-b59b-3e78942db2ed", "my-procedure")]
            Task<Response> InvokeAsync(Request request, CancellationToken cancellation = default);
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
