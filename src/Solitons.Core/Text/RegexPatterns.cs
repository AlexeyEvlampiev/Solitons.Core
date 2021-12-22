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
        public const string Email = @"^[a-zA-Z0-9.!#$%&''*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
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
