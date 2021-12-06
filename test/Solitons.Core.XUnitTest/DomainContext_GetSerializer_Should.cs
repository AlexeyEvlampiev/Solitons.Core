using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Xml.Linq;
using Newtonsoft.Json;
using Xunit;

namespace Solitons
{
    public class DomainContext_GetSerializer_Should
    {
        private int _counter = 0;


        [Theory]
        [InlineData(TestDomainContextOptions.Json | TestDomainContextOptions.GuidAttribute)]
        [InlineData(TestDomainContextOptions.Xml | TestDomainContextOptions.GuidAttribute)]
        [InlineData(TestDomainContextOptions.Json | TestDomainContextOptions.Xml | TestDomainContextOptions.GuidAttribute)]
        public void Work(TestDomainContextOptions options)
        {
            var code = new TestDomainContextRtt(options)
                .ToString();
            var counter = Interlocked.Increment(ref _counter);
            var assembly = Compiler.Compile($"{nameof(TestDomainContextRtt)}{counter}" ,code);
            var contextType = assembly.GetType("TestCase.TestDomainContext");
            var dtoType = assembly.GetType("TestCase.TestDto");
            Assert.NotNull(contextType);
            Assert.NotNull(dtoType);
            var context = (DomainContext)Activator.CreateInstance(contextType);
            Assert.NotNull(context);
            var serializer = context.GetSerializer();
            if (options.HasFlag(TestDomainContextOptions.Json))
            {
                var json = JsonConvert.SerializeObject(new { text = "Hello world!"});
                Assert.True(serializer.CanDeserialize(dtoType.GUID, "application/json"));
                dynamic obj = serializer.Deserialize(dtoType.GUID, "application/json", json);
                Assert.Equal("Hello world!", obj.Text);
            }
            else
            {
                Assert.False(serializer.CanDeserialize(dtoType.GUID, "application/json"));
            }

            
            if (options.HasFlag(TestDomainContextOptions.Xml))
            {
                var xml = new XElement("Dto", new XAttribute("Text", "Hello world!")).ToString();
                Assert.True(serializer.CanDeserialize(dtoType.GUID, "application/xml"));
                //TODO: uncomment when fixed in .NET
                /*
                dynamic obj = serializer.Deserialize(dtoType.GUID, "application/xml", xml);
                Assert.Equal("Hello world!", obj.Text);
                */
            }
            else
            {
                Assert.False(serializer.CanDeserialize(dtoType.GUID, "application/xml"));
            }

        }
    }
}
