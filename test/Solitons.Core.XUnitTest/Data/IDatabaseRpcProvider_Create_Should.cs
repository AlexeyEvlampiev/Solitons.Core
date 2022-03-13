using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Solitons.Data
{
    // ReSharper disable once InconsistentNaming
    public sealed class IDatabaseRpcProvider_Create_Should
    {
        [Fact]
        public async Task CreateValidProxy()
        {
            var mock = new Mock<IDatabaseRpcProvider>();
            mock
                .Setup(i => i.InvokeAsync(It.IsAny<DbCommandAttribute>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DbCommandAttribute args, string payload, CancellationToken cancellation) =>
                {
                    var expectedOid = Guid.Parse("626fc813-514e-4dfa-b59b-3e78942db2ed");
                    var expectedProcedure = "my-procedure";
                    Assert.Equal(expectedOid, args.Oid);
                    Assert.Equal(expectedProcedure, args.Procedure);
                    Assert.Equal(typeof(Request), args.RequestType);
                    Assert.Equal(typeof(Response), args.ResponseType);

                    var request = JsonSerializer.Deserialize<Request>(payload);
                    Assert.Equal("This is a test request", request.Text);

                    var response = new Response() { Text = "This is a test response" };
                    return JsonSerializer.Serialize(response);
                });

            mock.Setup(i => i.Serialize(It.IsAny<object>(), "application/json"))
                .Returns((object obj, string _) => JsonSerializer.Serialize(obj));
            mock.Setup(i => i.Deserialize(It.IsAny<string>(), "application/json", It.IsAny<Type>()))
                .Returns((string content, string _, Type type) => JsonSerializer.Deserialize(content, type));

            var databaseRpcClient = DatabaseRpcProviderProxy.Wrap(mock.Object);

            var target = databaseRpcClient.Create<IDbRpc>();

            Debug.WriteLine(target.ToString());


            var response = await target.InvokeAsync(new Request(){ Text = "This is a test request"});
            Assert.Equal("This is a test response", response.Text);

        }
        public interface IDbRpc
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
