using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class BlobSecureAccessSignatureMetadata : Attribute, ISecureAccessSignatureMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="ttlTimeSpan"></param>
        protected BlobSecureAccessSignatureMetadata(BlobSasPermissions permissions, string ttlTimeSpan) : this(permissions, TimeSpan.Parse(ttlTimeSpan))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="ttl"></param>
        protected BlobSecureAccessSignatureMetadata(BlobSasPermissions permissions, TimeSpan ttl)
        {
            Permissions = permissions;
            TimeToLive = ttl;
        }

        public BlobSasPermissions Permissions { get; }
        public TimeSpan TimeToLive { get; }

        public PropertyInfo TargetProperty { get; private set; }
        public Type DeclaringType { get; private set; }

        private static BlobSecureAccessSignatureMetadata Get(PropertyInfo property)
        {
            return property
                .GetCustomAttributes()
                .OfType<BlobSecureAccessSignatureMetadata>()
                .Do(att =>
                {
                    att.TargetProperty = property;
                    att.DeclaringType = property.DeclaringType;
                })
                .SingleOrDefault();
        }



        public static IEnumerable<BlobSecureAccessSignatureMetadata> Discover(Type type)
        {
            return
                from p in type.GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.GetProperty |
                    BindingFlags.SetProperty)
                let att = Get(p)
                where att is not null
                select att;
        }

        public static Dictionary<Type, BlobSecureAccessSignatureMetadata[]> Discover(Type[] types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));
            var groups =
                from t in types
                from att in Discover(t)
                group att by att.GetType();
            return groups
                .ToDictionary(grp => grp.Key, grp => grp.ToArray());
        }
    }
}
