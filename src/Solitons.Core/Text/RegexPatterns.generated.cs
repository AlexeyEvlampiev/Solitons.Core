namespace Solitons.Text
{
    public static partial class RegexPatterns
    {
        public static class Uuid
        {
            /// <summary>
            /// 
            /// </summary>
            public const string LooseWithoutBrakets = @"[0-9a-fA-F]{8}-?([0-9a-fA-F]{4}-?){3}[0-9a-fA-F]{12}";

            /// <summary>
            /// 
            /// </summary>
            public const string Loose = "(?:[0-9a-fA-F]{8}-?([0-9a-fA-F]{4}-?){3}[0-9a-fA-F]{12}|[{][0-9a-fA-F]{8}-?([0-9a-fA-F]{4}-?){3}[0-9a-fA-F]{12}[}])";

            /// <summary>
            /// 
            /// </summary>
            public const string Strict = "^(?:[0-9a-fA-F]{8}-?([0-9a-fA-F]{4}-?){3}[0-9a-fA-F]{12}|[{][0-9a-fA-F]{8}-?([0-9a-fA-F]{4}-?){3}[0-9a-fA-F]{12}[}])$";
        }
    }
}
