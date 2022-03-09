using Solitons.Data;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public class Extensions_SortOrder_Inverse_Should
    {
        [Fact]
        public void Inverse()
        {
            var target = SortOrder.Asc;
            Assert.Equal(SortOrder.Desc, target.Invert());
            Assert.Equal(SortOrder.Asc, target.Invert().Invert());
            Assert.Equal(SortOrder.Desc, target.Invert().Invert().Invert());
            Assert.Equal(SortOrder.Asc, target.Invert().Invert().Invert().Invert());
        }
    }
}
