using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Solitons.Net;
using Solitons.Reflection.Common;
using Solitons.Security;

namespace Solitons.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BlobSasUriPropertyInspector : 
        AnnotatedObjectPropertyInspector<BlobSasUriAttribute>, 
            ICloneable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobPath"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        protected abstract string GenerateSasUri(string blobPath, BlobSasUriAttribute attribute);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract BlobSasUriPropertyInspector Clone();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        public BlobSasUriPropertyInspector WithIpAddress(IPAddress startAddress, IPAddress endAddress = null)
        {
            var clone = Clone();
            clone.IpRange = new IpRange(startAddress, endAddress);
            return clone;
        }



        /// <summary>
        /// 
        /// </summary>
        public IpRange IpRange { get; private set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="attribute"></param>
        /// <exception cref="InvalidOperationException"></exception>
        protected sealed override void Inspect(
            object target, 
            PropertyInfo property, 
            BlobSasUriAttribute attribute)
        {

            if (typeof(string) == property.PropertyType)
            {
                var value = property.GetValue(target)?.ToString();
                if (value is null) return;
                value = GenerateSasUri(value, attribute);
                property.SetValue(target, value);
            }
            else if (typeof(Uri) == property.PropertyType)
            {
                var value = property.GetValue(target)?.ToString();
                if (value is null) return;
                value = GenerateSasUri(value, attribute);
                property.SetValue(target, new Uri(value));
            }
            else if (typeof(ICollection<string>).IsAssignableFrom(property.PropertyType))
            {
                var collection = (ICollection<string>)property.GetValue(target)!;
                var originalValues = collection.ToList();
                collection.Clear();
                foreach (var uri in originalValues)
                {
                    collection.Add(GenerateSasUri(uri, attribute));
                }

            }
            else if (typeof(ICollection<Uri>).IsAssignableFrom(property.PropertyType))
            {
                var collection = (ICollection<Uri>)property.GetValue(target)!;
                var originalValues = collection.ToList();
                collection.Clear();
                foreach (var uri in originalValues)
                {
                    var signed = GenerateSasUri(uri.ToString(), attribute);
                    collection.Add(new Uri(signed));
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid property inspection request.".Append(builder => builder
                    .Append($" {property.PropertyType} property type is not supported.")
                    .Append($" See {property.PropertyType}.{property.Name}")));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected override bool IsTargetProperty(PropertyInfo property, BlobSasUriAttribute attribute)
        {
            var result = base.IsTargetProperty(property, attribute);
            if (!result) return false;

            if (typeof(string) == property.PropertyType ||
                typeof(Uri) == property.PropertyType)
            {
                if (property.CanRead == false ||
                    property.CanWrite == false)
                    throw new InvalidOperationException("Invalid property inspection request.".Append(builder => builder
                        .Append($" Both property getter and setter are required.").Append((string?)$" See {property.DeclaringType}.{property.Name}")));
            }
            else if (typeof(ICollection<string>).IsAssignableFrom(property.PropertyType) ||
                     typeof(ICollection<Uri>).IsAssignableFrom(property.PropertyType))
            {
                if (property.CanRead == false)
                    throw new InvalidOperationException("Invalid property inspection request.".Append(builder => builder
                        .Append($" The property getter is required.").Append((string?)$" See {property.DeclaringType}.{property.Name}")));
            }
            else
            {
                throw new InvalidOperationException("Invalid property inspection request.".Append(builder => builder.Append((string?)$" The {property.PropertyType} property type is not supported.").Append((string?)$" See {property.DeclaringType}.{property.Name}")));
            }

            return true;
        }

        object ICloneable.Clone() => Clone();
    }
}
