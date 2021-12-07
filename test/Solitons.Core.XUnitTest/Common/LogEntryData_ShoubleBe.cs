using System;
using System.Collections.Generic;
using Xunit;

namespace Solitons.Common
{
    public class LogEntryData_ShoubleBe
    {

        [Fact]
        public void XmlSerializable()
        {



            var entry = new LogEntryData()
            {
                Details = "Details go here...",
                Message = "Message goes here",
                Level = LogLevel.Error,
                Properties = new Dictionary<string, string>()
                {
                    ["PropertyA"] = "A property value goes here",
                    ["PropertyB"] = "B property value goes here"
                },
                Tags = new List<string>(){"Tag goes here"},
                Created = DateTimeOffset.UtcNow
            };

            var xml = entry.ToJsonString();
            var copy = IBasicJsonDataTransferObject.FromJson<LogEntryData>(xml);

            Assert.Equal(entry.Properties, copy.Properties);


        }
    }
}
