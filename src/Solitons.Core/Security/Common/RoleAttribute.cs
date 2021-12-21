using System;

namespace Solitons.Security.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class RoleAttribute : Attribute, IRoleAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        protected RoleAttribute(string name)
        {
            Name = name
                .ThrowIfNullOrWhiteSpaceArgument(nameof(name))
                .Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
    }
}
