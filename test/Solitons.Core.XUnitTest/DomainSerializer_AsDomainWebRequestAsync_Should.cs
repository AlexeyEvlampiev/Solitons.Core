using Moq;
using Solitons.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xunit;

namespace Solitons
{
    public class DomainSerializer_AsDomainWebRequestAsync_Should
    {
        const string TestDtoTypeId = "12300000-0000-0000-0000-000000000000";

        [Fact]
        public async Task ThrowOnContentTypeNotSupported()
        {
            var webRequest = new Mock<IWebRequest>();
            var caller = new ClaimsIdentity();
            caller.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            caller.AddClaim(new Claim("tid", Guid.NewGuid().ToString()));

            webRequest.SetupGet(r => r.Uri).Returns($"/users/{Guid.NewGuid()}");
            webRequest.SetupGet(r => r.ClientVersion).Returns(Version.Parse("2.0.1"));
            webRequest.SetupGet(r => r.Method).Returns("POST");
            webRequest.SetupGet(r => r.Caller).Returns(new ClaimsPrincipal(caller));
            webRequest.SetupGet(r => r.ContentType).Returns("some-non-existing-content-type");
            webRequest.SetupGet(r => r.IPAddress).Returns(IPAddress.Parse("3.4.5.6"));
            webRequest.Setup(r => r.GetBody()).Returns("some-non-existing-content".ToMemoryStream(Encoding.UTF8));


            var target = IDomainSerializer.FromTypes(typeof(UpdateUserHttpEventArgs), typeof(UserAttributeData));
            var expection = await Assert.ThrowsAsync<NotSupportedException>(() => target.AsDomainWebRequestAsync(webRequest.Object));
            Debug.WriteLine(expection.Message);

        }


        [Fact]
        public async Task CastMatchedWebRequestsWithNoPayload()
        {
            var webRequest = new Mock<IWebRequest>();
            var caller = new ClaimsIdentity();
            caller.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

            webRequest.SetupGet(r => r.Uri).Returns($"/products/{Guid.NewGuid()}/orders?itemsCount=1");
            webRequest.SetupGet(r => r.ClientVersion).Returns(Version.Parse("1.0.0"));
            webRequest.SetupGet(r => r.Method).Returns("PUT");
            webRequest.SetupGet(r => r.Caller).Returns(new ClaimsPrincipal(caller));
            webRequest.SetupGet(r => r.IPAddress).Returns(IPAddress.Parse("1.2.3.4"));
            var target = IDomainSerializer.FromTypes(typeof(CustomerOrderRequestHttpEventArgs));

            var domainWebRequest = await target.AsDomainWebRequestAsync(webRequest.Object);
            Assert.NotNull(domainWebRequest);
            Assert.Equal(IPAddress.Parse("1.2.3.4"), domainWebRequest.IPAddress);
            Assert.Equal("PUT", domainWebRequest.Method);
            Assert.Null(domainWebRequest.ContentType);
            Assert.Equal(Version.Parse("1.0.0"), domainWebRequest.ClientVersion);
            var httpEventArgs = (CustomerOrderRequestHttpEventArgs)domainWebRequest.HttpEventArgs;
            Assert.Null(domainWebRequest.MessageBody);
        }


        [Theory]
        [InlineData("image-1 data")]
        [InlineData("image-2 data")]
        public async Task CastMatchedWebRequestsWithStreamPayload(string expectedImageString)
        {
            var webRequest = new Mock<IWebRequest>();
            var caller = new ClaimsIdentity();
            caller.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            caller.AddClaim(new Claim("tid", Guid.NewGuid().ToString()));

            webRequest.SetupGet(r => r.Uri).Returns($"/images/{Guid.NewGuid()}");
            webRequest.SetupGet(r => r.ClientVersion).Returns(Version.Parse("1.0.1"));
            webRequest.SetupGet(r => r.Method).Returns("POST");
            webRequest.SetupGet(r => r.Caller).Returns(new ClaimsPrincipal(caller));
            webRequest.SetupGet(r => r.ContentType).Returns("application/octet-stream");
            webRequest.SetupGet(r => r.IPAddress).Returns(IPAddress.Parse("2.3.4.5"));
            webRequest.Setup(r => r.GetBody()).Returns(expectedImageString.ToMemoryStream(Encoding.UTF8));
            var target = IDomainSerializer.FromTypes(typeof(ImageUploadHttpEventArgs));

            var domainWebRequest = await target.AsDomainWebRequestAsync(webRequest.Object);
            Assert.NotNull(domainWebRequest);
            var httpEventArgs = (ImageUploadHttpEventArgs)domainWebRequest.HttpEventArgs;
            var payload = (Stream)domainWebRequest.MessageBody;

            using var reader = new StreamReader(payload);
            var actualImageString = await reader.ReadToEndAsync();
            Assert.Equal(actualImageString, expectedImageString);

            Assert.Equal(IPAddress.Parse("2.3.4.5"), domainWebRequest.IPAddress);
            Assert.Equal("POST", domainWebRequest.Method);
            Assert.Equal("application/octet-stream", domainWebRequest.ContentType);
            Assert.Equal(Version.Parse("1.0.1"), domainWebRequest.ClientVersion);
        }


