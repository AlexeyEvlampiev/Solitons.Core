using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Solitons.Data
{

    sealed class MediaContentTypeEqualityComparer : EqualityComparer<string>
    {
        private MediaContentTypeEqualityComparer()
        {
            
        }

        public static readonly IEqualityComparer<string> Instance = new MediaContentTypeEqualityComparer();

        public new static IEqualityComparer<string> Default => Instance;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool Equals(string? x, string? y)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(x?.Trim(), y?.Trim());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public override int GetHashCode(string contentType) =>
            StringComparer.OrdinalIgnoreCase.GetHashCode(contentType.Trim());
    }
}
