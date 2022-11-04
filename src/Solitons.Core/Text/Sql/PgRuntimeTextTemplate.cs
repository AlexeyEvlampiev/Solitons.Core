using System;
using System.Linq;
using System.Xml;

namespace Solitons.Text.Sql
{
    /// <summary>
    /// 
    /// </summary>
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

        protected static string Quote(string text, bool isNullable = true)
        {
            if (text is null)
            {
                return isNullable 
                    ? "NULL" 
                    : throw new ArgumentException("The printed field may not be NULL.", nameof(text));
            }
            return text.Quote(QuoteType.SqlLiteral);
        }

        protected static string ArrayConstructor(string[] items, bool isNullable = true)
        {
            if (items is null)
            {
                return isNullable
                    ? "NULL"
                    : throw new ArgumentException("The printed array may not be NULL.", nameof(items));
            }
            return string.Format("ARRAY[{0}]", items
                .Select(r => r.Trim().Quote(QuoteType.SqlLiteral))
                .Join());
        }
    }
}
