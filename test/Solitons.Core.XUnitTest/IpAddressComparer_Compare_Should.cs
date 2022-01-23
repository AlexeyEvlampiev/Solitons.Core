// ReSharper disable InconsistentNaming

using System.Net;
using Solitons.Net;
using Xunit;

namespace Solitons
{
    public sealed class IpAddressComparer_Compare_Should
    {
        [Fact]
        public void Work()
        {
            var target = IpAddressComparer.Instance;
            Assert.Equal(0, target.Compare(IPAddress.Parse("204.120.0.10"), IPAddress.Parse("204.120.0.10")));
            Assert.True(target.Compare(IPAddress.Parse("204.120.0.15"), IPAddress.Parse("204.120.0.10")) > 0);
            Assert.True(target.Compare(IPAddress.Parse("204.120.0.10"), IPAddress.Parse("204.120.0.15")) < 0);
        }
    }
}
