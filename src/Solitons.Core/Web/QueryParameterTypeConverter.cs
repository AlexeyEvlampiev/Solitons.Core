using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solitons.Web
{
    public abstract class QueryParameterTypeConverter : TypeConverter
    {
        private readonly Type _propertyType;
        private readonly Regex _parameterNameRegex;

        protected QueryParameterTypeConverter(Type propertyType, string parameterNamePattern)
        {
            _propertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
            _parameterNameRegex = new Regex(parameterNamePattern, RegexOptions.IgnoreCase);
        }

        public static TypeConverter Create(Type propertyType, string parameterNamePattern)
        {
            if (typeof(IEnumerable).IsAssignableFrom(propertyType) &&
                propertyType != typeof(string))
            {
                return new CollectionQueryParameterTypeConverter(propertyType, parameterNamePattern);
            }
            else
            {
                return new ScalarQueryParameterTypeConverter(propertyType, parameterNamePattern);
            }
        }

        protected abstract object ConvertFromQueryParameterValues(IEnumerable<string> values);

        protected virtual IEnumerable<string> Split(string value)
        {
            yield return value;
        }

        public sealed override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            typeof(IWebRequest).IsAssignableFrom(sourceType);

        public sealed override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            _propertyType == destinationType;

        public sealed override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if(CanConvertFrom(value.GetType()) == false)
                return base.ConvertFrom(context, culture, value);
            var request = ((IWebRequest) value).AsWebRequest();
            return ConvertFromQueryParameterValues(request
                .QueryParameterNames
                .Where(name => _parameterNameRegex.IsMatch(name))
                .SelectMany(name => request.GetQueryParameterValues(name))
                .SelectMany(Split));
        }
    }
}
