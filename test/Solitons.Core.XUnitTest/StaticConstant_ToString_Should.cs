using System;
using Xunit;

namespace Solitons;

// ReSharper disable once InconsistentNaming
public sealed class StaticConstant_ToString_Should
{
    [Fact]
    public void Work()
    {
        var admin = RoleCode.Admin;
        Assert.True(Guid.TryParse(admin, out var guid));
        Assert.Equal(Guid.Parse("46c71cd5-3b14-425f-a633-ceab3487306d"), admin);
    }

    public sealed record RoleCode : StaticConstant<Guid>
    {
        public static readonly RoleCode Admin = new RoleCode("46c71cd5-3b14-425f-a633-ceab3487306d");
        private RoleCode(string guid) : base(Guid.Parse(guid))
        {
        }
    }
}