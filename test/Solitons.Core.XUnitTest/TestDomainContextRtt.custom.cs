using Solitons.Common;

namespace Solitons
{
    public partial class TestDomainContextRtt
    {
        private readonly TestDomainContextOptions _options;

        public TestDomainContextRtt(TestDomainContextOptions options)
        {
            _options = options;
        }

        public bool Json => _options.HasFlag(TestDomainContextOptions.Json);

        public bool Xml => _options.HasFlag(TestDomainContextOptions.Xml);

        public bool GuidAttribute => _options.HasFlag(TestDomainContextOptions.GuidAttribute);

        public string BasicJsonSerializer => typeof(BasicJsonDataTransferObjectSerializer).FullName;

        public string BasicXmlSerializer => typeof(BasicXmlDataTransferObjectSerializer).FullName;
    }

    public enum TestDomainContextOptions
    {
        None = 0,
        Json = 1,
        Xml = 2,
        GuidAttribute = 4
    }
}
