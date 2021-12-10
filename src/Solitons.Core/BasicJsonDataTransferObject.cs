using Solitons.Common;
using System.Diagnostics;

namespace Solitons
{

    /// <summary>
    /// JSON- first Data Transfer Object. When used as a base class, ensures that the overriden <see cref="object.ToString"/> returns the objects json representation.    
    /// </summary>
    /// <remarks>
    /// Implies an implicit <see cref="DataTransferObjectAttribute"/> declaration.
    /// </remarks>
    /// <seealso cref="IBasicJsonDataTransferObject"/>
    public abstract class BasicJsonDataTransferObject : SerializationCallback, IBasicJsonDataTransferObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public sealed override string ToString() => this.ToJsonString();
    }

    public static partial class Extensions
    {
        [DebuggerStepThrough]
        public static T ConvertTo<T>(this string self) where T : BasicJsonDataTransferObject =>
            IBasicJsonDataTransferObject.Parse<T>(self);
    }
}
