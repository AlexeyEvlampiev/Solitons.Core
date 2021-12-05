using System;

namespace Solitons.Text
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class RegexPatterns
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Enumeration type</typeparam>
        /// <param name="values"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string GenerateRegexExpression<T>(T values, bool ignoreCase = true) where T : Enum
        {
            var expression = values
                .ToString()
                .Replace(@"\s*,\s*", m => "|");
            var settings = ignoreCase ? "(?si)" : "(?s)";
            return $"{settings}^(?:{expression})$";
        }
    }
}
