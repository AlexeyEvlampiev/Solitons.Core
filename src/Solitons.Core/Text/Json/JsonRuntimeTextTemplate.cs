using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;

namespace Solitons.Text.Json
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Solitons.Text.RuntimeTextTemplate" />
    public abstract class JsonRuntimeTextTemplate : RuntimeTextTemplate
    {
        /// <summary>
        /// Converts the given object to a string applying the current culture.
        /// </summary>
        /// <param name="objectToConvert">The object to convert</param>
        /// <returns></returns>
        protected override string ToStringWithCulture(object? objectToConvert)
        {
            if (objectToConvert is null) return "null";
            if (objectToConvert is bool boolean)
                return boolean ? "true" : "false";
            if (objectToConvert is string str) return JsonSerializer.Serialize(str);
            if (objectToConvert is StringBuilder stringBuilder) return JsonSerializer.Serialize(stringBuilder.ToString());
            if (objectToConvert is TimeSpan timeSpan) return XmlConvert.ToString(timeSpan);
            if (objectToConvert is DateTime dateTime) return XmlConvert.ToString(dateTime, XmlDateTimeSerializationMode.Unspecified);
            if (objectToConvert is DateTimeOffset dateTimeOffset) return XmlConvert.ToString(dateTimeOffset);
            if (TrySerialize(objectToConvert, out var json)) return json;
            return base.ToStringWithCulture(objectToConvert);
        }

        /// <summary>
        /// Applies the last transformation before producing the generated text.
        /// </summary>
        /// <param name="text">The text to be transformed.</param>
        /// <returns></returns>
        protected override string PostTransformText(string text)
        {
            text = Regex.Replace(text, @"(?xis-m)(?<=[^\s,{\[])(?=\s+[""][^""]+[""]\s*:)", ",");
            text = Regex.Replace(text,  @"(?xis-m)(?<=[}]])(?=\s*[{])", ",");
            text = Regex.Replace(text, @"(?xis-m),(?=\s*[}\]])", String.Empty);
            return base.PostTransformText(text);
        }

        protected virtual bool TrySerialize(object obj, out string json)
        {
            try
            {
                json = JsonSerializer.Serialize(obj);
                return true;
            }
            catch (NotSupportedException)
            {
                json = null;
                return false;
            }
        }
    }
}
