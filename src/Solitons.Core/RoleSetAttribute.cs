using System;
using System.Diagnostics;
using System.Reflection;

namespace Solitons
{
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class RoleSetAttribute : Attribute
    {
        public RoleSetAttribute(string name) : this(name, name)
        {
        }

        public RoleSetAttribute(string name, string description)
        {
            Name = name.ThrowIfNullOrWhiteSpaceArgument(nameof(name)).Trim();
            Description = description.DefaultIfNullOrWhiteSpace(name).Trim();
        }

        public string Name { get; }
        public string Description { get; }

        public Type TargetType { get; private set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public RoleAttribute[] Roles { get; private set; }


        public static RoleSetAttribute Get(Type type)
        {
            var result = (RoleSetAttribute)type?.GetCustomAttribute(typeof(RoleSetAttribute));
            if (result != null)
            {
                result.TargetType = type;
                result.Roles = RoleAttribute.Get(type);
                Debug.WriteLine("");
            }
            return result;
        }

        public override string ToString() => Name;

        public override int GetHashCode() => (TargetType?.GetHashCode()).GetValueOrDefault(base.GetHashCode());
    }
}
