using System.Reflection;

namespace Solitons.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPropertyInspector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        void Inspect(object target, PropertyInfo property);
    }
}
