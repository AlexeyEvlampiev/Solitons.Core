using System;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UrlParameterAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="regexGroup"></param>
        public UrlParameterAttribute(string regexGroup)
        {
            RegexGroupName = regexGroup;
        }

        /// <summary>
        /// 
        /// </summary>
        public string RegexGroupName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => RegexGroupName;

    }
}
