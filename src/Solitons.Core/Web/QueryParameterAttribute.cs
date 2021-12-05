using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Solitons.Web
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class QueryParameterAttribute : Attribute
    {
        private TypeConverter _converter;

        public QueryParameterAttribute(string name, string parameterNamePattern)
        {
            ParameterName = name;
            ParameterNamePattern = parameterNamePattern;
        }

        public string ParameterNamePattern { get; }
        public object ParameterName { get;  }

        internal PropertyInfo TargetProperty { get; private set; }

        public bool IsRequired { get; init; }


        internal object GetValue(IWebRequest request)
        {
            return _converter.ConvertFrom(request);
        }

        public static QueryParameterAttribute Get(PropertyInfo property)
        {
            var result = property?.GetCustomAttribute<QueryParameterAttribute>();
            if (result is null) return null;
            var basicTypes = new HashSet<Type>(){typeof(bool), typeof(string)};
            result.TargetProperty = property;
            if (basicTypes.Contains(property.PropertyType))
            {
                result._converter = new ScalarQueryParameterTypeConverter(property.PropertyType, result.ParameterNamePattern);
            }
            else
            {
                throw new InvalidOperationException();
            }
            return result;
        }
    }
}
