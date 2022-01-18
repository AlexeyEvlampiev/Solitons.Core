// ReSharper disable InconsistentNaming

using System;
using System.Reflection;
using Xunit;

namespace Solitons.Reflection
{
    public sealed class RecursivePropertyInspector_Inspect_Should
    {

        [Fact]
        public void Work()
        {
            var inspector = new QuotationInspector(typeof(TestEntity));
            var entity = new TestEntity("To quote or not to quote");
            
            Assert.Equal("To quote or not to quote", entity.QuotedText);
            Assert.Equal("To quote or not to quote", entity.PlainText);

            inspector.Inspect(entity);

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

        sealed class QuotationInspector : RecursivePropertyInfoInspector
        {
            public QuotationInspector(Type type) : base(new TypePropertiesDictionary(type))
            {
            }

            protected override void Inspect(object target, PropertyInfo property)
            {
                string text = (string)property.GetValue(target);
                property.SetValue(target, text.Quote(QuoteType.Single));
            }

            protected override bool IsTarget(PropertyInfo property) =>
                property.PropertyType == typeof(string) &&
                property.Name.Contains("Quoted");
        }
    }
}
