using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Solitons.Collections;

// ReSharper disable once InconsistentNaming
public sealed class KeyValuePairCollection_ParseCsvLine_Should
{
    [Theory]
    [InlineData("a=b;c=d", ';', "a=b;c=d")]
    [InlineData("a = b ; c = d", ';', "a=b;c=d")]
    [InlineData("a=b,c=d", ',', "a=b;c=d")]
    [InlineData("a=b|c=d", '|', "a=b;c=d")]
    [InlineData("a=b\tc=d", '\t', "a=b;c=d")]
    [InlineData(@"msg1 = ""Hello world!"" ; msg2 = ""This is a test"" ", ';', @"msg1=Hello world!;msg2=This is a test")]
    public void HandleValidLines(string input, char delimiter, string expectedLine)
    {
        Debug.WriteLine(input);
        var expected = Regex
            .Split(expectedLine, ";")
            .Select(equation =>
            {
                var pair = Regex.Split(equation, @"(?<=^[^\s=]+)=");
                return KeyValuePair.Create(pair[0], pair[1].Trim('"'));
            })
            .OrderBy(_ => _.Key)
            .ToArray();
        var actual = KeyValuePairCollection
            .ParseCsvLine(input, delimiter)
            .Select(pair => KeyValuePair.Create(pair.Key, pair.Value.Trim('"')))
            .OrderBy(_ => _.Key)
            .ToArray();
        
        Assert.Equal(expected, actual);
    }
}