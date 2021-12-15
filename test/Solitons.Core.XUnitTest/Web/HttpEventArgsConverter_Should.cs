using Moq;
using Solitons.Cloud;
using Solitons.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xunit;

namespace Solitons.Web
{
    public class HttpEventArgsConverter_Should
    {
        [Fact]
        public void HandleClaimParameters()
        {
            var expectedUserId = Guid.NewGuid();
            var target = IHttpEventArgsConverter
                .FromTypes(typeof(CustomerOrderRequestHttpEventArgs).ToEnumerable());

            var caller = new ClaimsIdentity();
            caller.AddClaim(new Claim(ClaimTypes.NameIdentifier, expectedUserId.ToString()));
 
            var webRequestMock = new Mock<IWebRequest>();
            webRequestMock.SetupGet(r => r.Uri).Returns($"/products/{Guid.NewGuid()}/orders?count=1");
            webRequestMock.SetupGet(r => r.ClientVersion).Returns(Version.Parse("1.0.0"));
            webRequestMock.SetupGet(r => r.Method).Returns("PUT");
            webRequestMock.SetupGet(r => r.Caller).Returns(new ClaimsPrincipal(caller));

            var httpEventArgs = (CustomerOrderRequestHttpEventArgs)target.Convert(webRequestMock.Object);
            Assert.NotNull(httpEventArgs);
            Assert.Equal(expectedUserId, httpEventArgs.UserId);            
        }


        [Fact]
        public void HandleUrlParameters()
        {
            var expectedProductId = Guid.NewGuid();
            var target = IHttpEventArgsConverter
                .FromTypes(typeof(CustomerOrderRequestHttpEventArgs).ToEnumerable());

            var caller = new ClaimsIdentity();
            caller.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

            var webRequestMock = new Mock<IWebRequest>();
            webRequestMock.SetupGet(r => r.Uri).Returns($"/products/{expectedProductId}/orders?count=1");
            webRequestMock.SetupGet(r => r.ClientVersion).Returns(Version.Parse("1.0.0"));
            webRequestMock.SetupGet(r => r.Method).Returns("PUT");
            webRequestMock.SetupGet(r => r.Caller).Returns(new ClaimsPrincipal(caller));

            var httpEventArgs = (CustomerOrderRequestHttpEventArgs)target.Convert(webRequestMock.Object);
            Assert.NotNull(httpEventArgs);
            Assert.Equal(expectedProductId, httpEventArgs.ProductId);
        }




        [Theory]
        [InlineData("count", 10)]
        [InlineData("itemsCount", 12)]
        [InlineData("items-count", 15)]
        public void HandleQueryParameters(string parameterNameVariant, int expectedParameterValue)
        {
            var expectedProductId = Guid.NewGuid();
            var target = IHttpEventArgsConverter
                .FromTypes(typeof(CustomerOrderRequestHttpEventArgs).ToEnumerable());

            var caller = new ClaimsIdentity();
            caller.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

            var webRequestMock = new Mock<IWebRequest>();
            webRequestMock.SetupGet(r => r.Uri).Returns($"/products/{Guid.NewGuid()}/orders?{parameterNameVariant}={expectedParameterValue}");
            webRequestMock.SetupGet(r => r.ClientVersion).Returns(Version.Parse("1.0.0"));
            webRequestMock.SetupGet(r => r.Method).Returns("PUT");
            webRequestMock.SetupGet(r => r.Caller).Returns(new ClaimsPrincipal(caller));

            var httpEventArgs = (CustomerOrderRequestHttpEventArgs)target.Convert(webRequestMock.Object);
            Assert.NotNull(httpEventArgs);
            Assert.Equal(expectedParameterValue, httpEventArgs.ItemsCount);
        }



        [Fact]
        public void ThrowIfRequiredClaimIsMissing()
        {
            var expectedUserId = Guid.NewGuid();
            var target = IHttpEventArgsConverter
                .FromTypes(typeof(CustomerOrderRequestHttpEventArgs).ToEnumerable());

            var caller = new ClaimsIdentity();

            var webRequestMock = new Mock<IWebRequest>();
            webRequestMock.SetupGet(r => r.Uri).Returns($"/products/{Guid.NewGuid()}/orders?count=1");
            webRequestMock.SetupGet(r => r.ClientVersion).Returns(Version.Parse("1.0.0"));
            webRequestMock.SetupGet(r => r.Method).Returns("PUT");
            webRequestMock.SetupGet(r => r.Caller).Returns(new ClaimsPrincipal(caller));

            Assert.Throws<ClaimNotFoundException>(()=> target.Convert(webRequestMock.Object));
        }



        [Guid("bbd464a1-ce5a-444e-9a7a-947d932913b0")]
        [BasicHttpEventArgs(@".*", "put|post", "/products/(?<id>rgx:guid)/orders")]
        [TargetQueue("orders")]
        public sealed class CustomerOrderRequestHttpEventArgs :
            BasicJsonDataTransferObject,
            IBasicXmlDataTransferObject
        {
            [UrlParameter("id"), JsonPropertyName("productId"), XmlAttribute("ProductId")]
            public Guid ProductId { get; set; }

            [QueryParameter("count", "(?:items?-?)?count"), JsonPropertyName("itemsCount"), XmlAttribute("ItemsCount")]
            public int ItemsCount { get; set; }

            [Claim(ClaimTypes.NameIdentifier, IsRequired = true), JsonPropertyName("userId"), XmlAttribute("UserId")]
            public Guid UserId { get; set; }

            protected override void OnSerializing(object sender)
            {
                if (UserId == Guid.Empty)
                    throw new InvalidOperationException($"{nameof(UserId)} is required.");
                if (ProductId == Guid.Empty)
                    throw new InvalidOperationException($"{nameof(ProductId)} is required.");
                if (ItemsCount < 1)
                    throw new InvalidOperationException($"{nameof(ItemsCount)} is required.");
                base.OnSerializing(sender);
            }
        }
    }
}
