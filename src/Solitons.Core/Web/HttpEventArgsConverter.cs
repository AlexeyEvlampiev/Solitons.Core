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
            IHttpEventArgsMetadata HttpEventArgsMetadata,
            Type HttpEventArgsType,
            RequestBoundField[] WebBoundFields);

        private readonly Dictionary<IHttpEventArgsMetadata, HttpEventArgsEntry> _entries;


        internal HttpEventArgsConverter(IEnumerable<Type> types)
        {
            var webPropertyAttributeTypes = new HashSet<Type>()
            {
                typeof(UrlParameterAttribute),
                typeof(QueryParameterAttribute),
                typeof(ClaimAttribute)
            };

            _entries =
                (from type in types
                 from httpEventArgsAtt in type.GetCustomAttributes().OfType<IHttpEventArgsMetadata>()
                 where httpEventArgsAtt != null
                 from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                 let att = p.GetCustomAttributes()
                     .Where(a => webPropertyAttributeTypes.Contains(a.GetType()))
                     .ToList()
                 where att.Count > 0
                 let converter = TypeDescriptor.GetConverter(p.PropertyType)
                 let webProperty = new RequestBoundField(p, converter, att.ToArray())
                 group webProperty by new
                 {
                     HttpEventArgsType = type,
                     HttpEventArgsMetadata = httpEventArgsAtt
                 } into grp
                 select new HttpEventArgsEntry(
                     grp.Key.HttpEventArgsMetadata,
                     grp.Key.HttpEventArgsType,
                     grp.ToArray())
                ).ToDictionary(i => i.HttpEventArgsMetadata);

        }



        public object Convert(IWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            request = WebRequestProxy.Wrap(request.ThrowIfNullArgument(nameof(request)));
            var version = request.ClientVersion.ToString();

            var match = _entries
                .Keys
                .FirstOrDefault(r =>
                    r.IsUriMatch(request.Uri) &&
                    r.IsMethodMatch(request.Method) &&
                    r.IsVersionMatch(version));

            if (match is null) return null;
            var entry = _entries[match];
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
                            throw new InvalidOperationException($"{property.Name} property is required.");
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
                                throw new InvalidOperationException($"{property.Name} property is required.");
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
