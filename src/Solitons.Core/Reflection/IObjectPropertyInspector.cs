using System.Reflection;

namespace Solitons.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IObjectPropertyInspector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        void Inspect(object target, PropertyInfo property);
    }
}
