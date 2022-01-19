// ReSharper disable InconsistentNaming

using System;
using System.Reflection;
using System.Threading.Tasks;
using Solitons.Reflection.Common;
using Xunit;

namespace Solitons.Reflection
{
    public sealed class RecursivePropertyInspector_Inspect_Should
    {

        [Fact]
        public async Task WorkAsync()
        {
            var recursiveInspector = RecursivePropertyInspector
                .Create(new QuotationInspector());

            var entity = new TestEntity("To quote or not to quote");
            
            Assert.Equal("To quote or not to quote", entity.QuotedText);
            Assert.Equal("To quote or not to quote", entity.PlainText);

            await recursiveInspector.InspectAsync(entity);

            Assert.Equal("'To quote or not to quote'", entity.QuotedText);
            Assert.Equal("To quote or not to quote", entity.PlainText);
        }


        public sealed class TestEntity
        {
            public TestEntity(string text)
            {
                QuotedText = text;
                PlainText = text;
            }
            public string QuotedText { get; set; }
            public string PlainText { get; set; }
        }

        sealed class QuotationInspector : PropertyInspector
        {

            protected override void Inspect(object target, PropertyInfo property)
            {
                string text = (string)property.GetValue(target);
                property.SetValue(target, text.Quote(QuoteType.Single));
            }

            protected override bool IsTargetProperty(PropertyInfo property) =>
                property.PropertyType == typeof(string) &&
                property.Name.Contains("Quoted");
        }
    }
}
