using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBasicDataTransferObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        [DebuggerStepThrough]
        public string ToString(string contentType)
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            if (comparer.Equals("application/json", contentType))
            {
                return this is IBasicJsonDataTransferObject jsonDataTransferObject
                    ? jsonDataTransferObject.ToJsonString()
                    : throw new NotSupportedException();
            }

            if (comparer.Equals("application/xml", contentType))
            {
                return this is IBasicXmlDataTransferObject xmlDataTransferObject
                    ? xmlDataTransferObject.ToXmlString()
                    : throw new NotSupportedException();
            }

            if (comparer.Equals("text/plain", contentType))
            {
                return this.ToString();
            }

            throw new NotSupportedException();
        }

        [DebuggerStepThrough]
        public string ToString(out string contentType)
        {
            if (this is BasicJsonDataTransferObject ||
                this is BasicJsonDataTransferRecord)
            {
                contentType = "application/json";
                return (this as IBasicJsonDataTransferObject)?
                    .ToJsonString();
            }

            if (this is BasicXmlDataTransferObject)
            {
                contentType = "application/json";
                return (this as IBasicXmlDataTransferObject)?
                    .ToXmlString();
            }

            if (this is IBasicJsonDataTransferObject jsonObj)
            {
                contentType = "application/json";
                return jsonObj.ToJsonString();
            }

            if (this is IBasicXmlDataTransferObject xmlObj)
            {
                contentType = "application/xml";
                return xmlObj.ToXmlString();
            }

            contentType = "text/plain";
            return ToString();
        }


        public static T Parse<T>(string input, string contentType) where T : class, IBasicDataTransferObject
        {
            var comparer = StringComparer.OrdinalIgnoreCase;
            if (comparer.Equals("application/json", contentType))
            {
                return typeof(IBasicJsonDataTransferObject).IsAssignableFrom(typeof(T))
                    ? (T)IBasicJsonDataTransferObject.Parse(input, typeof(T))
                    : throw new NotSupportedException();
            }

            if (comparer.Equals("application/xml", contentType))
            {
                return typeof(IBasicXmlDataTransferObject).IsAssignableFrom(typeof(T))
                    ? (T)IBasicXmlDataTransferObject.Parse(input, typeof(T))
                    : throw new NotSupportedException();
            }

            if (comparer.Equals("text/plain", contentType))
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return converter.CanConvertFrom(typeof(string))
                    ? (T)converter.ConvertFrom(input)
                    : throw new NotSupportedException();
            }

            throw new NotSupportedException();
        }
    }

    public static partial class Extensions 
    {
        [DebuggerStepThrough]
        public static string ToString(this IBasicDataTransferObject self, string contentType) => self.ToString(contentType);
    }
}
