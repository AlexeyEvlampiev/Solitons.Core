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
    sealed class WebQueryConverter : IWebQueryConverter
    {
        private readonly List<IHttpTriggerMetadata> _resources = new();
        private readonly Dictionary<Type, IHttpTriggerMetadata> _resourceByType = new();
        private readonly Dictionary<Type, Func<object, Version, string>> _uriFactoryByType = new();
        private readonly Dictionary<Type, PropertyInfo[]> _webPropertiesByType = new();
        private readonly Dictionary<PropertyInfo, RouteSegmentAttribute> _uriSegmentByProperty = new();
        private readonly Dictionary<PropertyInfo, QueryParameterAttribute> _queryParameterByProperty = new();



        private WebQueryConverter(IEnumerable<IHttpTriggerMetadata> attributes)
        {
            if (attributes == null) throw new ArgumentNullException(nameof(attributes));
            _resources.AddRange(attributes);
            _resources.ForEach(r=> _resourceByType.Add(r.TargetType, r));
            

            var webProperties = new List<PropertyInfo>();
            foreach (var type in _resources
                .Select(r => r.TargetType))
            {
                webProperties.Clear();
                var allProperties = type
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (var property in allProperties)
                {
                    var segment = RouteSegmentAttribute.TryGet(property);
                    if (segment != null)
                    {
                        var converter = TypeDescriptor.GetConverter(property.PropertyType);
                        if (converter.CanConvertFrom(typeof(string)))
                        {
                            segment.TypeConverter = converter;
                            _uriSegmentByProperty.Add(property, segment);
                            webProperties.Add(property);
                            if (property.SetMethod == null) throw new InvalidOperationException(new StringBuilder("Property setter is missing.")
                                  .Append($" See {type}.{property.Name}")
                                  .ToString());
                            continue;
                        }

                        throw new InvalidOperationException(new StringBuilder($"{property.PropertyType} cannot be converted from {typeof(string)}.")
                            .Append($" See {type}")
                            .ToString());
                    }

                    var queryParameter = QueryParameterAttribute.Get(property);
                    if (queryParameter != null)
                    {
                        _queryParameterByProperty.Add(property, queryParameter);
                        webProperties.Add(property);
                    }
                }
                _webPropertiesByType.Add(type, webProperties.ToArray());

            }

            _resources.ForEach(r => _uriFactoryByType.Add(r.TargetType, CreateUriFactory(r)));

        }

        public static Dictionary<Type, IWebQueryConverter> Discover(IEnumerable<Assembly> assemblies)
        {
            if (assemblies is null) throw new ArgumentNullException(nameof(assemblies));
            return HttpTriggerAttribute
                .Discover(assemblies)
                .GroupBy(attribute => attribute.GetType())
                .ToDictionary(grp => grp.Key, grp => (IWebQueryConverter)new WebQueryConverter(grp));
        }


        public static Dictionary<Type, IWebQueryConverter> Discover(IEnumerable<Type> types)
        {
            if (types is null) throw new ArgumentNullException(nameof(types));
            var attributes = HttpTriggerAttribute
                .Discover(types)
                .ToList();
            var converter = new WebQueryConverter(attributes);
            var result = attributes
                .GroupBy(attribute => attribute.GetType())
                .ToDictionary(grp => grp.Key, grp => (IWebQueryConverter)new WebQueryConverter(grp));
            result.Add(typeof(IHttpTriggerMetadata), converter);
            result.Add(typeof(HttpTriggerAttribute), converter);
            return result;
        }

        private Func<object, Version, string> CreateUriFactory(IHttpTriggerMetadata httpTriggerMetadata)
        {
            var properties = _webPropertiesByType[httpTriggerMetadata.TargetType];
            var format = httpTriggerMetadata.UriRegexp.Replace(@"\b", String.Empty);
            var segmentParameters = new List<PropertyInfo>();
            var queryParameters = new List<QueryParameterAttribute>();
            foreach (var propertyInfo in properties)
            {
                if (_uriSegmentByProperty.TryGetValue(propertyInfo, out var segment))
                {
                    format = Regex.Replace(format, @$"[(][?][<]{segment.RegexGroupName}[>][^)]+[)]", $"{{{segmentParameters.Count}}}");
                    segmentParameters.Add(propertyInfo);
                }
                else if(_queryParameterByProperty.TryGetValue(propertyInfo, out var parameter))
                {
                    queryParameters.Add(parameter);
                }

            }


            string Convert(object target, Version version)
            {
                var parameters = new object[segmentParameters.Count];
                for (int i = 0; i < parameters.Length; i++)
                {
                    var value = segmentParameters[i].GetValue(target);
                    parameters[i] = HttpUtility.UrlEncode(value?.ToString());
                }

                var uri = string.Format(format, parameters);
                var builder = new StringBuilder($"{uri}?version={version}");
                foreach (var queryParameter in queryParameters)
                {
                    var value = queryParameter.TargetProperty.GetValue(target);
                    if (value != null)
                    {
                        builder.AppendFormat("&{0}={1}", queryParameter.ParameterName,
                            HttpUtility.UrlEncode(value.ToString()));
                    }
                    else if(queryParameter.IsRequired)
                    {
                        throw new InvalidOperationException();
                    }
                }
                return builder.ToString();
            }


            return Convert;
        }

        public HttpRequestMessage ToHttpRequestMessage(object dto, HttpMethod method, Version apiVersion)
        {
            if (false == _resourceByType.TryGetValue(dto.GetType(), out var resource))
                throw new NotSupportedException();

            var uri = _uriFactoryByType[dto.GetType()].Invoke(dto, apiVersion);

            var result = new HttpRequestMessage(method, uri);
            return result;
        }

        public object ToDataTransferObject(IWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            request = request.AsWebRequest().ThrowIfNullArgument(nameof(request));
            var version = request.ClientVersion.ToString();

            var resource = _resources
                .FirstOrDefault(r => r.IsUriMatch(request.Uri) && r.IsVersionMatch(version));
            if (resource is null) return null;

            var webQueryDto = Activator.CreateInstance(resource.TargetType);
            Populate(webQueryDto, resource, request);
            return webQueryDto;

        }

        private void Populate(object webQueryDto, IHttpTriggerMetadata route, IWebRequest request)
        {
            var match = route.MatchUri(request.Uri);
            Debug.Assert(match.Success);
            var properties = _webPropertiesByType[webQueryDto.GetType()];
            foreach (var property in properties)
            {
                if (_uriSegmentByProperty.TryGetValue(property, out var uriSegment))
                {
                    var group = match.Groups[uriSegment.RegexGroupName];
                    if (group.Success)
                    {
                        var value = uriSegment.TypeConverter.ConvertFromInvariantString(group.Value);
                        property.SetValue(webQueryDto, value);
                    }
                    continue;
                }

                if (_queryParameterByProperty.TryGetValue(property, out var queryParameter))
                {
                    var value = queryParameter.GetValue(request);
                    property.SetValue(webQueryDto, value);
                }
            }
        }
    }
}
