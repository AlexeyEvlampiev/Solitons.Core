using Solitons.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Solitons.Web
{
    sealed class HttpEventArgsConverter : IHttpEventArgsConverter
    {
        sealed record RequestBoundField(
            PropertyInfo PropertyInfo,
            TypeConverter TypeConverter,
            Attribute[] Attributes);
        sealed record HttpEventArgsEntry(
            IHttpEventArgsAttribute HttpEventArgsMetadata,
            Type HttpEventArgsType,
            RequestBoundField[] WebBoundFields);

        private readonly Dictionary<IHttpEventArgsAttribute, HttpEventArgsEntry> _entries;


        internal HttpEventArgsConverter(IEnumerable<Type> types)
        {
            var webPropertyAttributeTypes = new HashSet<Type>()
            {
                typeof(UrlParameterAttribute),
                typeof(QueryParameterAttribute),
                typeof(ClaimAttribute)
            };

            var typeConverters = new Dictionary<Type, TypeConverter>();


            _entries =
                (from type in types
                 from httpEventArgsAtt in type.GetCustomAttributes().OfType<IHttpEventArgsAttribute>()
                 where httpEventArgsAtt != null
                 let webProperties = 
                    from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                     let att = p.GetCustomAttributes()
                         .Where(a => webPropertyAttributeTypes.Contains(a.GetType()))
                         .ToList()
                     where att.Count > 0
                    let converter = typeConverters.GetOrAdd(p.PropertyType, ()=> TypeDescriptor.GetConverter(p.PropertyType))
                    select new RequestBoundField(p, converter, att.ToArray())
                 select new HttpEventArgsEntry(
                     httpEventArgsAtt,
                     type,
                     webProperties.ToArray())
                ).ToDictionary(i => i.HttpEventArgsMetadata);

            _entries.Values
                .SelectMany(i => i.WebBoundFields)
                .ForEach(field => 
                {
                    var property = field.PropertyInfo;
                    if (false == field.TypeConverter.CanConvertFrom(typeof(string)))
                        throw new InvalidOperationException(new StringBuilder("String conversion is not dupported.")
                            .Append($"{property.DeclaringType}.{property.Name} property of type {property.PropertyType} declared as HTTP request bound cannot be parsed from {typeof(string)}.")                            
                            .ToString());
                    if (property.SetMethod is null)
                        throw new InvalidOperationException(new StringBuilder("Property setter is required.")
                             .Append($"{property.DeclaringType}.{property.Name} property declared as HTTP request is required to have a setter method.")
                            .ToString());
                });

        }



        public object Convert(IWebRequest request, out IHttpEventArgsAttribute metadata)
        {
            metadata = null;
            if (request == null) throw new ArgumentNullException(nameof(request));
            request = WebRequestProxy.Wrap(request.ThrowIfNullArgument(nameof(request)));
            var version = request.ClientVersion.ToString();

            metadata = _entries
                .Keys
                .Where(r =>
                {
                    bool success = true;
                    success &= r.IsUriMatch(request.Uri);
                    success &= r.IsMethodMatch(request.Method);
                    success &= r.IsVersionMatch(version);
                    return success;
                })
                .FirstOrDefault();

            if (metadata is null) return null;
            var entry = _entries[metadata];
            var webQueryDto = Activator.CreateInstance(entry.HttpEventArgsType);
            Populate(webQueryDto, entry, request);
            return webQueryDto;
        }



        private void Populate(object webQueryDto, HttpEventArgsEntry entry, IWebRequest request)
        {
            var match = entry.HttpEventArgsMetadata.MatchUri(request.Uri);
            Debug.Assert(match.Success);
            
            foreach (var field in entry.WebBoundFields)
            {
                var (property, attributes) = (field.PropertyInfo, field.Attributes);

                attributes
                    .OfType<UrlParameterAttribute>()
                    .ForEach(att =>
                    {
                        var group = match.Groups[att.RegexGroupName];
                        if (group.Success)
                        {
                            var value = field.TypeConverter.ConvertFromInvariantString(group.Value);
                            property.SetValue(webQueryDto, value);
                        }
                    });
                attributes
                    .OfType<QueryParameterAttribute>()
                    .ForEach(att =>
                    {
                        if (att.TryGetValue(request.Uri, out var valueString))
                        {
                            var value = field.TypeConverter.ConvertFrom(valueString);
                            property.SetValue(webQueryDto, value);
                        }
                        else if (att.IsRequired)
                        {
                            throw new QueryParameterNotFoundException(att.ParameterName, $"{property.Name} property is required.");
                        }
                    });
                attributes
                    .OfType<ClaimAttribute>()
                    .ForEach(att => 
                    {
                        var valueString = request.Caller.Claims
                            .Where(c => c.Type.Equals(att.ClaimTypeName))
                            .Select(c => c.Value)
                            .FirstOrDefault();
                        if (valueString.IsNullOrWhiteSpace())
                        {
                            if (att.IsRequired)
                                throw new ClaimNotFoundException(att.ClaimTypeName);
                        }
                        else
                        {
                            var value = field.TypeConverter.ConvertFrom(valueString);
                            property.SetValue(webQueryDto, value);
                        }
                    });
            }
        }
    }
}
