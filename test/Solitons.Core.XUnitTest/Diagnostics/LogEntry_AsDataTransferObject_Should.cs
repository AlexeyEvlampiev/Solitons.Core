
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Solitons.Diagnostics
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LogEntry_AsDataTransferObject_Should
    {
        [Fact]
        public async Task CopyAllEntryFields()
        {
            var logger = IAsyncLogger.Null;
            using var listener = logger
                .AsObservable()
                .Subscribe(OnLog);

            await logger.InfoAsync("This is a test", "This is details", log => log
                .WithProperty("My property key", "My property value")
                .WithTags("This is a tag"));

            void OnLog(ILogEntry entry)
            {
                var comparer = new LogEntryEqualityComparer();
                var json = entry.AsDataTransferObject().ToJsonString();
                var clone = LogEntryData.Parse(json);
                Assert.True(comparer.Equals(entry, clone));
                Assert.Equal("This is a test", clone.Message);
                Assert.Equal("This is details", clone.Details);
                Assert.Equal(new []{ "This is a tag" }, clone.Tags?.ToArray());
                Assert.Equal(new[] { "My property key" }, clone.Properties?.Keys.ToArray());
                Assert.Equal("My property value", clone.Properties!["My property key"]);
                Assert.True(clone.Created > default(DateTimeOffset));
            }
        }


    }
}
