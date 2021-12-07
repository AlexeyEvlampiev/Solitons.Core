using System.Diagnostics;

namespace Solitons
{

    /// <summary>
    /// JSON- first Data Transfer Object. When used as a base class, ensures that the overriden <see cref="object.ToString"/> returns the objects json representation.
    /// <seealso cref="IBasicDataTransferObject"/>
    /// <seealso cref="IBasicJsonDataTransferObject"/>
    /// </summary>
    public abstract class BasicJsonDataTransferObject : IBasicJsonDataTransferObject
    {
        public sealed override string ToString() => this.ToJsonString();
    }

    public static class JsonDataTransferObjectExtensions
    {
        [DebuggerStepThrough]
        public static T ConvertTo<T>(this string self) where T : BasicJsonDataTransferObject =>
            IBasicJsonDataTransferObject.FromJson<T>(self);
    }
}
