using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Solitons.Web
{
    public sealed class ScalarQueryParameterTypeConverter : QueryParameterTypeConverter
    {
        private readonly Func<string, object> _parse;
        public ScalarQueryParameterTypeConverter(Type propertyType, string parameterNamePattern) 
            : base(propertyType, parameterNamePattern)
        {
            var defaultConverter = TypeDescriptor.GetConverter(propertyType);

            if (defaultConverter.CanConvertFrom(typeof(string)))
            {
                var parse = propertyType
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Where(m =>
                    {
                        var parameters = m.GetParameters();
                        return m.Name.Equals("Parse", StringComparison.Ordinal) &&
                               parameters.Length == 1 &&
                               parameters[0].ParameterType == typeof(string);
                    })
                    .FirstOrDefault();
                if (parse != null)
                {
                    _parse = (string text) => parse.Invoke(null, new object[]{text});
                    return;
                }
                throw new ArgumentException($"{propertyType} cannot be converted from {typeof(string)}.");
            }

            _parse = defaultConverter.ConvertFromInvariantString;
        }

        protected override object ConvertFromQueryParameterValues(IEnumerable<string> values)
        {
            return values
                .ThrowIfCountExceeds(1, () => new WebResourceSerializerException())
                .Select(_parse)
                .SingleOrDefault();

        }
    }
}
