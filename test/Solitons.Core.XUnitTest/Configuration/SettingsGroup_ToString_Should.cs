using Xunit;

namespace Solitons.Configuration
{
    // ReSharper disable once InconsistentNaming
    public sealed class SettingsGroup_ToString_Should
    {
        [Fact]
        public void SkipNotRequiredMissingItems()
        {
            var group = new MySettingsGroup();
            var actual = group.ToString();
            Assert.Equal(string.Empty, actual);

            group.NotRequiredField = "test";
            actual = group.ToString();
            Assert.False(string.IsNullOrWhiteSpace(actual));
        }

        
        public sealed class MySettingsGroup : SettingsGroup
        {
            [Setting("not-required", IsRequired = false)]
            public string? NotRequiredField { get; set; }
        }
    }
}