        [Theory]
        [InlineData("alex@contoso.com")]
        [InlineData("john@contoso.com")]
        public async Task CastMatchedWebRequestsWithDtoPaylod(string expectedEmail)
        {
            var webRequest = new Mock<IWebRequest>();
            var caller = new ClaimsIdentity();
            caller.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
            caller.AddClaim(new Claim("tid", Guid.NewGuid().ToString()));

            webRequest.SetupGet(r => r.Uri).Returns($"/users/{Guid.NewGuid()}");
            webRequest.SetupGet(r => r.ClientVersion).Returns(Version.Parse("2.0.1"));
            webRequest.SetupGet(r => r.Method).Returns("POST");
            webRequest.SetupGet(r => r.Caller).Returns(new ClaimsPrincipal(caller));
            webRequest.SetupGet(r => r.ContentType).Returns("application/json");
            webRequest.SetupGet(r => r.IPAddress).Returns(IPAddress.Parse("3.4.5.6"));
            webRequest.Setup(r => r.GetBody()).Returns(
                new UserAttributeData { Email = expectedEmail }
                .ToJsonString()
                .ToMemoryStream(Encoding.UTF8));


            var target = IDomainSerializer.FromTypes(typeof(UpdateUserHttpEventArgs), typeof(UserAttributeData));

            var domainWebRequest = await target.AsDomainWebRequestAsync(webRequest.Object);
            Assert.NotNull(domainWebRequest);
            var httpEventArgs = (UpdateUserHttpEventArgs)domainWebRequest.HttpEventArgs;
            var payload = (UserAttributeData)domainWebRequest.MessageBody;

            Assert.Equal(IPAddress.Parse("3.4.5.6"), domainWebRequest.IPAddress);
            Assert.Equal("POST", domainWebRequest.Method);
            Assert.Equal(Version.Parse("2.0.1"), domainWebRequest.ClientVersion);
            Assert.Equal("application/json", domainWebRequest.ContentType);

            Assert.Equal(expectedEmail, payload.Email);
        }

        [Guid(TestDtoTypeId)]
        [HttpEventArgs(@".*", "put", "/products/(?<id>rgx:guid)/orders")]
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

        [Guid(TestDtoTypeId)]
        [HttpEventArgs(@".*", "post", "/images/(?<id>rgx:guid)", PayloadType = typeof(Stream))]
        public sealed class ImageUploadHttpEventArgs
        {
            [Claim(ClaimTypes.NameIdentifier, IsRequired = true), JsonPropertyName("userId"), XmlAttribute("UserId")]
            public Guid UserId { get; set; }

            [Claim("tid", IsRequired = true), JsonPropertyName("userId"), XmlAttribute("UserId")]
            public Guid TenantId { get; set; }
        }

        [Guid(TestDtoTypeId)]
        [HttpEventArgs(@".*", "post", "/users/(?<id>rgx:guid)", PayloadType = typeof(UserAttributeData))]
        public sealed class UpdateUserHttpEventArgs : BasicJsonDataTransferObject
        {
            [Claim(ClaimTypes.NameIdentifier, IsRequired = true), JsonPropertyName("userId"), XmlAttribute("UserId")]
            public Guid UserId { get; set; }

            [Claim("tid", IsRequired = true), JsonPropertyName("tenantId"), XmlAttribute("TenantId")]
            public Guid TenantId { get; set; }
        }

        [Guid("23400000-0000-0000-0000-000000000000")]
        public sealed class UserAttributeData : BasicJsonDataTransferObject, IBasicXmlDataTransferObject
        {
            [JsonPropertyName("email")]
            public string Email { get; set; }
        }
    }
}
