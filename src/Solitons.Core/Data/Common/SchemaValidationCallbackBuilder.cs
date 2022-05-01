using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class SchemaValidationCallbackBuilder : ISchemaValidationCallbackBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public virtual SchemaValidationCallback Build(string contentType, string schema)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals("application/xml", contentType))
            {
                using var textReader = new StringReader(schema);
                using var xmlReader = XmlReader.Create(textReader);
                var settings = new XmlReaderSettings();
                var xmlSchema = XmlSchema.Read(textReader, (sender, args) =>
                {
                    if (args.Severity == XmlSeverityType.Error)
                        throw new InvalidOperationException($"Invalid XSD. {args.Message}");
                });
                settings.Schemas.Add(xmlSchema!);
                settings.ValidationEventHandler += (sender, args) =>
                {
                    if (args.Severity == XmlSeverityType.Error)
                    {
                        throw args.Exception;
                    }
                };
            }

            if (StringComparer.OrdinalIgnoreCase.Equals("text/plain", contentType))
            {
                var regex = new Regex(schema);

                return (string content,  out string comment) =>
                {
                    if (regex.IsMatch(content))
                    {
                        comment = "Match";
                        return true;
                    }

                    comment = "No REGEXP match";
                    return false;
                };
            }

            throw new NotSupportedException($"Specified content type not supported. Content type: \"{contentType}\"");
        }

    }
}
