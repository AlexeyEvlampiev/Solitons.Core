using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Solitons.Collections;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public sealed class Extensions_Convert_Should
    {
        [Fact]
        public async Task EnableFluentApi()
        {
            Assert.Equal(123, await 123
                .Convert(Observable.Return)
                .SingleAsync());

            Assert.Equal(123, 123
                .Convert(FluentEnumerable.Yield)
                .Single());

            Assert.Equal(
                Guid.Parse("1607549d-60a7-4f93-a9cf-363b1a0f6013"), 
                "1607549d-60a7-4f93-a9cf-363b1a0f6013".Convert(Guid.Parse));
        }
    }
}
