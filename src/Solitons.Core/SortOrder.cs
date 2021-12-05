using System.Runtime.Serialization;

namespace Solitons
{
    /// <summary>
    /// Represents sort order
    /// </summary>
    public enum SortOrder : byte
    {
        /// <summary>
        /// Ascending order
        /// </summary>
        [EnumMember(Value = "asc")]
        Asc = 0,
        /// <summary>
        /// Descending
        /// </summary>
        [EnumMember(Value = "desc")]
        Desc = 1
    }

    public static partial class Extensions
    {
        /// <summary>
        /// Inverts this sort order
        /// </summary>
        /// <param name="self">The sort order to invert.</param>
        /// <returns>The inverted sort order.</returns>
        public static SortOrder Invert(this SortOrder self) =>(SortOrder)((byte)self ^ 1);
    }
}
