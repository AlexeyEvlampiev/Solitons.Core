using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Solitons.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public abstract class HttpServiceAttribute : Attribute, IHttpServiceAttribute
    {
        private readonly Version _currentVersion;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        protected HttpServiceAttribute(string id, Version currentVersion)
        {
            var attributes = GetType().GetCustomAttributes(false);
            var guid = attributes
                .OfType<GuidAttribute>()
                .FirstOrDefault()
                .ThrowIfNull(() => new InvalidOperationException(
                    new StringBuilder($"{typeof(GuidAttribute)} decoration is missing.")
                        .Append($" The {GetType()} type is required to be decorated with {typeof(GuidAttribute)} specifying the API unqiue identifier.")
                        .ToString()));
            var description = attributes
                .OfType<DescriptionAttribute>()
                .FirstOrDefault();

            Guid = Guid.Parse(guid.Value);
            Id = id.Trim();
            Description = string.IsNullOrWhiteSpace(description?.Description)
                ? Id
                : description.Description.Trim();
            _currentVersion = currentVersion.ThrowIfNullArgument(nameof(currentVersion));
        }

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; }

        Version IHttpServiceAttribute.CurrentVersion => _currentVersion;
    }
}
