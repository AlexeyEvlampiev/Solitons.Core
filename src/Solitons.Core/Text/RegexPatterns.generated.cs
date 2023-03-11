namespace Solitons.Text
{
    public static partial class RegexPatterns
    {
        /// <summary>
        /// Contains regular expression patterns for matching UUIDs in various formats.
        /// </summary>
        public static class Uuid
        {
            /// <summary>
            /// Matches UUIDs in a loose format without braces, with each segment separated by a hyphen.
            /// </summary>
            public const string LooseFormatNoBraces = @"[0-9a-fA-F]{8}-?([0-9a-fA-F]{4}-?){3}[0-9a-fA-F]{12}";

            /// <summary>
            /// Matches UUIDs in a loose format with optional braces, with each segment separated by a hyphen.
            /// </summary>
            public const string LooseFormatWithBraces = "(?:[0-9a-fA-F]{8}-?([0-9a-fA-F]{4}-?){3}[0-9a-fA-F]{12}|[{][0-9a-fA-F]{8}-?([0-9a-fA-F]{4}-?){3}[0-9a-fA-F]{12}[}])";

            /// <summary>
            /// Matches UUIDs in a strict format, requiring either no braces or braces surrounding the entire UUID, with each segment separated by a hyphen.
            /// </summary>
            public const string StrictFormat = "^(?:[0-9a-fA-F]{8}-?([0-9a-fA-F]{4}-?){3}[0-9a-fA-F]{12}|[{][0-9a-fA-F]{8}-?([0-9a-fA-F]{4}-?){3}[0-9a-fA-F]{12}[}])$";
        }
    }
}
