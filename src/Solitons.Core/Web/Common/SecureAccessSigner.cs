using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Solitons.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SecureAccessSigner : ISecureAccessSigner
    {
        private readonly Regex _queryRegex = new Regex("(?:[?].*$)|(?:[/]$)");
        protected abstract IEnumerable<PropertyInfo> GetProperties(Type type);
        protected abstract TypeConverter GetConverter(PropertyInfo property);
        protected abstract ISecureAccessSignatureMetadata GetAttribute(PropertyInfo property);


        private int Sign(
            object dto, 
            IPAddress startAddress, 
            IPAddress endAddress,
            ISet<object> signedItems)
        {
            if (false == signedItems.Add(dto)) return 0;
            var properties = GetProperties(dto.GetType());
            if (properties is null) return 0;
            int count = 0;
            foreach (var property in properties)
            {
                var converter = GetConverter(property);
                var propertyValue = property.GetValue(dto);
                if (propertyValue is null) continue;
                var attribute = GetAttribute(property);
                if (attribute is null)
                {
                    count += Sign(propertyValue, startAddress, endAddress, signedItems);
                }
                else
                {
                    var uri = propertyValue
                        .ToString()
                        .Replace(_queryRegex, String.Empty)
                        .ThrowIfNotUri(UriKind.RelativeOrAbsolute, () => new InvalidCastException(
                            new StringBuilder($"Expected well formed absolute or relative URI.")
                                .Append($" Actual '{property.GetValue(dto)}'.")
                                .Append($" See property {dto.GetType()}.{property.Name}.")
                                .ToString()));

                    var signedUri = Sign(uri, startAddress, endAddress, attribute)
                        .ThrowIfNull(() => new NullReferenceException($"{GetType()}.{nameof(Sign)} returned null."))
                        .ThrowIfNotUri(UriKind.Absolute, () => new InvalidOperationException($"{GetType()}.{nameof(Sign)} created invalid uri. Expected a well formed absolute uri"));


                    propertyValue = converter.CanConvertFrom(signedUri.GetType())
                        ? converter.ConvertFrom(signedUri)
                        : property.PropertyType == typeof(string)
                            ? signedUri.ToString()
                            : throw new InvalidCastException();
                    property.SetValue(dto, propertyValue);
                    count++;
                }
                
            }

            return count;
        }

        [DebuggerStepThrough]
        Uri ISecureAccessSigner.Sign(string resource, ISecureAccessSignatureMetadata protection) =>
            Sign(
                resource.ThrowIfNullOrWhiteSpaceArgument(nameof(resource)), 
                null, 
                null, 
                protection.ThrowIfNullArgument(nameof(protection)));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="startAddress"></param>
        /// <param name="endtAddress"></param>
        /// <param name="protection"></param>
        /// <returns></returns>
        protected abstract Uri Sign(        
            string resource, 
            IPAddress startAddress,
            IPAddress endtAddress,
            ISecureAccessSignatureMetadata protection);


        ///
        protected abstract bool CanSign(ISecureAccessSignatureMetadata declaration);

        int ISecureAccessSigner.Sign(object dto) => 
            Sign(
                dto.ThrowIfNullArgument(nameof(dto)), 
                null, 
                null, 
                new HashSet<object>());

        [DebuggerStepThrough]
        int ISecureAccessSigner.Sign(object dto, IPAddress startAddress, IPAddress endAddress) => 
            Sign(
                dto.ThrowIfNullArgument(nameof(dto)), 
                startAddress.ThrowIfNullArgument(nameof(startAddress)),
                endAddress.ThrowIfNullArgument(nameof(endAddress)),
                new HashSet<object>());

        [DebuggerStepThrough]
        bool ISecureAccessSigner.CanSign(ISecureAccessSignatureMetadata declaration) => declaration is not null && CanSign(declaration);

        [DebuggerStepThrough]
        Uri ISecureAccessSigner.Sign(string resource, IPAddress startAddress, IPAddress endAddress, ISecureAccessSignatureMetadata protection) =>
            Sign(
                resource.ThrowIfNullOrWhiteSpaceArgument(nameof(resource)),
                startAddress.ThrowIfNullArgument(nameof(startAddress)),
                endAddress.ThrowIfNullArgument(nameof(endAddress)),
                protection.ThrowIfNullArgument(nameof(protection))
            );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SecureAccessSigner<T> : SecureAccessSigner where T : ISecureAccessSignatureMetadata
    {
        private readonly Dictionary<Type, PropertyInfo[]> _typeProperties;
        private readonly Dictionary<PropertyInfo, T> _propertyAttributes;
        private readonly Dictionary<PropertyInfo, TypeConverter> _propertyConverters;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <param name="protection"></param>
        /// <returns></returns>
        protected abstract Uri SpecializedSign(string resource, IPAddress startAddress, IPAddress endAddress, T protection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainContext"></param>
        protected SecureAccessSigner(DomainContext domainContext)
        {
            if (domainContext == null) throw new ArgumentNullException(nameof(domainContext));
            _propertyAttributes = domainContext
                .GetSecureAccessSignatureDeclarations<T>() 
                .ToDictionary(sas=> sas.TargetProperty);
            _typeProperties = domainContext
                .GetTypes()
                .Select(t=> KeyValuePair.Create(t, t.GetProperties(BindingFlags.Public|BindingFlags.Instance|BindingFlags.GetProperty)))
                .ToDictionary();
            _propertyConverters = _propertyAttributes
                .Select(pair =>
                {
                    var converter = TypeDescriptor.GetConverter(pair.Key.PropertyType);
                    return KeyValuePair.Create(pair.Key, converter);
                })
                .ToDictionary();
        }

        protected sealed override IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return _typeProperties.TryGetValue(type, out var properties)
                ? properties
                : Enumerable.Empty<PropertyInfo>();
        }

        protected sealed override ISecureAccessSignatureMetadata GetAttribute(PropertyInfo property)
        {
            return _propertyAttributes.TryGetValue(property, out var attribute)
                ? attribute
                : null;
        }


        protected sealed override TypeConverter GetConverter(PropertyInfo property)
        {
            return _propertyConverters.TryGetValue(property, out var converter)
                ? converter
                : null;
        }

        protected sealed override Uri Sign(string resource, IPAddress startAddress, IPAddress endAddress, ISecureAccessSignatureMetadata metadata)
        {
            if (metadata is T targetAttribute)
            {
                return SpecializedSign(resource, startAddress, endAddress, targetAttribute);
            }

            throw new NotSupportedException($"{metadata.GetType()} type is not supported.");
        }

        protected override bool CanSign(ISecureAccessSignatureMetadata declaration) => declaration is T;
    }
}
