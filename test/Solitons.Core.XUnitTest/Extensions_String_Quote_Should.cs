

using System;
using System.Diagnostics;
using System.Reactive.Linq;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public class Extensions_String_Quote_Should
    {
        [Theory()]
        [InlineData(QuoteType.Double, "Hello wolrd!", "\"Hello wolrd!\"")]
        [InlineData(QuoteType.Single, "Hello wolrd!", "'Hello wolrd!'")]
        [InlineData(QuoteType.SqlIdentity, "a\"b", "\"a\"\"b\"")]
        [InlineData(QuoteType.SqlLiteral, "O'Reilly", "'O''Reilly'")]
        [InlineData(QuoteType.SqlLiteral, "'O''Reilly'", "'O''Reilly'")]
        public void HandleCommonCases(QuoteType quoteType, string input, string expected)
        {
            var actual = input.Quote(quoteType);
            Assert.Equal(expected, actual, StringComparer.Ordinal);
        }

        [Fact]
        public void SupportAllQuoteTypes()
        {
            Enum.GetValues<QuoteType>()
                .ToObservable()
                .Select(quoteType=> "Hello world!".Quote(quoteType))
                .Do(result=> Debug.WriteLine(result))
                .Subscribe();
        }
    }
}
