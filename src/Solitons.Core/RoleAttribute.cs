using System;
using System.Collections.Generic;
using System.Reflection;

namespace Solitons
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class RoleAttribute : Attribute
    {
        public RoleAttribute(
            string guid,
            string name,
            string description)
        {
            Guid = Guid.Parse(guid).ThrowIfEmptyArgument(nameof(guid));
            Name = name.ThrowIfNullOrWhiteSpaceArgument(nameof(name)).Trim();
            Description = description.ThrowIfNullOrWhiteSpaceArgument(nameof(description)).Trim();
        }

        public Guid Guid { get; }
        public string Name { get; }
        public string Description { get; }

        public object TargetValue { get; private set; }

        public override string ToString() => Name;

        public static RoleAttribute[] Get(Type type)
        {
            var members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);
            var enumUnderlyingType = Enum.GetUnderlyingType(type);
            var enumValues = Enum.GetValues(type);
            var result = new List<RoleAttribute>(members.Length);
            foreach (var member in members)
            {
                var attribute = member
                    .GetCustomAttribute<RoleAttribute>()
                    .ThrowIfNull(()=>new InvalidOperationException($"{typeof(RoleAttribute)} attribute is missing. See {type}.{member.Name}"));
                attribute.TargetValue = Enum.Parse(type, member.Name);
                

                result.Add(attribute);
            }
            return result.ToArray();
        }
    }
}
