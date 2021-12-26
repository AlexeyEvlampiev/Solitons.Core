using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Solitons
{
    public class DomainContext_GetSerializer_Should
    {
        public sealed class MiddingGuidDto : BasicXmlDataTransferObject { }

        [Guid("aae46e8b-e084-4286-8ee8-27ebd5f693a6")]
        public sealed class FirstDtoWithSameTypeId : BasicXmlDataTransferObject { }

        [Guid("aae46e8b-e084-4286-8ee8-27ebd5f693a6")]
        public sealed class SecondDtoWithSameTypeId : BasicXmlDataTransferObject { }

        [Fact]
        public void ThrowWhenDtoGuidIsMissing()
        {
            var target = IDomainContext.CreateGenericContext(typeof(MiddingGuidDto));
            Assert.Throws<InvalidOperationException>(() => target.GetSerializer());
        }

        [Fact]
        public void ThrowWhenFoundDuplicateTypeIds()
        {
            var target = IDomainContext.CreateGenericContext(
                typeof(FirstDtoWithSameTypeId), 
                typeof(SecondDtoWithSameTypeId));
            Assert.Throws<InvalidOperationException>(() => target.GetSerializer());
        }
    }
}
