using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solitons.Web
{
    sealed class CollectionQueryParameterTypeConverter : QueryParameterTypeConverter
    {
        private readonly TypeConverter _itemTypeConverter;
        private readonly Func<object> _create;
        private readonly Action<object, object> _append;
        private readonly Func<object, object> _materialize;
        public CollectionQueryParameterTypeConverter(Type propertyType, string parameterNamePattern) 
            : base(propertyType, parameterNamePattern)
        {
            Type elementType;
            if (propertyType.IsArray)
            {
                elementType = propertyType.GetElementType();
                var listType= typeof(List<>).MakeGenericType(new[] { elementType });

                var append = listType.GetMethod("Add");
                var toArray = listType
                    .GetMethod("ToArray")
                    .ThrowIfNull(()=> new InvalidOperationException());
                _create = () => Activator.CreateInstance(listType);
                _append = (list, item) => append.Invoke(list, new[] {item});
                _materialize = (object list) => toArray.Invoke(list, Array.Empty<object>());
            }
            else if(propertyType.IsGenericType)
            {
                elementType = propertyType.GenericTypeArguments.Single();
                if (propertyType.IsAssignableFrom(typeof(List<>).MakeGenericType(elementType)) ||
                    propertyType.IsAssignableFrom(typeof(IEnumerable<>).MakeGenericType(elementType)))
                {
                    var listType = typeof(List<>).MakeGenericType(new[] { elementType });
                    var append = listType.GetMethod("Add");
                    _create = () => Activator.CreateInstance(listType);
                    _append = (list, item) => append.Invoke(list, new[] { item });
                    _materialize = (object list) => list;
                }
                else if (propertyType.IsAssignableFrom(typeof(Collection<>).MakeGenericType(elementType)))
                {
                    var listType = typeof(Collection<>).MakeGenericType(new[] { elementType });
                    var append = listType.GetMethod("Add");
                    _create = () => Activator.CreateInstance(listType);
                    _append = (list, item) => append.Invoke(list, new[] { item });
                    _materialize = (object list) => list;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            Debug.Assert(elementType != null);
            _itemTypeConverter = TypeDescriptor.GetConverter(elementType);
        }

        protected override IEnumerable<string> Split(string value)
        {
            return Regex.Split(value, @",");
        }

        protected override object ConvertFromQueryParameterValues(IEnumerable<string> values)
        {
            var result = _create.Invoke();
            foreach (var value in values)
            {
                _append.Invoke(result, _itemTypeConverter.ConvertFromInvariantString(value));
            }

            return _materialize.Invoke(result);
        }
    }
}
