using System;
using System.Xml;

namespace Solitons.Text.Sql
{
    public abstract class PgRuntimeTextTemplate : RuntimeTextTemplate
    {
        protected override string ToStringWithCulture(object objectToConvert)
        {
            if (objectToConvert is bool boolean)
                return boolean ? "true" : "false";
            if (objectToConvert is TimeSpan interval)
                return XmlConvert.ToString(interval);
            if (objectToConvert is DateTime dateTime)
                return dateTime.ToString("O");
            if (objectToConvert is DateTimeOffset dateTimeOffset)
                return dateTimeOffset.ToString("O");
            return base.ToStringWithCulture(objectToConvert);
        }

        public string Quote(string text)
        {
            if (text is null) return "NULL";
            return text.Quote(QuoteType.SqlLiteral);
        }
    }
}
