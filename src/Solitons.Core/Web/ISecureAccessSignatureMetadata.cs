using System;
using System.Reflection;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISecureAccessSignatureMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        TimeSpan TimeToLive { get; }

        /// <summary>
        /// 
        /// </summary>
        PropertyInfo TargetProperty { get; }

        /// <summary>
        /// 
        /// </summary>
        Type DeclaringType { get; }
    }
}