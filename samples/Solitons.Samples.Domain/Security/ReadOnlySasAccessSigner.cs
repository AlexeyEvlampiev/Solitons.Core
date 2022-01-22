using System.Reflection;
using Solitons.Reflection.Common;

namespace Solitons.Samples.Domain.Security
{
    public abstract class ReadOnlySasAccessSigner : DeclarativePropertyInspector<ReadOnlySasAccessAttribute>
    {
        protected abstract string Sign(string url, ReadOnlySasAccessAttribute attribute);

        protected sealed override void Inspect(object target, PropertyInfo property, ReadOnlySasAccessAttribute attribute)
        {
            if (typeof(string) == property.PropertyType)
            {
                var value = property.GetValue(target)?.ToString();
                if (value is null) return;
                value = Sign(value, attribute);
                property.SetValue(target, value);
            }
            else if (typeof(Uri) == property.PropertyType)
            {
                var value = property.GetValue(target)?.ToString();
                if (value is null) return;
                value = Sign(value, attribute);
                property.SetValue(target, new Uri(value));
            }
            else if (typeof(ICollection<string>).IsAssignableFrom(property.PropertyType))
            {
                var collection = (ICollection<string>)property.GetValue(target)!;
                var originalValues = collection.ToList();
                collection.Clear();
                foreach (var uri in originalValues)
                {
                    collection.Add(Sign(uri, attribute));
                }

            }
            else if (typeof(ICollection<Uri>).IsAssignableFrom(property.PropertyType))
            {
                var collection = (ICollection<Uri>)property.GetValue(target)!;
                var originalValues = collection.ToList();
                collection.Clear();
                foreach (var uri in originalValues)
                {
                    var signed = Sign(uri.ToString(), attribute);
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

        protected sealed override bool IsTargetProperty(PropertyInfo property)
        {
            var result = base.IsTargetProperty(property);
            if (result)
            {
                if (typeof(string) == property.PropertyType ||
                    typeof(Uri) == property.PropertyType)
                {
                    if (property.CanRead == false ||
                        property.CanWrite == false)
                        throw new InvalidOperationException("Invalid property inspection request.".Append(builder => builder
                            .Append($" Both property getter and setter are required.")
                            .Append($" See {property.DeclaringType}.{property.Name}")));
                }
                else if(typeof(ICollection<string>).IsAssignableFrom(property.PropertyType) ||
                        typeof(ICollection<Uri>).IsAssignableFrom(property.PropertyType))
                {
                    if (property.CanRead == false)
                        throw new InvalidOperationException("Invalid property inspection request.".Append(builder => builder
                            .Append($" The property getter is required.")
                            .Append($" See {property.DeclaringType}.{property.Name}")));
                }
                else
                {
                    throw new InvalidOperationException("Invalid property inspection request.".Append(builder => builder
                        .Append($" The {property.PropertyType} property type is not supported.")
                        .Append($" See {property.DeclaringType}.{property.Name}")));
                }

            }
            return result;
        }
    }
}
